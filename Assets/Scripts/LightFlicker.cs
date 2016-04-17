using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float timeOff = 0.5f;

    private float changeTime;
    private float timeOn;
    private Light lamp;


    void Start()
    {
        lamp = GetComponent<Light>();
        timeOn = Random.Range(0.5f, 5);
    }

    void Update()
    {
        /*if (Time.time > changeTime)
        {
            lamp.enabled = !lamp.enabled;
            if (lamp.enabled)
            {
                changeTime = Time.time + timeOn;
                timeOn = Random.Range(0, 5);
            }
            else
            {
                changeTime = Time.time + timeOff;
            }
        }*/

        if (Random.value > 0.95) //a random chance
        {
            if (lamp.enabled == true) //if the light is on...
            {
                lamp.enabled = false; //turn it off
            }
            else
            {
                lamp.enabled = true; //turn it on
            }
        }
    }
}
