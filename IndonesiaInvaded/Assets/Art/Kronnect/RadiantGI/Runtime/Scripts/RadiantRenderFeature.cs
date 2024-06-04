using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_2023_3_OR_NEWER
using UnityEngine.Rendering.RenderGraphModule;
#endif
using UnityEngine.Rendering.Universal;
using DebugView = RadiantGI.Universal.RadiantGlobalIllumination.DebugView;
using DeferredLights = UnityEngine.Rendering.Universal.Internal.DeferredLights;

namespace RadiantGI.Universal {

    public class RadiantRenderFeature : ScriptableRendererFeature {

        public enum RenderingPath {
            Forward,
            Deferred,
            Both
        }


        enum Pass {
            CopyExact,
            Raycast,
            BlurHorizontal,
            BlurVertical,
            Upscale,
            TemporalAccum,
            Albedo,
            Normals,
            Compose,
            Compare,
            FinalGIDebug,
            Specular,
            Copy,
            WideFilter,
            Depth,
            CopyDepth,
            RSM_Debug,
            RSM,
            NFO,
            NFOBlur,
            CopyMultiTaps
        }

        static readonly List<ReflectionProbe> probes = new List<ReflectionProbe>();
        static readonly List<RadiantVirtualEmitter> emitters = new List<RadiantVirtualEmitter>();
        static bool emittersForceRefresh;

        static class ShaderParams {

            // targets
            public static int MainTex = Shader.PropertyToID("_MainTex");
            public static int DownscaledColorAndDepthRT = Shader.PropertyToID("_DownscaledColorAndDepthRT");
            public static int ResolveRT = Shader.PropertyToID("_ResolveRT");
            public static int SourceSize = Shader.PropertyToID("_SourceSize");
            public static int NoiseTex = Shader.PropertyToID("_NoiseTex");
            public static int Downscaled1RT = Shader.PropertyToID("_Downscaled1RT");
            public static int Downscaled1RTA = Shader.PropertyToID("_Downscaled1RTA");
            public static int Downscaled2RT = Shader.PropertyToID("_Downscaled2RT");
            public static int Downscaled2RTA = Shader.PropertyToID("_Downscaled2RTA");
            public static int InputRT = Shader.PropertyToID("_InputRTGI");
            public static int CompareTex = Shader.PropertyToID("_CompareTexGI");
            public static int TempAcum = Shader.PropertyToID("_TempAcum");
            public static int PrevResolve = Shader.PropertyToID("_PrevResolve");
            public static int DownscaledDepthRT = Shader.PropertyToID("_DownscaledDepthRT");
            public static int Probe1Cube = Shader.PropertyToID("_Probe1Cube");
            public static int Probe2Cube = Shader.PropertyToID("_Probe2Cube");
            public static int NFO_RT = Shader.PropertyToID("_NFO_RT");
            public static int NFOBlurRT = Shader.PropertyToID("_NFOBlurRT");

            // RG supplemental ids
            public static int CameraDepthTexture = Shader.PropertyToID("_CameraDepthTexture");
            public static int CameraNormalsTexture = Shader.PropertyToID("_CameraNormalsTexture");
            public static int MotionVectorTexture = Shader.PropertyToID("_MotionVectorTexture");

            // uniforms
            public static int IndirectData = Shader.PropertyToID("_IndirectData");
            public static int RayData = Shader.PropertyToID("_RayData");
            public static int TemporalData = Shader.PropertyToID("_TemporalData");
            public static int WorldToViewDir = Shader.PropertyToID("_WorldToViewDir");
            public static int CompareParams = Shader.PropertyToID("_CompareParams");
            public static int ExtraData = Shader.PropertyToID("_ExtraData");
            public static int ExtraData2 = Shader.PropertyToID("_ExtraData2");
            public static int ExtraData3 = Shader.PropertyToID("_ExtraData3");
            public static int ExtraData4 = Shader.PropertyToID("_ExtraData4");
            public static int ExtraData5 = Shader.PropertyToID("_ExtraData5");
            public static int EmittersPositions = Shader.PropertyToID("_EmittersPositions");
            public static int EmittersBoxMin = Shader.PropertyToID("_EmittersBoxMin");
            public static int EmittersBoxMax = Shader.PropertyToID("_EmittersBoxMax");
            public static int EmittersColors = Shader.PropertyToID("_EmittersColors");
            public static int EmittersCount = Shader.PropertyToID("_EmittersCount");
            public static int RSMIntensity = Shader.PropertyToID("_RadiantShadowMapIntensity");
            public static int StencilValue = Shader.PropertyToID("_StencilValue");
            public static int StencilCompareFunction = Shader.PropertyToID("_StencilCompareFunction");
            public static int SubstractLightingMultiplier = Shader.PropertyToID("_ExtraData4");
            public static int ProbeData = Shader.PropertyToID("_ProbeData");
            public static int Probe1HDR = Shader.PropertyToID("_Probe1HDR");
            public static int Probe2HDR = Shader.PropertyToID("_Probe2HDR");
            public static int BoundsXZ = Shader.PropertyToID("_BoundsXZ");
            public static int DebugDepthMultiplier = Shader.PropertyToID("_DebugDepthMultiplier");
            public static int NFOTint = Shader.PropertyToID("_NFOTint");

            public static int OrganicLightData = Shader.PropertyToID("_OrganicLightData");
            public static int OrganicLightTint = Shader.PropertyToID("_OrganicLightTint");
            public static int OrganicLightOffset = Shader.PropertyToID("_OrganicLightOffset");

            // keywords
            public const string SKW_FORWARD = "_FORWARD";
            public const string SKW_FORWARD_AND_DEFERRED = "_FORWARD_AND_DEFERRED";
            public const string SKW_COMPARE_MODE = "_COMPARE_MODE";
            public const string SKW_USES_BINARY_SEARCH = "_USES_BINARY_SEARCH";
            public const string SKW_USES_MULTIPLE_RAYS = "_USES_MULTIPLE_RAYS";
            public const string SKW_REUSE_RAYS = "_REUSE_RAYS";
            public const string SKW_FALLBACK_1_PROBE = "_FALLBACK_1_PROBE";
            public const string SKW_FALLBACK_2_PROBES = "_FALLBACK_2_PROBES";
            public const string SKW_VIRTUAL_EMITTERS = "_VIRTUAL_EMITTERS";
            public const string SKW_USES_NEAR_FIELD_OBSCURANCE = "_USES_NEAR_FIELD_OBSCURANCE";
            public const string SKW_ORTHO_SUPPORT = "_ORTHO_SUPPORT";
            public const string SKW_DISTANCE_BLENDING = "_DISTANCE_BLENDING";
        }

        static Mesh _fullScreenMesh;

        static Mesh fullscreenMesh {
            get {
                if (_fullScreenMesh != null) {
                    return _fullScreenMesh;
                }
                float num = 1f;
                float num2 = 0f;
                Mesh val = new Mesh();
                _fullScreenMesh = val;
                _fullScreenMesh.SetVertices(new List<Vector3> {
            new Vector3 (-1f, -1f, 0f),
            new Vector3 (-1f, 1f, 0f),
            new Vector3 (1f, -1f, 0f),
            new Vector3 (1f, 1f, 0f)
        });
                _fullScreenMesh.SetUVs(0, new List<Vector2> {
            new Vector2 (0f, num2),
            new Vector2 (0f, num),
            new Vector2 (1f, num2),
            new Vector2 (1f, num)
        });
                _fullScreenMesh.SetIndices(new int[6] { 0, 1, 2, 2, 1, 3 }, (MeshTopology)0, 0, false);
                _fullScreenMesh.UploadMeshData(true);
                return _fullScreenMesh;
            }
        }

