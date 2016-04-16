using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ButtonSmash : MonoBehaviour
{
    public float speed;
    public GameObject buttonPrefab;

    public int score;

    private KeyCode lastPress;
    private List<Key> nextKeys;
    private float timer;

    void Start()
    {
        InvokeRepeating("ButtonSpawner", 0, speed);
        nextKeys = new List<Key>();
    }

    void Update()
    {
        InputUpdate();
    }

    private void ButtonSpawner()
    {
        KeyCode nextKey = RandomKey();

        GameObject buttonGo = Instantiate(buttonPrefab, transform.position, Quaternion.identity) as GameObject;
        buttonGo.transform.SetParent(transform);
        Text buttonContent = buttonGo.GetComponentInChildren<Text>();
        buttonContent.text = KeyCodeToSign(nextKey);

        Key key = new Key(nextKey, buttonGo);
        nextKeys.Add(key);

        float animTime = 4f;
        StartCoroutine(DestroyButton(buttonGo, animTime));
    }

    private KeyCode RandomKey()
    {
        KeyCode[] values = { KeyCode.LeftArrow, KeyCode.RightArrow };
        int random = Random.Range(0, values.Length);

        return values[random];
    }

    private String KeyCodeToSign(KeyCode key)
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

    private IEnumerator DestroyButton(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
    }

    private void InputUpdate()
    {
        KeyCode pressedKey = KeyCode.Escape;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            pressedKey = KeyCode.LeftArrow;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            pressedKey = KeyCode.RightArrow;
        }

        if (nextKeys.Count <= 0)
        {
            return;
        }
        Key nextKey = nextKeys[0];

        if (nextKeys.Count > 0 && pressedKey == nextKey.keyCode)
        {
            score++;
            Destroy(nextKey.go);
            nextKeys.RemoveAt(0);
        }
        else if (pressedKey != KeyCode.Escape)
        {
            score--;
        }
    }


    private class Key
    {
        public Key(KeyCode keyCode, GameObject go)
        {
            this.keyCode = keyCode;
            this.go = go;
        }

        public KeyCode keyCode;
        public GameObject go;
    }
}
