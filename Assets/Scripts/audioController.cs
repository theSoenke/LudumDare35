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
        breath.PlayDelayed(uff.clip.length + 2);
    }

    public void PlayUffBoxing()
    {
        uff.Play();
        punch2.Play();
    }

    public void PlayPunch()
    {
        punch.Play();
    }
	
	
}
