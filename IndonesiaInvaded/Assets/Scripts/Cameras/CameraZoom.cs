using System.Collections;
using Cinemachine;
using UnityEngine;

namespace IndonesiaInvaded
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] [Range(0f, 12f)] public float defaultDistance = 6f;
        [SerializeField] [Range(0f, 12f)] private float minimumDistance = 1f;
        [SerializeField] [Range(0f, 12f)] private float maximumDistance = 6f;

        [SerializeField] [Range(0f, 20f)] private float smoothing = 4f;
        [SerializeField] [Range(0f, 20f)] private float zoomSensitivity = 1f;

        private CinemachineFramingTransposer framingTransposer;
        private CinemachineInputProvider inputProvider;

        private float currentTargetDistance;
        private bool isZooming = false; // Menandakan apakah sedang dalam proses zoom atau tidak

        private void Awake()
        {
            framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            inputProvider = GetComponent<CinemachineInputProvider>();

            currentTargetDistance = defaultDistance;
        }

        private void Update()
        {
            if (!isZooming) // Hanya izinkan zoom jika tidak sedang dalam proses zoom
            {
                Zoom();
            }
        }

        private void Zoom()
        {
            float zoomValue = inputProvider.GetAxisValue(2) * zoomSensitivity;

            currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue, minimumDistance, maximumDistance);

            float currentDistance = framingTransposer.m_CameraDistance;

            if (currentDistance == currentTargetDistance)
            {
                return;
            }

            float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);

            framingTransposer.m_CameraDistance = lerpedZoomValue;
        }

        public void ZoomIn(float duration)
        {
            StartCoroutine(ZoomCoroutine(duration, true));
        }

        public void ZoomOut(float duration)
        {
            StartCoroutine(ZoomCoroutine(duration, false));
        }

        private IEnumerator ZoomCoroutine(float duration, bool zoomIn)
        {
            isZooming = true;

            float originalDistance = framingTransposer.m_CameraDistance;
            float targetDistance = zoomIn ? minimumDistance : defaultDistance; // Tentukan target distance sesuai dengan zoomIn

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                framingTransposer.m_CameraDistance = Mathf.Lerp(originalDistance, targetDistance, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            framingTransposer.m_CameraDistance = targetDistance;
            isZooming = false;
        }
    }
}
