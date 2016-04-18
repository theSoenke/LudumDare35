using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float timeOff = 0.5f;
    public Vector2 randomTimeRange;
    public Color offColor;
    public Color onColor;
    public AudioSource sound;

    private float changeTime;
    private float timeOn;
    private Light lamp;
    private Renderer render;


    void Start()
    {
        sound.Pause();
        lamp = GetComponent<Light>();
        render = GetComponentInParent<Renderer>();
        timeOn = Random.Range(randomTimeRange.x, randomTimeRange.y);
    }

    void Update()
    {
        if (Time.time > changeTime)
        {
            lamp.enabled = !lamp.enabled;
            if (lamp.enabled)
            {
                sound.Pause();
                changeTime = Time.time + timeOn;
                timeOn = Random.Range(randomTimeRange.x, randomTimeRange.y);
                render.sharedMaterial.SetColor("_EmissionColor", onColor);
            }
            else
            {
                sound.UnPause();
                changeTime = Time.time + timeOff;
                render.sharedMaterial.SetColor("_EmissionColor", offColor);
            }
        }
    }
}
