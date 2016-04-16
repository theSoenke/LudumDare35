using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyGroup : MonoBehaviour
{
    public float keyNum = 5;
    public int maxKeyErrors = 2;
    public GameObject keyPrefab;

    private float keyPressTime;
    private List<Key> keyGroup;
    private int keyErrors;
    private Animator animator;
    private bool locked;

    void Awake()
    {
        animator = GetComponent<Animator>();
        float spawnDelay = ButtonSmash.Instance.spawnDelay;
        animator.speed = 1 / spawnDelay;
        keyPressTime = spawnDelay;

        InitKeys();
    }

    public void Input(KeyCode keyCode)
    {
        if (keyGroup.Count <= 0 || locked)
        {
            return;
        }
        Key nextKey = keyGroup[0];

        if (keyCode == nextKey.keyCode)
        {
            Destroy(nextKey.gameObject);
            keyGroup.RemoveAt(0);
        }
        else
        {
            keyErrors++;
        }
    }

    public void SetColor(Color color)
    {
        foreach(Key key in keyGroup)
        {
            Image image = key.gameObject.GetComponentInChildren<Image>();
            image.color = color;
        }
    }

    private void InitKeys()
    {
        keyGroup = new List<Key>();

        for (int i = 0; i < keyNum; i++)
        {
            SpawnKey(i);
        }
    }

    private void SpawnKey(int keyNum)
    {
        Vector3 pos = transform.position;
        pos.x = pos.x + 40 * keyNum;

        GameObject buttonGo = Instantiate(keyPrefab, pos, Quaternion.identity) as GameObject;
        buttonGo.transform.SetParent(transform);
        Text buttonContent = buttonGo.GetComponentInChildren<Text>();

        KeyCode keyCode = RandomKey();
        Key key = new Key(keyCode, buttonGo, keyPressTime);
        buttonContent.text = key.ToString();

        keyGroup.Add(key);
    }

    public bool IsFinished()
    {
        if(keyGroup.Count == 0)
        {
            return true;
        }
        return false;
    }

    public bool IsFailed()
    {
        bool failed = false;

        for (int i = 0; i < keyGroup.Count; i++)
        {
            Key key = keyGroup[i];

            if (key.TimeOver())
            {
                failed = true;
            }
        }

        if (keyErrors >= maxKeyErrors)
        {
            failed = true;
        }

        locked = failed;

        return failed;
    }

    private KeyCode RandomKey()
    {
        KeyCode[] values = { KeyCode.LeftArrow, KeyCode.RightArrow };
        int random = Random.Range(0, values.Length);

        return values[random];
    }

    private class Key
    {
        public Key(KeyCode keyCode, GameObject gameObject, float timer)
        {
            this.keyCode = keyCode;
            this.timer = timer;
            this.gameObject = gameObject;
            spawnTime = Time.realtimeSinceStartup;
        }

        public KeyCode keyCode;
        public GameObject gameObject;

        private float spawnTime;
        private float timer;

        public override string ToString()
        {
            return KeyCodeToSign(keyCode);
        }

        public bool TimeOver()
        {
            if (Time.realtimeSinceStartup - spawnTime > timer)
            {
                return true;
            }
            return false;
        }

        private string KeyCodeToSign(KeyCode key)
        {
            switch (key)
            {
                case KeyCode.LeftArrow:
                    return "<=";
                case KeyCode.RightArrow:
                    return "=>";
                default:
                    return "Undefined";
            }
        }
    }
}
