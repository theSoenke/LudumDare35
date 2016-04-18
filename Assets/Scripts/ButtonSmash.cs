using UnityEngine;

public class ButtonSmash : MonoBehaviour
{
    public static ButtonSmash Instance;

    public float spawnDelay = 3;
    public GameObject groupPrefab;
    public Color failedColor;

    public AnimationCurve buttonSpeed;
    public int groupNr;

    private KeyCode[] level1Keys = { KeyCode.LeftArrow, KeyCode.RightArrow };
    private KeyCode[] level2Keys = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };

    private KeyGroup currentGroup;
    private float timer;


    void OnEnable()
    {
        Instance = this;
    }

    void Start()
    {
        groupNr = 0;
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


    private bool PosInViewport(Transform pos)
    {
        if (pos != null && Camera.main.ScreenToViewportPoint(pos.position).x < 0)
            return false;

        return true;
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
            pos.x = Mathf.Lerp(xPos, -800, t) * buttonSpeed.Evaluate(groupNr);
            currentGroup.transform.position = pos;
        }

        if (currentGroup.IsFinished())
        {
            timer = 0;
            GameManager.Instance.GroupFinished();
            if (currentGroup != null)
            {
                Destroy(currentGroup.gameObject);
            }
            SpawnKeyGroup();
        }

        if (!PosInViewport(currentGroup.pos))        //t >= 1)
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
