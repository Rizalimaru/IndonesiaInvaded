using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;
using DebugView = RadiantGI.Universal.RadiantGlobalIllumination.DebugView;
using System.Reflection;
using UnityEngine.Rendering;

namespace RadiantGI.Universal {
#if UNITY_2022_2_OR_NEWER
    [CustomEditor(typeof(RadiantGlobalIllumination))]
#else
    [VolumeComponentEditor(typeof(RadiantGlobalIllumination))]
#endif
    public class RadiantGlobalIlluminationEditor : VolumeComponentEditor {

        SerializedDataParameter indirectIntensity, maxIndirectSourceBrightness, indirectDistanceAttenuation, normalMapInfluence, lumaInfluence;
        SerializedDataParameter nearFieldObscurance, nearFieldObscuranceSpread, nearFieldObscuranceOccluderDistance, nearFieldObscuranceMaxCameraDistance, nearFieldObscuranceTintColor;
        SerializedDataParameter virtualEmitters;
        SerializedDataParameter organicLight, organicLightThreshold, organicLightNormalsInfluence, organicLightSpread, organicLightTintColor, organicLightAnimationSpeed, organicLightDistanceScaling;
        SerializedDataParameter brightnessThreshold, brightnessMax, specularContribution, sourceBrightness, giWeight, nearCameraAttenuation, saturation, limitToVolumeBounds, aoInfluence;
        SerializedDataParameter stencilCheck, stencilValue, stencilCompareFunction;
        SerializedDataParameter rayCount, rayMaxLength, rayMaxSamples, rayJitter, thickness, rayBinarySearch, rayReuse, rayBounce;
        SerializedDataParameter fallbackReuseRays, fallbackReflectionProbes, probesIntensity, fallbackReflectiveShadowMap, reflectiveShadowMapIntensity;
        SerializedDataParameter downsampling, raytracerAccuracy, smoothing;
        SerializedDataParameter temporalReprojection, temporalResponseSpeed, temporalCameraTranslationResponse, temporalChromaThreshold, temporalDepthRejection;
        SerializedDataParameter showInEditMode, showInSceneView, debugView, debugDepthMultiplier, compareMode, compareSameSide, comparePanning, compareLineAngle, compareLineWidth;

#if !UNITY_2021_2_OR_NEWER
        public override bool hasAdvancedMode => false;
#endif

