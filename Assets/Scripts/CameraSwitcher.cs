using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    private List<Transform> cameras;
    private int selection;

    void Start()
    {
        cameras = new List<Transform>();

        foreach (Transform child in transform)
        {
            cameras.Add(child);
            child.gameObject.SetActive(false);
        }

        cameras[0].gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameras[selection].gameObject.SetActive(false);
            selection++;
            selection %= cameras.Count;
            cameras[selection].gameObject.SetActive(true);
        }
    }
}
