using UnityEngine;

public class ButtonSmash : MonoBehaviour
{
    public static ButtonSmash Instance;

    public float spawnDelay = 3;
    public GameObject groupPrefab;
    public int maxFails = 3;
    public Color failedColor;

    [Range(0, 1)]
    public int level;

    private KeyCode[] level1Keys = { KeyCode.LeftArrow, KeyCode.RightArrow };
    private KeyCode[] level2Keys = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };

    private KeyGroup currentGroup;
    private float timer;
    private int failedGroups;
    private bool running = true;


    void OnEnable()
    {
        Instance = this;
    }

    public KeyCode RandomKey()
    {
        KeyCode[] values;

        switch (level)
        {
            case 0:
                values = level1Keys;
                break;
            case 1:
                values = level2Keys;
                break;
            default:
                values = level1Keys;
                break;
        }

        int random = Random.Range(0, values.Length);

        return values[random];
    }

    void Update()
    {
        if (timer <= 0)
        {
            if (currentGroup != null)
            {
                if (!currentGroup.locked && !currentGroup.IsFinished())
                {
                    Failure();
                }
            }

            SpawnKeyGroup();
            timer = spawnDelay;
        }

        timer -= Time.deltaTime;

        if (currentGroup != null)
        {
            if (!currentGroup.locked && currentGroup.IsFailed())
            {
                Failure();
                currentGroup.SetColor(failedColor);
            }
            else if (currentGroup.IsFinished())
            {
                SpawnKeyGroup();
                timer = spawnDelay;
            }
        }

        InputUpdate();
    }

    private void SpawnKeyGroup()
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

    private void Failure()
    {
        failedGroups++;

        if (failedGroups >= maxFails)
        {
            Debug.Log("You lost!");
            running = false;
        }
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
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentGroup.Input(KeyCode.UpArrow);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentGroup.Input(KeyCode.DownArrow);
        }
    }
}
