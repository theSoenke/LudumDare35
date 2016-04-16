using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyGroup : MonoBehaviour
{
    public float keyNum = 5;
    public int maxKeyErrors = 2;
    public GameObject keyPrefab;
    public float keyTime = 3;

    private List<Key> keyGroup;
    private int keyErrors;


    void Start()
    {
        InitKeys();
    }

    void Update()
    {
        IsFailed();
    }

    public void Input(KeyCode keyCode)
    {
        if (keyGroup.Count <= 0)
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
        Key key = new Key(keyCode, buttonGo, keyTime);
        buttonContent.text = key.ToString();

        keyGroup.Add(key);
    }

    public bool IsFailed()
    {
        for (int i = 0; i < keyGroup.Count; i++)
        {
            Key key = keyGroup[i];

            if (key.TimeOver())
            {
                return true;
            }
        }

        if (keyErrors > maxKeyErrors)
        {
            return true;
        }

        return false;
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