        public override void OnEnable() {
            base.OnEnable();

            var o = new PropertyFetcher<RadiantGlobalIllumination>(serializedObject);
            indirectIntensity = Unpack(o.Find(x => x.indirectIntensity));
            maxIndirectSourceBrightness = Unpack(o.Find(x => x.indirectMaxSourceBrightness));
            indirectDistanceAttenuation = Unpack(o.Find(x => x.indirectDistanceAttenuation));
            normalMapInfluence = Unpack(o.Find(x => x.normalMapInfluence));
            lumaInfluence = Unpack(o.Find(x => x.lumaInfluence));
            nearFieldObscurance = Unpack(o.Find(x => x.nearFieldObscurance));
            nearFieldObscuranceSpread = Unpack(o.Find(x => x.nearFieldObscuranceSpread));
            nearFieldObscuranceOccluderDistance = Unpack(o.Find(x => x.nearFieldObscuranceOccluderDistance));
            nearFieldObscuranceMaxCameraDistance = Unpack(o.Find(x => x.nearFieldObscuranceMaxCameraDistance));
            nearFieldObscuranceTintColor = Unpack(o.Find(x => x.nearFieldObscuranceTintColor));
            virtualEmitters = Unpack(o.Find(x => x.virtualEmitters));
            organicLight = Unpack(o.Find(x => x.organicLight));
            organicLightThreshold = Unpack(o.Find(x => x.organicLightThreshold));
            organicLightSpread = Unpack(o.Find(x => x.organicLightSpread));
            organicLightNormalsInfluence = Unpack(o.Find(x => x.organicLightNormalsInfluence));
            organicLightTintColor = Unpack(o.Find(x => x.organicLightTintColor));
            organicLightAnimationSpeed = Unpack(o.Find(x => x.organicLightAnimationSpeed));
            organicLightDistanceScaling = Unpack(o.Find(x => x.organicLightDistanceScaling));
            brightnessThreshold = Unpack(o.Find(x => x.brightnessThreshold));
            brightnessMax = Unpack(o.Find(x => x.brightnessMax));
            specularContribution = Unpack(o.Find(x => x.specularContribution));
            sourceBrightness = Unpack(o.Find(x => x.sourceBrightness));
            giWeight = Unpack(o.Find(x => x.giWeight));
            nearCameraAttenuation = Unpack(o.Find(x => x.nearCameraAttenuation));
            saturation = Unpack(o.Find(x => x.saturation));
            limitToVolumeBounds = Unpack(o.Find(x => x.limitToVolumeBounds));
            stencilCheck = Unpack(o.Find(x => x.stencilCheck));
            stencilValue = Unpack(o.Find(x => x.stencilValue));
            stencilCompareFunction = Unpack(o.Find(x => x.stencilCompareFunction));
            aoInfluence = Unpack(o.Find(x => x.aoInfluence));
            rayCount = Unpack(o.Find(x => x.rayCount));
            rayMaxLength = Unpack(o.Find(x => x.rayMaxLength));
            rayMaxSamples = Unpack(o.Find(x => x.rayMaxSamples));
            rayJitter = Unpack(o.Find(x => x.rayJitter));
            thickness = Unpack(o.Find(x => x.thickness));
            rayBinarySearch = Unpack(o.Find(x => x.rayBinarySearch));
            rayReuse = Unpack(o.Find(x => x.rayReuse));
            rayBounce = Unpack(o.Find(x => x.rayBounce));
            fallbackReuseRays = Unpack(o.Find(x => x.fallbackReuseRays));
            fallbackReflectionProbes = Unpack(o.Find(x => x.fallbackReflectionProbes));
            probesIntensity = Unpack(o.Find(x => x.probesIntensity));
            fallbackReflectiveShadowMap = Unpack(o.Find(x => x.fallbackReflectiveShadowMap));
            reflectiveShadowMapIntensity = Unpack(o.Find(x => x.reflectiveShadowMapIntensity));
            downsampling = Unpack(o.Find(x => x.downsampling));
            raytracerAccuracy = Unpack(o.Find(x => x.raytracerAccuracy));
            smoothing = Unpack(o.Find(x => x.smoothing));
            temporalReprojection = Unpack(o.Find(x => x.temporalReprojection));
            temporalResponseSpeed = Unpack(o.Find(x => x.temporalResponseSpeed));
            temporalCameraTranslationResponse = Unpack(o.Find(x => x.temporalCameraTranslationResponse));
            temporalDepthRejection = Unpack(o.Find(x => x.temporalDepthRejection));
            temporalChromaThreshold = Unpack(o.Find(x => x.temporalChromaThreshold));
            showInEditMode = Unpack(o.Find(x => x.showInEditMode));
            showInSceneView = Unpack(o.Find(x => x.showInSceneView));
            debugView = Unpack(o.Find(x => x.debugView));
            debugDepthMultiplier =  Unpack(o.Find(x => x.debugDepthMultiplier));
            compareMode = Unpack(o.Find(x => x.compareMode));
            compareSameSide = Unpack(o.Find(x => x.compareSameSide));
            comparePanning = Unpack(o.Find(x => x.comparePanning));
            compareLineAngle = Unpack(o.Find(x => x.compareLineAngle));
            compareLineWidth = Unpack(o.Find(x => x.compareLineWidth));
        }

