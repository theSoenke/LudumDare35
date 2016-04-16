using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSmash : MonoBehaviour
{
    public static ButtonSmash Instance;

    public float spawnDelay = 3;
    public GameObject groupPrefab;
    public int maxFails = 3;
    public Color failedColor;

    private KeyGroup currentGroup;
    private float timer;
    private int failedGroups;
    private bool running = true;


    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (timer <= 0)
        {
            SpawnGroup();
            timer = spawnDelay;
        }

        timer -= Time.deltaTime;

        if (currentGroup != null)
        {
            if (!currentGroup.locked && currentGroup.IsFailed())
            {
                failedGroups++;
                currentGroup.SetColor(failedColor);

                if (failedGroups >= maxFails)
                {
                    Debug.Log("You lost!");
                    running = false;
                }
            }
            else if (currentGroup.IsFinished())
            {
                SpawnGroup();
                timer = spawnDelay;
            }
        }

        InputUpdate();
    }

    private void SpawnGroup()
    {
        if (currentGroup != null)
        {
            Destroy(currentGroup.gameObject);
        }

        if (!running)
        {
            return;
        }

        GameObject groupGo = (GameObject)Instantiate(groupPrefab, transform.position, Quaternion.identity);
        currentGroup = groupGo.GetComponent<KeyGroup>();
        currentGroup.transform.SetParent(transform);
    }

    private void InputUpdate()
    {
        if (currentGroup == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentGroup.Input(KeyCode.LeftArrow);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentGroup.Input(KeyCode.RightArrow);
        }
    }
}