        class RadiantPass : ScriptableRenderPass {

            const string RGI_CBUF_NAME = "RadiantGI";
            const float GOLDEN_RATIO = 0.618033989f;
            const int MAX_EMITTERS = 32;

            class PerCameraData {
                public Vector3 lastCameraPosition;
                public RenderTexture rtAcum;
                public int rtAcumCreationFrame;
                public RenderTexture rtBounce;
                public int rtBounceCreationFrame;
                // emitters
                public float emittersSortTime = float.MinValue;
                public Vector3 emittersLastCameraPosition;
                public readonly List<RadiantVirtualEmitter> emittersSorted = new List<RadiantVirtualEmitter>();
            }

            ScriptableRenderer renderer;
            static RadiantRenderFeature settings;
            static RenderTextureDescriptor sourceDesc, cameraTargetDesc;
            static readonly Dictionary<Camera, PerCameraData> prevs = new Dictionary<Camera, PerCameraData>();

            [NonSerialized]
            static public RadiantGlobalIllumination radiant;
            static float goldenRatioAcum;
            static bool usesReprojection, usesCompareMode;
            static Vector3 camPos;
            static Volume[] volumes;
            static Material mat;
            static readonly Vector4 unlimitedBounds = new Vector4(-1e8f, -1e8f, 1e8f, 1e8f);
            static Vector4[] emittersBoxMin, emittersBoxMax, emittersColors, emittersPositions;
            static readonly Plane[] cameraPlanes = new Plane[6];
            static public int computedGIRT;

            public bool Setup(RadiantGlobalIllumination radiant, ScriptableRenderer renderer, RadiantRenderFeature settings, bool isSceneView) {
                if (radiant == null || !radiant.IsActive()) return false;
                RadiantPass.radiant = radiant;

#if UNITY_EDITOR
                if (isSceneView && !radiant.showInSceneView.value) return false;
                if (!Application.isPlaying && !radiant.showInEditMode.value) return false;
#endif

                usesReprojection = radiant.temporalReprojection.value && (!isSceneView || Application.isPlaying);
                usesCompareMode = radiant.compareMode.value && !isSceneView;
                renderPassEvent = settings.renderPassEvent;

#if UNITY_2022_1_OR_NEWER
                if (radiant.temporalReprojection.value) {
                    // when using motion vectors in RG, it needs to execute after motion vector pass which happens BeforeRenderingPostProcessing so we force Radiant to run just before post processing
                    int minRenderPassEvent = (int)RenderPassEvent.BeforeRenderingPostProcessing;
                    if ((int)renderPassEvent < minRenderPassEvent) {
                        renderPassEvent = (RenderPassEvent)minRenderPassEvent;
                    }
                }
#endif
                this.renderer = renderer;
                RadiantPass.settings = settings;
                if (mat == null) {
                    mat = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/Kronnect/RadiantGI_URP"));
                    mat.SetTexture(ShaderParams.NoiseTex, Resources.Load<Texture>("RadiantGI/blueNoiseGI128RGB"));
                }
                mat.SetInt(ShaderParams.StencilValue, radiant.stencilValue.value);
                mat.SetInt(ShaderParams.StencilCompareFunction, radiant.stencilCheck.value ? (int)radiant.stencilCompareFunction.value : (int)CompareFunction.Always);

                return true;
            }

#if UNITY_2023_3_OR_NEWER
            [Obsolete]
#endif
            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {
                ConfigureInput(GetRequiredInputs());
            }

            ScriptableRenderPassInput GetRequiredInputs() {
                ScriptableRenderPassInput input = ScriptableRenderPassInput.Color;
                if (settings.renderingPath == RenderingPath.Forward) {
                    input |= ScriptableRenderPassInput.Normal | ScriptableRenderPassInput.Depth;
                }
                if (usesReprojection) {
                    input |= ScriptableRenderPassInput.Motion;
                }
                return input;
            }

            readonly PassData passData = new PassData();

#if UNITY_2023_3_OR_NEWER
            [Obsolete]
#endif
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {

                sourceDesc = renderingData.cameraData.cameraTargetDescriptor;
                sourceDesc.colorFormat = RenderTextureFormat.ARGBHalf;
                sourceDesc.useMipMap = false;
                sourceDesc.msaaSamples = 1;
                sourceDesc.depthBufferBits = 0;
                cameraTargetDesc = sourceDesc;

                float downsampling = radiant.downsampling.value;
                sourceDesc.width = (int)(sourceDesc.width / downsampling);
                sourceDesc.height = (int)(sourceDesc.height / downsampling);

                Camera cam = renderingData.cameraData.camera;
                camPos = cam.transform.position;

                CommandBuffer cmd = CommandBufferPool.Get(RGI_CBUF_NAME);
                cmd.Clear();

#if UNITY_2022_2_OR_NEWER
                passData.source = renderer.cameraColorTargetHandle;
#else
                passData.source = renderer.cameraColorTarget;
#endif

                passData.cmd = cmd;
                passData.cam = cam;
                RenderGI(passData);

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            class PassData {
                public CommandBuffer cmd;
                public Camera cam;
#if UNITY_2022_2_OR_NEWER
                public RTHandle source;
#else
                public RenderTargetIdentifier source;
#endif
#if UNITY_2023_3_OR_NEWER
                public TextureHandle colorTexture, depthTexture, cameraNormalsTexture, motionVectorTexture;
#endif
            }

#if UNITY_2023_3_OR_NEWER
            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData) {

                using (var builder = renderGraph.AddUnsafePass<PassData>("Radiant GI RG Pass", out var passData)) {

                    builder.AllowPassCulling(false);

                    ConfigureInput(GetRequiredInputs());

                    UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
                    passData.cam = cameraData.camera;
                    sourceDesc = cameraData.cameraTargetDescriptor;
                    sourceDesc.colorFormat = RenderTextureFormat.ARGBHalf;
                    sourceDesc.useMipMap = false;
                    sourceDesc.msaaSamples = 1;
                    sourceDesc.depthBufferBits = 0;
                    cameraTargetDesc = sourceDesc;

                    float downsampling = radiant.downsampling.value;
                    sourceDesc.width = (int)(sourceDesc.width / downsampling);
                    sourceDesc.height = (int)(sourceDesc.height / downsampling);

                    camPos = passData.cam.transform.position;

                    UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

                    passData.colorTexture = resourceData.activeColorTexture;
                    builder.UseTexture(resourceData.activeColorTexture, AccessFlags.ReadWrite);

                    if (settings.renderingPath == RenderingPath.Forward) {
                        builder.UseTexture(resourceData.cameraNormalsTexture);
                        passData.cameraNormalsTexture = resourceData.cameraNormalsTexture;
                        builder.UseTexture(resourceData.cameraDepthTexture);
                        passData.depthTexture = resourceData.cameraDepthTexture;
                    }
                    if (usesReprojection) {
                        builder.UseTexture(resourceData.motionVectorColor);
                        passData.motionVectorTexture = resourceData.motionVectorColor;
                    }

                    builder.SetRenderFunc((PassData passData, UnsafeGraphContext context) => {

                        CommandBuffer cmd = CommandBufferHelpers.GetNativeCommandBuffer(context.cmd);

                        if (passData.depthTexture.IsValid()) {
                            cmd.SetGlobalTexture(ShaderParams.CameraDepthTexture, passData.depthTexture);
                        }
                        if (passData.cameraNormalsTexture.IsValid()) {
                            cmd.SetGlobalTexture(ShaderParams.CameraNormalsTexture, passData.cameraNormalsTexture);
                        }
                        if (usesReprojection && passData.motionVectorTexture.IsValid()) {
                            cmd.SetGlobalTexture(ShaderParams.MotionVectorTexture, passData.motionVectorTexture);
                        }

                        passData.source = passData.colorTexture;
                        passData.cmd = cmd;
                        RenderGI(passData);

                    });
                }
            }
#endif

