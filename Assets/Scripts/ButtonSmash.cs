using UnityEngine;

public class ButtonSmash : MonoBehaviour
{
    public static ButtonSmash Instance;

    public float spawnDelay = 3;
    public GameObject groupPrefab;
    public Color failedColor;

    private KeyCode[] level1Keys = { KeyCode.LeftArrow, KeyCode.RightArrow };
    private KeyCode[] level2Keys = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };

    private KeyGroup currentGroup;
    private float timer;


    void OnEnable()
    {
        Instance = this;
    }

    public KeyCode RandomKey()
    {
        KeyCode[] values;

        switch (GameManager.Instance.level)
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
        if(!GameManager.Instance.IsRunning())
        {
            return;
        }

        if (timer <= 0)
        {
            if (currentGroup != null)
            {
                if (!currentGroup.locked && !currentGroup.IsFinished())
                {
                    GameManager.Instance.GroupFailed();
                }
            }

            if(GameManager.Instance.IsRunning())
            {
                Debug.Log("spawn");
                SpawnKeyGroup();
            }
            timer = spawnDelay;
        }

        timer -= Time.deltaTime;

        if (currentGroup != null)
        {
            if (!currentGroup.locked && currentGroup.IsFailed())
            {
                GameManager.Instance.GroupFailed();
                currentGroup.SetColor(failedColor);
            }
            else if (currentGroup.IsFinished())
            {
                GameManager.Instance.GroupFinished();
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
