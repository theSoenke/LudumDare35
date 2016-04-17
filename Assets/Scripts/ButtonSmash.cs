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

        switch (GameManager.Instance.GetLevel())
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

    bool locked = false;
    float xPos = 0;
    void Update()
    {
        float t = timer / spawnDelay;
        if (currentGroup == null)
        {
            SpawnKeyGroup();
        }
        else
        {
            Vector3 pos = currentGroup.transform.position;
            timer += Time.deltaTime;
            pos.x = Mathf.Lerp(xPos + 70, -800, t);
            currentGroup.transform.position = pos;
        }

        if (currentGroup.IsFinished())
        {
            timer = 0;
            GameManager.Instance.GroupFinished();
            Destroy(currentGroup.gameObject);
            SpawnKeyGroup();
        }

        if (t >= 1)
        {
            timer = 0;

            if (!locked)
            {
                Destroy(currentGroup.gameObject);
                GameManager.Instance.GroupFailed();
                locked = true;
            }
            else
            {
                if (currentGroup != null)
                {
                    Destroy(currentGroup.gameObject);
                }
            }
        }

        if (!locked && currentGroup.IsFailed())
        {
            locked = true;
            currentGroup.SetColor(failedColor);
            GameManager.Instance.GroupFailed();
        }

        InputUpdate();
    }

    private void SpawnKeyGroup()
    {
        if (!GameManager.Instance.IsRunning())
        {
            return;
        }

        if (currentGroup != null)
        {
            Destroy(currentGroup.gameObject);
        }

        Vector3 pos = transform.position;
        pos.x += 70;
        GameObject groupGo = (GameObject)Instantiate(groupPrefab, pos, Quaternion.identity);
        currentGroup = groupGo.GetComponent<KeyGroup>();
        currentGroup.transform.SetParent(transform);

        xPos = currentGroup.transform.position.x;

        locked = false;
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
