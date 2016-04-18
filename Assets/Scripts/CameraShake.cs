using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float power = 0.2f;
    public float duration = 1;
    public float maxDist;

    private Vector3 originalCameraPosition;
    private Camera mainCamera;

    public void Shake(float delay)
    {
        mainCamera = Camera.main;
        originalCameraPosition = mainCamera.transform.position;

        InvokeRepeating("StartShake", delay, .01f);
        Invoke("StopShake", duration + delay);
    }

    private void StartShake()
    {
        if (power > 0)
        {
            float shakeAmount = Mathf.Clamp(Random.value * power * 2 - power, -maxDist, maxDist);
            Vector3 pp = Camera.main.transform.position;
            pp.y += shakeAmount;
            Camera.main.transform.position = pp;
        }
    }

    private void StopShake()
    {
        CancelInvoke("StartShake");
        mainCamera.transform.position = originalCameraPosition;
    }
}
