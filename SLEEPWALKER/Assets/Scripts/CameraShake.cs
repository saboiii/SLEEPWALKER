using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform cameraTransform;
    public float shakeDuration = 0.2f;
    public float shakeIntensity = 0.05f;

    private Vector3 initialPosition;

    private void Awake()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    public void ShakeOnHit()
    {
        StartCoroutine(ShakeCamera(shakeDuration, shakeIntensity));
    }

    public void ShakeOnPlayerDeath()
    {
        StartCoroutine(ShakeCamera(shakeDuration * 2f, shakeIntensity * 10f));
    }

    private IEnumerator ShakeCamera(float duration, float intensity)
    {
        initialPosition = cameraTransform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            cameraTransform.localPosition = new Vector3(x, y, initialPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = initialPosition;
    }
}
