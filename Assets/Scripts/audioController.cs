using UnityEngine;
using System.Collections;

public class audioController: MonoBehaviour
{
    public AudioSource uff;
    public AudioSource breath;
    public AudioSource punch;
    public AudioSource punch2;

	public void PlayUffTreadmill()
    {        
        breath.Stop();
        uff.Play();
        punch.Play();       
    }

    public void PlayUffBoxing()
    {
        breath.Stop();       
        uff.Play();
        punch2.Play();
    }

    public void PlayPunch()
    {
        punch.Play();
    }

    public void PlayBreath()
    {
        if(!breath.isPlaying)
            breath.Play();
    }
	
	
}