        public override void OnInspectorGUI() {

            var pipe = GraphicsSettings.currentRenderPipeline as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
            if (pipe == null) {
                EditorGUILayout.HelpBox("Universal Rendering Pipeline asset is not set in Project Settings / Graphics !", MessageType.Error);
                return;
            }

            // Check depth texture mode
            FieldInfo renderers = pipe.GetType().GetField("m_RendererDataList", BindingFlags.NonPublic | BindingFlags.Instance);
            if (renderers == null) return;
            foreach (var renderer in (object[])renderers.GetValue(pipe)) {
                if (renderer == null) continue;
                FieldInfo depthTextureModeField = renderer.GetType().GetField("m_CopyDepthMode", BindingFlags.NonPublic | BindingFlags.Instance);
                if (depthTextureModeField != null) {
                    int depthTextureMode = (int)depthTextureModeField.GetValue(renderer);
                    if (depthTextureMode == 1) { // transparent copy depth mode
                        EditorGUILayout.HelpBox("Depth Texture Mode in URP asset must be set to 'After Opaques' or 'Force Prepass'.", MessageType.Warning);
                        if (GUILayout.Button("Show Pipeline Asset")) {
                            Selection.activeObject = (Object)renderer;
                            GUIUtility.ExitGUI();
                        }
                        EditorGUILayout.Separator();
                    }
                }
            }

            serializedObject.Update();

            EditorGUILayout.LabelField("General", EditorStyles.miniLabel);
            PropertyField(indirectIntensity, new GUIContent("Indirect Light Intensity"));
            EditorGUI.indentLevel++;
            PropertyField(indirectDistanceAttenuation, new GUIContent("Distance Attenuation"));
            PropertyField(rayBounce, new GUIContent("One Extra Bounce"));
            PropertyField(maxIndirectSourceBrightness, new GUIContent("Max Source Brightness"));
            PropertyField(normalMapInfluence);
            PropertyField(lumaInfluence);
            EditorGUI.indentLevel--;
            PropertyField(nearFieldObscurance);
            if (nearFieldObscurance.value.floatValue > 0f) {
                EditorGUI.indentLevel++;
                PropertyField(nearFieldObscuranceSpread, new GUIContent("Spread"));
                PropertyField(nearFieldObscuranceOccluderDistance, new GUIContent("Occluder Distance"));
                PropertyField(nearFieldObscuranceMaxCameraDistance, new GUIContent("Max Camera Distance"));
                PropertyField(nearFieldObscuranceTintColor, new GUIContent("Tint color"));
                EditorGUI.indentLevel--;
            }
            PropertyField(virtualEmitters);
            PropertyField(organicLight);
            if (organicLight.value.floatValue > 0f) {
                EditorGUI.indentLevel++;
                if (!RadiantRenderFeature.isRenderingInDeferred) {
                    EditorGUILayout.HelpBox("Organic Light requires deferred rendering path.", MessageType.Warning);
                }
                PropertyField(organicLightSpread, new GUIContent("Spread"));
                PropertyField(organicLightThreshold, new GUIContent("Threshold"));
                PropertyField(organicLightNormalsInfluence, new GUIContent("Normals Influence"));
                PropertyField(organicLightTintColor, new GUIContent("Tint Color"));
                PropertyField(organicLightAnimationSpeed, new GUIContent("Animation Speed"));
                PropertyField(organicLightDistanceScaling, new GUIContent("Distance Scaling"));
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.LabelField("Quality", EditorStyles.miniLabel);
            PropertyField(rayCount);
            PropertyField(rayMaxLength, new GUIContent("Max Distance"));
            PropertyField(rayMaxSamples, new GUIContent("Max Samples"));
            PropertyField(rayJitter, new GUIContent("Jittering"));
            PropertyField(thickness);
            PropertyField(rayBinarySearch, new GUIContent("Binary Search"));
            PropertyField(smoothing);
            PropertyField(temporalReprojection, new GUIContent("Temporal Filter"));
            if (temporalReprojection.value.boolValue) {
                EditorGUI.indentLevel++;
                if (temporalReprojection.value.boolValue && !Application.isPlaying && showInEditMode.value.boolValue) {
                    EditorGUILayout.HelpBox("Temporal filter does not work in Scene View if not in play mode.", MessageType.Info);
                }
                PropertyField(temporalResponseSpeed, new GUIContent("Response Speed"));
                PropertyField(temporalChromaThreshold, new GUIContent("Chroma Threshold"));
                PropertyField(temporalCameraTranslationResponse, new GUIContent("Camera Translation Response"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.LabelField("Fallbacks", EditorStyles.miniLabel);
            PropertyField(fallbackReuseRays, new GUIContent("Reuse Rays"));
            if (fallbackReuseRays.value.boolValue) {
                EditorGUI.indentLevel++;
                PropertyField(rayReuse, new GUIContent("Intensity"));
                if (rayReuse.value.floatValue > 0) {
                    if (!temporalReprojection.value.boolValue) {
                        EditorGUILayout.HelpBox("Reuse Rays works with Temporal Filter enabled.", MessageType.Info);
                    }
                    PropertyField(temporalDepthRejection, new GUIContent("Depth Rejection"));
                }
                EditorGUI.indentLevel--;
            }
            PropertyField(fallbackReflectionProbes, new GUIContent("Use Reflection Probes"));
            if (fallbackReflectionProbes.value.boolValue) {
                EditorGUI.indentLevel++;
                PropertyField(probesIntensity);
                EditorGUI.indentLevel--;
            }
            PropertyField(fallbackReflectiveShadowMap, new GUIContent("Use Reflective Shadow Map"));
            if (fallbackReflectiveShadowMap.value.boolValue) {
                EditorGUI.indentLevel++;
                if (!RadiantShadowMap.installed) {
                    EditorGUILayout.HelpBox("Add Radiant Shadow Map script to the main directional light.", MessageType.Warning);
                }
                PropertyField(reflectiveShadowMapIntensity, new GUIContent("Intensity"));
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.LabelField("Performance", EditorStyles.miniLabel);
            PropertyField(raytracerAccuracy);
            PropertyField(downsampling);

            EditorGUILayout.LabelField("Artistic Controls", EditorStyles.miniLabel);
            PropertyField(brightnessThreshold);
            PropertyField(brightnessMax, new GUIContent("Maximum Brightness"));
            PropertyField(specularContribution);
            PropertyField(sourceBrightness);
            PropertyField(giWeight, new GUIContent("GI Weight"));
            PropertyField(saturation);
            PropertyField(nearCameraAttenuation);
            PropertyField(limitToVolumeBounds);
            PropertyField(stencilCheck);
            if (stencilCheck.value.boolValue) {
                PropertyField(stencilValue);
                PropertyField(stencilCompareFunction);
            }
            PropertyField(aoInfluence, new GUIContent("AO Influence"));

            EditorGUILayout.LabelField("Debug", EditorStyles.miniLabel);
            PropertyField(showInEditMode);
            PropertyField(showInSceneView);
            PropertyField(debugView);
            if (!temporalReprojection.value.boolValue && (debugView.value.intValue == (int)DebugView.TemporalAccumulationBuffer)) {
                EditorGUILayout.HelpBox("Temporal filter not in execution. No debug output available.", MessageType.Warning);
            } else if (debugView.value.intValue == (int)DebugView.ReflectiveShadowMap && !fallbackReflectiveShadowMap.value.boolValue) {
                EditorGUILayout.HelpBox("Reflective Shadow Map fallback option is not enabled. No debug output available.", MessageType.Warning);
            } else if (debugView.value.intValue == (int)DebugView.Depth)
            {
                PropertyField(debugDepthMultiplier);
            }
            PropertyField(compareMode);
            if (compareMode.value.boolValue) {
                EditorGUI.indentLevel++;
                PropertyField(compareSameSide, new GUIContent("Same Side"));
                if (compareSameSide.value.boolValue) {
                    PropertyField(comparePanning, new GUIContent("Panning"));
                } else {
                    PropertyField(compareLineAngle, new GUIContent("Line Angle"));
                    PropertyField(compareLineWidth, new GUIContent("Line Width"));
                }
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
