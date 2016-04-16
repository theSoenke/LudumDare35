using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyGroup : MonoBehaviour
{
    public float keyNum = 5;
    public int maxKeyErrors = 2;
    public GameObject keyPrefab;

    [HideInInspector]
    public bool locked;

    private float keyPressTime;
    private List<Key> keyGroup;
    private int keyErrors;
    private Animator animator;

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
        foreach (Key key in keyGroup)
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
        pos.x = pos.x + 45 * keyNum;

        GameObject buttonGo = Instantiate(keyPrefab, pos, Quaternion.identity) as GameObject;
        buttonGo.transform.SetParent(transform);
        Text buttonContent = buttonGo.GetComponentInChildren<Text>();

        KeyCode keyCode = ButtonSmash.Instance.RandomKey();
        Key key = new Key(keyCode, buttonGo, keyPressTime);
        buttonContent.text = key.ToString();

        keyGroup.Add(key);
    }

    public bool IsFinished()
    {
        if (keyGroup.Count == 0)
        {
            return true;
        }
        return false;
    }

    public bool IsFailed()
    {
        if (keyErrors >= maxKeyErrors)
        {
            locked = true;
        }

        return locked;
    }

    private class Key
    {
        public Key(KeyCode keyCode, GameObject gameObject, float timer)
        {
            this.keyCode = keyCode;
            this.gameObject = gameObject;
        }

        public KeyCode keyCode;
        public GameObject gameObject;

        public override string ToString()
        {
            switch (keyCode)
            {
                case KeyCode.LeftArrow:
                    return "<=";
                case KeyCode.RightArrow:
                    return "=>";
                case KeyCode.UpArrow:
                    return "^";
                case KeyCode.DownArrow:
                    return "_";
                default:
                    return "Undefined";
            }
        }
    }
}
