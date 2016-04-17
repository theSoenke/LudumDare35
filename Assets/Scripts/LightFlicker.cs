using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float timeOff = 0.5f;
    public Vector2 randomTimeRange;
    public Color offColor;
    public Color onColor;

    private float changeTime;
    private float timeOn;
    private Light lamp;
    private Renderer renderer;


    void Start()
    {
        lamp = GetComponent<Light>();
        renderer = GetComponentInParent<Renderer>();
        timeOn = Random.Range(randomTimeRange.x, randomTimeRange.y);
    }

    void Update()
    {
        if (Time.time > changeTime)
        {
            lamp.enabled = !lamp.enabled;
            if (lamp.enabled)
            {
                changeTime = Time.time + timeOn;
                timeOn = Random.Range(randomTimeRange.x, randomTimeRange.y);
                renderer.sharedMaterial.SetColor("_EmissionColor", onColor);
            }
            else
            {
                changeTime = Time.time + timeOff;
                renderer.sharedMaterial.SetColor("_EmissionColor", offColor);
            }
        }
    }
}
