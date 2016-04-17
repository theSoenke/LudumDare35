using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public float switchTime = 5;

    private List<Transform> cameras;
    private int selection;
    private float timer;

    void Start()
    {
        cameras = new List<Transform>();

        foreach (Transform child in transform)
        {
            cameras.Add(child);
            child.gameObject.SetActive(false);
        }

        cameras[selection].gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCameraPerspective();
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SwitchCameraPerspective();
            timer = switchTime;
        }
    }

    private void SwitchCameraPerspective()
    {
        cameras[selection].gameObject.SetActive(false);
        selection++;
        selection %= cameras.Count;
        cameras[selection].gameObject.SetActive(true);
    }
}