            static void RenderGI(PassData passData) {

#if UNITY_2022_2_OR_NEWER
                RTHandle source = passData.source;
#else
                RenderTargetIdentifier source = passData.source;
#endif

                CommandBuffer cmd = passData.cmd;
                Camera cam = passData.cam;

                int smoothing = radiant.smoothing.value;
                DebugView debugView = radiant.debugView.value;
                bool usesBounce = radiant.rayBounce.value;
                int frameCount = Application.isPlaying ? Time.frameCount : 0;
                bool usesForward = settings.renderingPath != RenderingPath.Deferred;
                float normalMapInfluence = radiant.normalMapInfluence.value;
                float lumaInfluence = radiant.lumaInfluence.value > 0 ? radiant.lumaInfluence.value * 100f : 20000;
                float downsampling = radiant.downsampling.value;
                int currentFrame = Time.frameCount;
                bool usesRSM = RadiantShadowMap.installed && radiant.fallbackReflectiveShadowMap.value && radiant.reflectiveShadowMapIntensity.value > 0;
                bool usesEmitters = radiant.virtualEmitters.value;

                // pass radiant settings to shader
                mat.SetVector(ShaderParams.IndirectData, new Vector4(radiant.indirectIntensity.value, radiant.indirectMaxSourceBrightness.value, radiant.indirectDistanceAttenuation.value, radiant.rayReuse.value));
                mat.SetVector(ShaderParams.RayData, new Vector4(radiant.rayCount.value, radiant.rayMaxLength.value, radiant.rayMaxSamples.value, radiant.thickness.value));

                // some uniforms required by compare render feature so declared as global vectors instead of material properties
                cmd.SetGlobalVector(ShaderParams.ExtraData2, new Vector4(radiant.brightnessThreshold.value, radiant.brightnessMax.value, radiant.saturation.value, radiant.reflectiveShadowMapIntensity.value)); // global because these params are needed by the compare pass

                mat.DisableKeyword(ShaderParams.SKW_FORWARD_AND_DEFERRED);
                mat.DisableKeyword(ShaderParams.SKW_FORWARD);
                if (usesForward) {
                    if (settings.renderingPath == RenderingPath.Both) {
                        mat.EnableKeyword(ShaderParams.SKW_FORWARD_AND_DEFERRED);
                    } else {
                        mat.EnableKeyword(ShaderParams.SKW_FORWARD);
                    }
                }

                if (radiant.rayBinarySearch.value) {
                    mat.EnableKeyword(ShaderParams.SKW_USES_BINARY_SEARCH);
                } else {
                    mat.DisableKeyword(ShaderParams.SKW_USES_BINARY_SEARCH);
                }

                if (radiant.rayCount.value > 1) {
                    mat.EnableKeyword(ShaderParams.SKW_USES_MULTIPLE_RAYS);
                } else {
                    mat.DisableKeyword(ShaderParams.SKW_USES_MULTIPLE_RAYS);
                }

                float nearFieldObscurance = radiant.nearFieldObscurance.value;
                bool useNFO = nearFieldObscurance > 0;
                if (useNFO) {
                    cmd.SetGlobalVector(ShaderParams.ExtraData4, new Vector4(radiant.nearFieldObscuranceMaxCameraDistance.value, (1f - radiant.nearFieldObscuranceOccluderDistance.value) * 10f, 0, 0));
                    cmd.SetGlobalColor(ShaderParams.NFOTint, radiant.nearFieldObscuranceTintColor.value);
                    mat.EnableKeyword(ShaderParams.SKW_USES_NEAR_FIELD_OBSCURANCE);
                } else {
                    mat.DisableKeyword(ShaderParams.SKW_USES_NEAR_FIELD_OBSCURANCE);
                }

                if (cam.orthographic) {
                    mat.EnableKeyword(ShaderParams.SKW_ORTHO_SUPPORT);
                } else {
                    mat.DisableKeyword(ShaderParams.SKW_ORTHO_SUPPORT);
                }
                cmd.SetGlobalVector(ShaderParams.ExtraData3, new Vector4(radiant.aoInfluence.value, radiant.nearFieldObscuranceSpread.value * 0.5f, 1f / (radiant.nearCameraAttenuation.value + 0.0001f), nearFieldObscurance));  // global because these params are needed by the compare pass

                // restricts to volume bounds?
                SetupVolumeBounds(cmd);

                // pass reprojection & other raymarch data
                if (usesReprojection) {
                    goldenRatioAcum += GOLDEN_RATIO * radiant.rayCount.value;
                    goldenRatioAcum %= 5000;
                }
                cmd.SetGlobalVector(ShaderParams.SourceSize, new Vector4(cameraTargetDesc.width, cameraTargetDesc.height, goldenRatioAcum, frameCount));
                cmd.SetGlobalVector(ShaderParams.ExtraData, new Vector4(radiant.rayJitter.value, 1f, normalMapInfluence, lumaInfluence));
                cmd.SetGlobalVector(ShaderParams.ExtraData5, new Vector4(radiant.specularContribution.value, downsampling, radiant.sourceBrightness.value, radiant.giWeight.value));

                // pass UNITY_MATRIX_V
                cmd.SetGlobalMatrix(ShaderParams.WorldToViewDir, cam.worldToCameraMatrix);

                // create downscaled depth
                RenderTextureDescriptor downDesc = cameraTargetDesc;
                downDesc.width = Mathf.Min(sourceDesc.width, downDesc.width / 2);
                downDesc.height = Mathf.Min(sourceDesc.height, downDesc.height / 2);

                int downHalfDescWidth = downDesc.width;
                int downHalfDescHeight = downDesc.height;

                // copy depth into an optimized render target
                int downsamplingDepth = 9 - radiant.raytracerAccuracy.value;
                RenderTextureDescriptor rtDownDepth = sourceDesc;
                rtDownDepth.width = Mathf.CeilToInt((float)rtDownDepth.width / downsamplingDepth);
                rtDownDepth.height = Mathf.CeilToInt((float)rtDownDepth.height / downsamplingDepth);
#if UNITY_WEBGL
                rtDownDepth.colorFormat = RenderTextureFormat.RFloat;
#else
                rtDownDepth.colorFormat = RenderTextureFormat.RHalf;
#endif
                rtDownDepth.sRGB = false;
                cmd.GetTemporaryRT(ShaderParams.DownscaledDepthRT, rtDownDepth, FilterMode.Point);
                FullScreenBlit(cmd, ShaderParams.DownscaledDepthRT, Pass.CopyDepth);

                // early debug views
                switch (debugView) {
                    case DebugView.Albedo:
                        FullScreenBlit(cmd, source, Pass.Albedo);
                        return;
                    case DebugView.Normals:
                        FullScreenBlit(cmd, source, Pass.Normals);
                        return;
                    case DebugView.Specular:
                        FullScreenBlit(cmd, source, Pass.Specular);
                        return;
                    case DebugView.Depth:
                        mat.SetFloat(ShaderParams.DebugDepthMultiplier, radiant.debugDepthMultiplier.value);
                        FullScreenBlit(cmd, source, Pass.Depth);
                        return;
                }

                // are we reusing rays?
                if (!prevs.TryGetValue(cam, out PerCameraData frameAcumData)) {
                    prevs[cam] = frameAcumData = new PerCameraData();
                }
                RenderTexture bounceRT = frameAcumData.rtBounce;

                RenderTargetIdentifier raycastInput = source;
                if (usesBounce) {
                    if (bounceRT != null && (bounceRT.width != cameraTargetDesc.width || bounceRT.height != cameraTargetDesc.height)) {
                        bounceRT.Release();
                        bounceRT = null;
                    }
                    if (bounceRT == null) {
                        bounceRT = new RenderTexture(cameraTargetDesc);
                        bounceRT.Create();
                        frameAcumData.rtBounce = bounceRT;
                        frameAcumData.rtBounceCreationFrame = currentFrame;
                    } else {
                        if (currentFrame - frameAcumData.rtBounceCreationFrame > 2) {
                            raycastInput = bounceRT; // only uses bounce rt a few frames after it's created
                        }
                    }
                } else if (bounceRT != null) {
                    bounceRT.Release();
                    DestroyImmediate(bounceRT);
                }

                // virtual emitters
                if (usesEmitters) {
                    float now = Time.time;
                    if (emittersForceRefresh) {
                        emittersForceRefresh = false;
                        foreach (PerCameraData cameraData in prevs.Values) {
                            cameraData.emittersSortTime = float.MinValue;
                        }
                    }
                    if (now - frameAcumData.emittersSortTime > 5 || (frameAcumData.emittersLastCameraPosition - camPos).sqrMagnitude > 25) {
                        frameAcumData.emittersSortTime = now;
                        frameAcumData.emittersLastCameraPosition = camPos;
                        SortEmitters(cam);
                        frameAcumData.emittersSorted.Clear();
                        frameAcumData.emittersSorted.AddRange(emitters);
                    }
                    usesEmitters = SetupEmitters(cam, frameAcumData.emittersSorted);
                }
                if (usesEmitters) {
                    mat.EnableKeyword(ShaderParams.SKW_VIRTUAL_EMITTERS);
                } else {
                    mat.DisableKeyword(ShaderParams.SKW_VIRTUAL_EMITTERS);
                }

                // set the fallback mode
                mat.DisableKeyword(ShaderParams.SKW_REUSE_RAYS);
                mat.DisableKeyword(ShaderParams.SKW_FALLBACK_1_PROBE);
                mat.DisableKeyword(ShaderParams.SKW_FALLBACK_2_PROBES);

                bool usingProbes = false;
                if (radiant.fallbackReflectionProbes.value) {
                    if (SetupProbes(cmd, out int numProbes)) {
                        mat.EnableKeyword(numProbes == 1 ? ShaderParams.SKW_FALLBACK_1_PROBE : ShaderParams.SKW_FALLBACK_2_PROBES);
                        usingProbes = true;
                    }
                }
                if (!usingProbes) {
                    if (radiant.fallbackReuseRays.value && currentFrame - frameAcumData.rtAcumCreationFrame > 2 && radiant.rayReuse.value > 0 && frameAcumData.rtAcum != null) {
                        RenderTargetIdentifier prevRT = new RenderTargetIdentifier(frameAcumData.rtAcum, 0, CubemapFace.Unknown, -1);
                        cmd.SetGlobalTexture(ShaderParams.PrevResolve, prevRT);
                        mat.EnableKeyword(ShaderParams.SKW_REUSE_RAYS);
                    }
                }

                // raycast & resolve
                RenderTextureDescriptor downscaledColorAndDepthDesc = sourceDesc;
                cmd.GetTemporaryRT(ShaderParams.DownscaledColorAndDepthRT, downscaledColorAndDepthDesc, FilterMode.Bilinear);

                cmd.GetTemporaryRT(ShaderParams.ResolveRT, sourceDesc, FilterMode.Bilinear);
                FullScreenBlit(cmd, raycastInput, ShaderParams.ResolveRT, Pass.Raycast);

                cmd.GetTemporaryRT(ShaderParams.Downscaled1RT, downDesc, FilterMode.Bilinear);
                cmd.GetTemporaryRT(ShaderParams.Downscaled1RTA, downDesc, FilterMode.Bilinear);

                // Prepare NFO
                if (useNFO) {
                    RenderTextureDescriptor nfoDesc = downDesc;
                    nfoDesc.colorFormat = RenderTextureFormat.RHalf;
                    cmd.GetTemporaryRT(ShaderParams.NFO_RT, nfoDesc, FilterMode.Bilinear);
                    cmd.GetTemporaryRT(ShaderParams.NFOBlurRT, nfoDesc, FilterMode.Bilinear);
                    FullScreenBlit(cmd, ShaderParams.NFOBlurRT, Pass.NFO);
                    FullScreenBlit(cmd, ShaderParams.NFOBlurRT, ShaderParams.NFO_RT, Pass.NFOBlur);
                }

                // downscale & blur
                downDesc.width /= 2;
                downDesc.height /= 2;
                cmd.GetTemporaryRT(ShaderParams.Downscaled2RT, downDesc, FilterMode.Bilinear);
                int downscaledQuarterRT = ShaderParams.Downscaled2RT;

                switch (smoothing) {
                    case 0:
                        if (downsampling <= 1f) {
                            FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled1RT, Pass.Copy);
                            FullScreenBlit(cmd, ShaderParams.Downscaled1RT, ShaderParams.Downscaled2RT, Pass.WideFilter);
                        } else {
                            cmd.SetGlobalVector(ShaderParams.ExtraData, new Vector4(radiant.rayJitter.value, 1.5f, normalMapInfluence, lumaInfluence));
                            FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled2RT, Pass.WideFilter);
                        }
                        if (usesRSM) {
                            FullScreenBlit(cmd, ShaderParams.Downscaled2RT, Pass.RSM);
                        }
                        break;
                    case 1:
                        cmd.GetTemporaryRT(ShaderParams.Downscaled2RTA, downDesc, FilterMode.Bilinear);
                        if (downsampling <= 1f) {
                            FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled1RT, Pass.Copy);
                            FullScreenBlit(cmd, ShaderParams.Downscaled1RT, ShaderParams.Downscaled2RTA, Pass.Copy);
                        } else {
                            FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled2RTA, Pass.CopyMultiTaps);
                        }
                        if (usesRSM) {
                            FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, Pass.RSM);
                        }
                        FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, ShaderParams.Downscaled2RT, Pass.WideFilter);
                        break;
                    case 2:
                        cmd.GetTemporaryRT(ShaderParams.Downscaled2RTA, downDesc, FilterMode.Bilinear);
                        if (downsampling <= 1f) {
                            FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled1RT, Pass.Copy);
                            FullScreenBlit(cmd, ShaderParams.Downscaled1RT, ShaderParams.Downscaled2RT, Pass.BlurHorizontal);
                            FullScreenBlit(cmd, ShaderParams.Downscaled2RT, ShaderParams.Downscaled2RTA, Pass.BlurVertical);
                            if (usesRSM) {
                                FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, Pass.RSM);
                            }
                            FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, ShaderParams.Downscaled2RT, Pass.WideFilter);
                        } else {
                            FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled2RT, Pass.BlurHorizontal);
                            FullScreenBlit(cmd, ShaderParams.Downscaled2RT, ShaderParams.Downscaled2RTA, Pass.BlurVertical);
                            if (usesRSM) {
                                FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, Pass.RSM);
                            }
                            FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, ShaderParams.Downscaled2RT, Pass.WideFilter);
                        }
                        break;
                    case 4:
                        cmd.GetTemporaryRT(ShaderParams.Downscaled2RTA, downDesc, FilterMode.Bilinear);
                        FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled1RTA, Pass.BlurHorizontal);
                        FullScreenBlit(cmd, ShaderParams.Downscaled1RTA, ShaderParams.Downscaled1RT, Pass.BlurVertical);
                        FullScreenBlit(cmd, ShaderParams.Downscaled1RT, ShaderParams.Downscaled2RT, Pass.BlurHorizontal);
                        FullScreenBlit(cmd, ShaderParams.Downscaled2RT, ShaderParams.Downscaled2RTA, Pass.BlurVertical);
                        if (usesRSM) {
                            FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, Pass.RSM);
                        }
                        FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, ShaderParams.Downscaled2RT, Pass.WideFilter);
                        cmd.SetGlobalVector(ShaderParams.ExtraData, new Vector4(radiant.rayJitter.value, 1.25f, normalMapInfluence, lumaInfluence));
                        FullScreenBlit(cmd, ShaderParams.Downscaled2RT, ShaderParams.Downscaled2RTA, Pass.WideFilter);
                        downscaledQuarterRT = ShaderParams.Downscaled2RTA;
                        break;
                    default:
                        cmd.GetTemporaryRT(ShaderParams.Downscaled2RTA, downDesc, FilterMode.Bilinear);
                        FullScreenBlit(cmd, ShaderParams.ResolveRT, ShaderParams.Downscaled1RTA, Pass.BlurHorizontal);
                        FullScreenBlit(cmd, ShaderParams.Downscaled1RTA, ShaderParams.Downscaled1RT, Pass.BlurVertical);
                        FullScreenBlit(cmd, ShaderParams.Downscaled1RT, ShaderParams.Downscaled2RT, Pass.BlurHorizontal);
                        FullScreenBlit(cmd, ShaderParams.Downscaled2RT, ShaderParams.Downscaled2RTA, Pass.BlurVertical);
                        if (usesRSM) {
                            FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, Pass.RSM);
                        }
                        if (downsampling > 1) {
                            cmd.SetGlobalVector(ShaderParams.ExtraData, new Vector4(radiant.rayJitter.value, 1.25f, normalMapInfluence, lumaInfluence));
                        }
                        FullScreenBlit(cmd, ShaderParams.Downscaled2RTA, ShaderParams.Downscaled2RT, Pass.WideFilter);
                        break;
                }

                // Upscale
                FullScreenBlit(cmd, downscaledQuarterRT, ShaderParams.Downscaled1RTA, Pass.Upscale);

                computedGIRT = ShaderParams.Downscaled1RTA;
                RenderTexture prev = frameAcumData?.rtAcum;

                if (usesReprojection) {

                    if (prev != null && (prev.width != downHalfDescWidth || prev.height != downHalfDescHeight)) {
                        prev.Release();
                        prev = null;
                    }

                    RenderTextureDescriptor acumDesc = sourceDesc;
                    acumDesc.width = downHalfDescWidth;
                    acumDesc.height = downHalfDescHeight;
                    float responseSpeed = radiant.temporalResponseSpeed.value;
                    Pass acumPass = Pass.TemporalAccum;

                    if (prev == null) {
                        prev = new RenderTexture(acumDesc);
                        prev.Create();
                        frameAcumData.rtAcum = prev;
                        frameAcumData.lastCameraPosition = camPos;
                        frameAcumData.rtAcumCreationFrame = currentFrame;
                        acumPass = Pass.Copy;
                    } else {
                        float camTranslationDelta = Vector3.Distance(camPos, frameAcumData.lastCameraPosition);
                        frameAcumData.lastCameraPosition = camPos;
                        responseSpeed += camTranslationDelta * radiant.temporalCameraTranslationResponse.value;
                    }

                    mat.SetVector(ShaderParams.TemporalData, new Vector4(Mathf.Clamp01(responseSpeed * Time.unscaledDeltaTime), radiant.temporalDepthRejection.value, radiant.temporalChromaThreshold.value, 0));

                    RenderTargetIdentifier prevRT = new RenderTargetIdentifier(prev, 0, CubemapFace.Unknown, -1);
                    cmd.SetGlobalTexture(ShaderParams.PrevResolve, prevRT);
                    cmd.GetTemporaryRT(ShaderParams.TempAcum, acumDesc, FilterMode.Bilinear);
                    FullScreenBlit(cmd, computedGIRT, ShaderParams.TempAcum, acumPass);
                    FullScreenBlit(cmd, ShaderParams.TempAcum, prevRT, Pass.CopyExact);
                    computedGIRT = ShaderParams.TempAcum;
                } else if (prev != null) {
                    prev.Release();
                    DestroyImmediate(prev);
                }

                // prepare output blending
                cmd.GetTemporaryRT(ShaderParams.InputRT, cameraTargetDesc, FilterMode.Point);
                FullScreenBlit(cmd, source, ShaderParams.InputRT, Pass.CopyExact);

                if (usesCompareMode) {
                    cmd.GetTemporaryRT(ShaderParams.CompareTex, cameraTargetDesc, FilterMode.Point); // needed by the compare pass
                    if (usesBounce) {
                        FullScreenBlit(cmd, computedGIRT, ShaderParams.CompareTex, Pass.Compose);
                        FullScreenBlit(cmd, ShaderParams.CompareTex, bounceRT, Pass.CopyExact);
                    }
                } else if (usesBounce) {
                    FullScreenBlit(cmd, computedGIRT, bounceRT, Pass.Compose);
                    FullScreenBlit(cmd, bounceRT, passData.source, Pass.CopyExact);
                } else {
                    FullScreenBlit(cmd, computedGIRT, passData.source, Pass.Compose);
                }


                switch (debugView) {
                    case DebugView.DownscaledHalf:
                        FullScreenBlit(cmd, ShaderParams.Downscaled1RT, source, Pass.CopyExact);
                        return;
                    case DebugView.DownscaledQuarter:
                        FullScreenBlit(cmd, downscaledQuarterRT, source, Pass.CopyExact);
                        return;
                    case DebugView.UpscaleToHalf:
                        FullScreenBlit(cmd, ShaderParams.Downscaled1RTA, source, Pass.CopyExact);
                        return;
                    case DebugView.Raycast:
                        FullScreenBlit(cmd, ShaderParams.ResolveRT, source, Pass.CopyExact);
                        return;
                    case DebugView.ReflectiveShadowMap:
                        if (usesRSM) {
                            FullScreenBlit(cmd, source, Pass.RSM_Debug);
                        }
                        return;
                    case DebugView.TemporalAccumulationBuffer:
                        if (usesReprojection) {
                            FullScreenBlit(cmd, ShaderParams.TempAcum, source, Pass.CopyExact);
                        }
                        return;
                    case DebugView.FinalGI:
                        FullScreenBlit(cmd, computedGIRT, source, Pass.FinalGIDebug);
                        return;
                }

            }


            static void FullScreenBlit(CommandBuffer cmd, RenderTargetIdentifier destination, Pass pass) {
                cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
                cmd.DrawMesh(fullscreenMesh, Matrix4x4.identity, mat, 0, (int)pass);
            }

            static void FullScreenBlit(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Pass pass) {
                cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
                cmd.SetGlobalTexture(ShaderParams.MainTex, source);
                cmd.DrawMesh(fullscreenMesh, Matrix4x4.identity, mat, 0, (int)pass);
            }

            static float CalculateProbeWeight(Vector3 wpos, Vector3 probeBoxMin, Vector3 probeBoxMax, float blendDistance) {
                Vector3 weightDir = Vector3.Min(wpos - probeBoxMin, probeBoxMax - wpos) / blendDistance;
                return Mathf.Clamp01(Mathf.Min(weightDir.x, Mathf.Min(weightDir.y, weightDir.z)));
            }


            static bool SetupProbes(CommandBuffer cmd, out int numProbes) {

                numProbes = PickNearProbes(out ReflectionProbe probe1, out ReflectionProbe probe2);
                if (numProbes == 0) return false;
                if (!probe1.bounds.Contains(camPos)) return false;
                if (numProbes >= 2 && !probe2.bounds.Contains(camPos)) numProbes = 1;

                float probe1Weight = 0, probe2Weight = 0;
                if (numProbes >= 1) {
                    Shader.SetGlobalTexture(ShaderParams.Probe1Cube, probe1.texture);
                    Shader.SetGlobalVector(ShaderParams.Probe1HDR, probe1.textureHDRDecodeValues);
                    Bounds probe1Bounds = probe1.bounds;
                    probe1Weight = CalculateProbeWeight(camPos, probe1Bounds.min, probe1Bounds.max, probe1.blendDistance);
                }
                if (numProbes >= 2) {
                    Shader.SetGlobalTexture(ShaderParams.Probe2Cube, probe2.texture);
                    Shader.SetGlobalVector(ShaderParams.Probe2HDR, probe1.textureHDRDecodeValues);
                    Bounds probe2Bounds = probe2.bounds;
                    probe2Weight = CalculateProbeWeight(camPos, probe2Bounds.min, probe2Bounds.max, probe2.blendDistance);
                }
                float probesIntensity = radiant.probesIntensity.value;
                cmd.SetGlobalVector(ShaderParams.ProbeData, new Vector4(probe1Weight * probesIntensity, probe2Weight * probesIntensity, 0, 0));

                return true;
            }

            static int PickNearProbes(out ReflectionProbe probe1, out ReflectionProbe probe2) {
                int probesCount = probes.Count;
                probe1 = probe2 = null;
                if (probesCount == 0) {
                    return 0;
                }
                if (probesCount == 1) {
                    probe1 = probes[0];
                    return 1;
                }

                float probe1Value = float.MaxValue;
                float probe2Value = float.MaxValue;
                for (int k = 0; k < probesCount; k++) {
                    ReflectionProbe probe = probes[k];
                    float probeValue = ComputeProbeValue(camPos, probe);
                    if (probeValue < probe2Value) {
                        probe2 = probe;
                        probe2Value = probeValue;
                        if (probe2Value < probe1Value) {
                            // swap probe1 & probe2
                            probeValue = probe1Value;
                            probe = probe1;
                            probe1 = probe2;
                            probe1Value = probe2Value;
                            probe2 = probe;
                            probe2Value = probeValue;
                        }
                    }
                }
                return 2;

            }

            static float ComputeProbeValue(Vector3 camPos, ReflectionProbe probe) {
                Vector3 probePos = probe.transform.position;
                float d = (probePos - camPos).sqrMagnitude * (probe.importance + 1) * 1000;
                if (!probe.bounds.Contains(camPos)) d += 100000;
                return d;
            }

            static void SetupVolumeBounds(CommandBuffer cmd) {
                if (!radiant.limitToVolumeBounds.value) {
                    cmd.SetGlobalVector(ShaderParams.BoundsXZ, unlimitedBounds);
                    return;
                }
                if (volumes == null) {
                    volumes = VolumeManager.instance.GetVolumes(-1);
                }
                int volumeCount = volumes.Length;
                for (int k = 0; k < volumeCount; k++) {
                    Volume volume = volumes[k];
                    if (volume == null) continue;
                    List<Collider> colliders = volume.colliders;
                    int colliderCount = colliders.Count;
                    for (int j = 0; j < colliderCount; j++) {
                        Collider collider = colliders[j];
                        if (collider == null) continue;
                        Bounds bounds = collider.bounds;
                        if (collider.bounds.Contains(camPos) && volume.sharedProfile.Has<RadiantGlobalIllumination>()) {
                            Vector4 effectBounds = new Vector4(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
                            cmd.SetGlobalVector(ShaderParams.BoundsXZ, effectBounds);
                            return;
                        }
                    }
                }
            }

            static bool SetupEmitters(Camera cam, List<RadiantVirtualEmitter> emitters) {
                // copy emitters data
                if (emittersBoxMax == null || emittersBoxMax.Length != MAX_EMITTERS) {
                    emittersBoxMax = new Vector4[MAX_EMITTERS];
                    emittersBoxMin = new Vector4[MAX_EMITTERS];
                    emittersColors = new Vector4[MAX_EMITTERS];
                    emittersPositions = new Vector4[MAX_EMITTERS];
                }
                int emittersCount = 0;

                const int EMITTERS_BUDGET = 150; // max number of emitters to be processed per frame
                int emittersMax = Mathf.Min(EMITTERS_BUDGET, emitters.Count);

                GeometryUtility.CalculateFrustumPlanes(cam, cameraPlanes);

                for (int k = 0; k < emittersMax; k++) {
                    RadiantVirtualEmitter emitter = emitters[k];

                    // Cull emitters

                    // disabled emitter?
                    if (emitter == null || !emitter.isActiveAndEnabled) continue;

                    // emitter with no intensity or range?
                    if (emitter.intensity <= 0 || emitter.range <= 0) continue;

                    // emitter with black color (nothing to inject)?
                    Vector4 colorAndRange = emitter.GetGIColorAndRange();
                    if (colorAndRange.x == 0 && colorAndRange.y == 0 && colorAndRange.z == 0) continue;

                    // emitter bounds out of camera frustum
                    Bounds emitterBounds = emitter.GetBounds();
                    if (!GeometryUtility.TestPlanesAABB(cameraPlanes, emitterBounds)) continue;

                    // add emitter
                    Vector3 emitterPosition = emitter.transform.position;
                    emittersPositions[emittersCount] = emitterPosition;

                    emittersColors[emittersCount] = colorAndRange;

                    Vector3 boxMin = emitterBounds.min;
                    Vector3 boxMax = emitterBounds.max;

                    float lightRangeSqr = colorAndRange.w * colorAndRange.w;
                    // Commented out for future versions if needed
                    //float fadeStartDistanceSqr = 0.8f * 0.8f * lightRangeSqr;
                    //float fadeRangeSqr = (fadeStartDistanceSqr - lightRangeSqr);
                    //float oneOverFadeRangeSqr = 1.0f / fadeRangeSqr;
                    //float lightRangeSqrOverFadeRangeSqr = -lightRangeSqr / fadeRangeSqr;
                    float oneOverLightRangeSqr = 1.0f / Mathf.Max(0.0001f, lightRangeSqr);

                    float pointAttenX = oneOverLightRangeSqr;
                    //float pointAttenY = lightRangeSqrOverFadeRangeSqr;

                    emittersBoxMin[emittersCount] = new Vector4(boxMin.x, boxMin.y, boxMin.z, pointAttenX);
                    emittersBoxMax[emittersCount] = new Vector4(boxMax.x, boxMax.y, boxMax.z, 0); // pointAttenY

                    emittersCount++;
                    if (emittersCount >= MAX_EMITTERS) break;
                }

                if (emittersCount == 0) return false;

                Shader.SetGlobalVectorArray(ShaderParams.EmittersPositions, emittersPositions);
                Shader.SetGlobalVectorArray(ShaderParams.EmittersBoxMin, emittersBoxMin);
                Shader.SetGlobalVectorArray(ShaderParams.EmittersBoxMax, emittersBoxMax);
                Shader.SetGlobalVectorArray(ShaderParams.EmittersColors, emittersColors);
                Shader.SetGlobalInt(ShaderParams.EmittersCount, emittersCount);

                return true;
            }

            static void SortEmitters(Camera cam) {
                emitters.Sort(EmittersDistanceComparer);
            }

            static int EmittersDistanceComparer(RadiantVirtualEmitter p1, RadiantVirtualEmitter p2) {
                Vector3 p1Pos = p1.transform.position;
                Vector3 p2Pos = p2.transform.position;
                float d1 = (p1Pos - camPos).sqrMagnitude;
                float d2 = (p2Pos - camPos).sqrMagnitude;
                Bounds p1bounds = p1.GetBounds();
                Bounds p2bounds = p2.GetBounds();
                if (!p1bounds.Contains(camPos)) d1 += 100000;
                if (!p2bounds.Contains(camPos)) d2 += 100000;
                if (d1 < d2) return -1; else if (d1 > d2) return 1;
                return 0;
            }

            public void Cleanup() {
                CoreUtils.Destroy(mat);
                if (prevs != null) {
                    foreach (PerCameraData fad in prevs.Values) {
                        if (fad.rtAcum != null) {
                            fad.rtAcum.Release();
                            DestroyImmediate(fad.rtAcum);
                        }
                        if (fad.rtBounce != null) {
                            fad.rtBounce.Release();
                            DestroyImmediate(fad.rtBounce);
                        }
                    }
                    prevs.Clear();
                }
            }

        }


        class RadiantComparePass : ScriptableRenderPass {

            const string RGI_CBUF_NAME = "RadiantGICompare";
            static Material mat;
            static RadiantGlobalIllumination radiant;
            ScriptableRenderer renderer;
            static RadiantRenderFeature settings;

            class PassData {
                public CommandBuffer cmd;
#if UNITY_2022_2_OR_NEWER
                public RTHandle source, sourceDepth;
#else
                public RenderTargetIdentifier source, sourceDepth;
#endif
#if UNITY_2023_3_OR_NEWER
                public TextureHandle colorTexture, depthTexture;
#endif
            }

            readonly PassData passData = new PassData();

            public bool Setup(ScriptableRenderer renderer, RenderPassEvent passEvent, RadiantRenderFeature settings) {

                radiant = VolumeManager.instance.stack.GetComponent<RadiantGlobalIllumination>();
                if (radiant == null || !radiant.IsActive() || radiant.debugView.value != DebugView.None) return false;

#if UNITY_EDITOR
                if (!Application.isPlaying && !radiant.showInEditMode.value) return false;
#endif

                if (!radiant.compareMode.value) return false;

                renderPassEvent = passEvent;
                RadiantComparePass.settings = settings;
                this.renderer = renderer;
                if (mat == null) {
                    mat = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/Kronnect/RadiantGI_URP"));
                }
                return true;
            }

#if UNITY_2023_3_OR_NEWER
            [Obsolete]
#endif
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {

                CommandBuffer cmd = CommandBufferPool.Get(RGI_CBUF_NAME);
                cmd.Clear();

#if UNITY_2022_2_OR_NEWER
                passData.source = renderer.cameraColorTargetHandle;
                passData.sourceDepth = renderer.cameraDepthTargetHandle;
#else
                passData.source = renderer.cameraColorTarget;
                passData.sourceDepth = renderer.cameraDepthTarget;
#endif

                passData.cmd = cmd;

                ExecutePass(passData);

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

#if UNITY_2023_3_OR_NEWER
            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData) {

                using (var builder = renderGraph.AddUnsafePass<PassData>("Radiant GI Compare RG Pass", out var passData)) {

                    builder.AllowPassCulling(false);

                    UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
                    passData.colorTexture = resourceData.activeColorTexture;
                    passData.depthTexture = resourceData.activeDepthTexture;

                    builder.UseTexture(resourceData.activeColorTexture, AccessFlags.ReadWrite);
                    builder.UseTexture(resourceData.activeDepthTexture, AccessFlags.ReadWrite);

                    builder.SetRenderFunc((PassData passData, UnsafeGraphContext context) => {

                        CommandBuffer cmd = CommandBufferHelpers.GetNativeCommandBuffer(context.cmd);
                        passData.cmd = cmd;
                        passData.source = passData.colorTexture;
                        passData.sourceDepth = passData.depthTexture;
                        ExecutePass(passData);
                    });
                }
            }


#endif

            static void ExecutePass(PassData passData) {

                mat.DisableKeyword(ShaderParams.SKW_FORWARD_AND_DEFERRED);
                mat.DisableKeyword(ShaderParams.SKW_FORWARD);
                if (settings.renderingPath == RenderingPath.Both) {
                    mat.EnableKeyword(ShaderParams.SKW_FORWARD_AND_DEFERRED);
                } else if (settings.renderingPath == RenderingPath.Forward) {
                    mat.EnableKeyword(ShaderParams.SKW_FORWARD);
                }

                if (radiant.virtualEmitters.value) {
                    mat.EnableKeyword(ShaderParams.SKW_VIRTUAL_EMITTERS);
                } else {
                    mat.DisableKeyword(ShaderParams.SKW_VIRTUAL_EMITTERS);
                }

                float nearFieldObscurance = radiant.nearFieldObscurance.value;
                if (nearFieldObscurance > 0) {
                    mat.EnableKeyword(ShaderParams.SKW_USES_NEAR_FIELD_OBSCURANCE);
                } else {
                    mat.DisableKeyword(ShaderParams.SKW_USES_NEAR_FIELD_OBSCURANCE);
                }

                float angle = radiant.compareSameSide.value ? Mathf.PI * 0.5f : radiant.compareLineAngle.value;
                mat.SetVector(ShaderParams.CompareParams, new Vector4(Mathf.Cos(angle), Mathf.Sin(angle), radiant.compareSameSide.value ? radiant.comparePanning.value : -10, radiant.compareLineWidth.value));
                mat.SetInt(ShaderParams.StencilValue, radiant.stencilValue.value);
                mat.SetInt(ShaderParams.StencilCompareFunction, radiant.stencilCheck.value ? (int)radiant.stencilCompareFunction.value : (int)CompareFunction.Always);

                CommandBuffer cmd = passData.cmd;
                FullScreenBlit(cmd, passData.source, ShaderParams.InputRT, Pass.CopyExact); // include transparent objects in the original compare texture
                FullScreenBlit(cmd, RadiantPass.computedGIRT, ShaderParams.CompareTex, Pass.Compose); // add gi
                FullScreenBlit(cmd, ShaderParams.InputRT, passData.source, Pass.Compare);    // render the split
            }

            static void FullScreenBlit(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Pass pass) {
                cmd.SetRenderTarget(destination, 0, CubemapFace.Unknown, -1);
                cmd.SetGlobalTexture(ShaderParams.MainTex, source);
                cmd.DrawMesh(fullscreenMesh, Matrix4x4.identity, mat, 0, (int)pass);
            }

            public void Cleanup() {
                CoreUtils.Destroy(mat);
            }
        }



        class RadiantOrganicLightPass : ScriptableRenderPass {

            enum Pass {
                OrganicLight = 0
            }

            const string m_strProfilerTag = "Radiant GI Organic Light";

            static Material mat;
            DeferredLights m_DeferredLights;

            Texture2D noiseTex;
            Vector3 offset;

            public bool Setup(RadiantGlobalIllumination radiant, ScriptableRenderer renderer, bool isSceneView) {

                if (radiant == null || radiant.organicLight.value <= 0) return false;

#if UNITY_EDITOR
                if (isSceneView && !radiant.showInSceneView.value) return false;
                if (!Application.isPlaying && !radiant.showInEditMode.value) return false;
#endif

                DeferredLights deferredLights = ((UniversalRenderer)renderer).deferredLights;
                if (deferredLights == null) return false;

                renderPassEvent = RenderPassEvent.AfterRenderingGbuffer;
                m_DeferredLights = deferredLights;

                if (mat == null) {
                    mat = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/Kronnect/RadiantGIOrganicLight"));
                }

                if (noiseTex == null) {
                    noiseTex = Resources.Load<Texture2D>("RadiantGI/NoiseTex");
                }

                mat.SetTexture(ShaderParams.NoiseTex, noiseTex);
                mat.SetVector(ShaderParams.OrganicLightData, new Vector4(1.001f - radiant.organicLightSpread.value, radiant.organicLight.value, radiant.organicLightThreshold.value, radiant.organicLightNormalsInfluence.value));
                mat.SetColor(ShaderParams.OrganicLightTint, radiant.organicLightTintColor.value);
                offset += radiant.organicLightAnimationSpeed.value * Time.deltaTime;
                offset.x %= 10000f;
                offset.y %= 10000f;
                offset.z %= 10000f;
                mat.SetVector(ShaderParams.OrganicLightOffset, offset);

                if (radiant.organicLightDistanceScaling.value) {
                    mat.EnableKeyword(ShaderParams.SKW_DISTANCE_BLENDING);
                } else {
                    mat.DisableKeyword(ShaderParams.SKW_DISTANCE_BLENDING);
                }
                return true;
            }



#if UNITY_2022_1_OR_NEWER
            RTHandle GetAlbedoFromGbuffer() {
                return m_DeferredLights.GbufferAttachments[m_DeferredLights.GBufferAlbedoIndex];
            }
#else
            RenderTargetIdentifier GetAlbedoFromGbuffer() {
                return m_DeferredLights.GbufferAttachmentIdentifiers[m_DeferredLights.GBufferAlbedoIndex];
            }
#endif

#if UNITY_2023_3_OR_NEWER
            [Obsolete]
#endif
            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {
#if UNITY_2022_1_OR_NEWER
                RTHandle m_GbufferAttachmentsHandle = GetAlbedoFromGbuffer();
                ConfigureTarget(m_GbufferAttachmentsHandle, m_DeferredLights.DepthAttachmentHandle);
#else
                RenderTargetIdentifier m_GbufferAttachmentsIdentifier = GetAlbedoFromGbuffer();
                m_GbufferAttachmentsIdentifier = new RenderTargetIdentifier(m_GbufferAttachmentsIdentifier, 0, CubemapFace.Unknown, -1);
                ConfigureTarget(m_GbufferAttachmentsIdentifier, m_DeferredLights.DepthAttachmentIdentifier);
#endif
            }


#if UNITY_2023_3_OR_NEWER
            [Obsolete]
#endif
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {

                CommandBuffer cmd = CommandBufferPool.Get(m_strProfilerTag);
                cmd.DrawMesh(fullscreenMesh, Matrix4x4.identity, mat, 0, (int)Pass.OrganicLight);
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

#if UNITY_2023_3_OR_NEWER

            class PassData {
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData) {
                using (var builder = renderGraph.AddRasterRenderPass<PassData>(m_strProfilerTag, out var passData)) {

                    UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

                    builder.SetRenderAttachment(resourceData.gBuffer[0], 1, AccessFlags.Write);

                    builder.SetRenderFunc((PassData passData, RasterGraphContext context) => {
                        context.cmd.DrawMesh(fullscreenMesh, Matrix4x4.identity, mat, 0, (int)Pass.OrganicLight);
                    });
                }
            }

#endif

            public void Cleanup() {
                CoreUtils.Destroy(mat);
            }
        }


        [Tooltip("This is the render pass event that Radiant will use to execute its render passes. If temporal reprojection is used, Radiant will always execute before post processing to ensure the motion vectors texture has been generated.")]
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;

        [Tooltip("Select the rendering mode according to the URP asset.")]
        public RenderingPath renderingPath = RenderingPath.Deferred;

        [Tooltip("Allows Radiant to be executed even if camera has Post Processing option disabled.")]
        public bool ignorePostProcessingOption = true;

        RadiantPass radiantPass;
        RadiantComparePass comparePass;
        RadiantOrganicLightPass organicLightPass;

        void OnDisable() {
            if (radiantPass != null) {
                radiantPass.Cleanup();
            }
            if (comparePass != null) {
                comparePass.Cleanup();
            }
            if (organicLightPass != null) {
                organicLightPass.Cleanup();
            }
        }

        public override void Create() {
            radiantPass = new RadiantPass();
            comparePass = new RadiantComparePass();
            organicLightPass = new RadiantOrganicLightPass();
            emittersForceRefresh = true;
        }

        public static bool needRTRefresh;
        public static bool isRenderingInDeferred;

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
#if UNITY_EDITOR
            isRenderingInDeferred = renderingPath != RenderingPath.Forward; 
#endif
            if (!renderingData.cameraData.postProcessEnabled && !ignorePostProcessingOption) return;

            Camera cam = renderingData.cameraData.camera;
            bool isSceneView = cam.cameraType == CameraType.SceneView;
            if (cam.cameraType != CameraType.Game && !isSceneView) return;
            if (renderingData.cameraData.renderType != CameraRenderType.Base) return;

#if UNITY_EDITOR
            if (UnityEditor.ShaderUtil.anythingCompiling) {
                needRTRefresh = true;
            }
            if (needRTRefresh) {
                needRTRefresh = false;
                radiantPass.Cleanup();
            }

#endif
            RadiantGlobalIllumination radiant = VolumeManager.instance.stack.GetComponent<RadiantGlobalIllumination>();

            if (organicLightPass.Setup(radiant, renderer, isSceneView)) {
                renderer.EnqueuePass(organicLightPass);
            }

            if (radiantPass.Setup(radiant, renderer, this, isSceneView)) {
                renderer.EnqueuePass(radiantPass);
                if (!isSceneView) {
                    if (comparePass.Setup(renderer, radiantPass.renderPassEvent, this)) {
                        renderer.EnqueuePass(comparePass);
                    }
                }
            }
        }

        public static void RegisterReflectionProbe(ReflectionProbe probe) {
            if (probe == null) return;
            if (!probes.Contains(probe)) {
                probes.Add(probe);
            }
        }

        public static void UnregisterReflectionProbe(ReflectionProbe probe) {
            if (probe == null) return;
            if (probes.Contains(probe)) {
                probes.Remove(probe);
            }
        }

        public static void RegisterVirtualEmitter(RadiantVirtualEmitter emitter) {
            if (emitter == null) return;
            if (!emitters.Contains(emitter)) {
                emitters.Add(emitter);
                emittersForceRefresh = true;
            }
        }

        public static void UnregisterVirtualEmitter(RadiantVirtualEmitter emitter) {
            if (emitter == null) return;
            if (emitters.Contains(emitter)) {
                emitters.Remove(emitter);
                emittersForceRefresh = true;
            }
        }

    }

}

