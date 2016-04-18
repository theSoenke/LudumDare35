using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyGroup : MonoBehaviour
{
    public float keyNum = 5;
    public int maxKeyErrors = 2;
    public GameObject keyPrefab;
    public Sprite[] keyTextures;
    public Transform pos;

    [HideInInspector]
    public bool locked;

    private List<Key> keyGroup;
    private int keyErrors;

    void Awake()
    {
        InitKeys();
        pos = keyGroup[0].gameObject.transform;
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
            // nextKey.gameObject.GetComponentInChildren<Image>().color = Color.green;
            Destroy(nextKey.gameObject);
            keyGroup.RemoveAt(0);
            GameManager.Instance.klick.Play();               
        }
        else
        {
            GameManager.Instance.error.Play();
            keyErrors++;
        }
        if(keyGroup.Count > 0)
            pos = keyGroup[0].gameObject.transform;
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
        pos.x = pos.x + 75 * keyNum;

        GameObject buttonGo = Instantiate(keyPrefab, pos, Quaternion.identity) as GameObject;
        buttonGo.transform.SetParent(transform);

        KeyCode keyCode = ButtonSmash.Instance.RandomKey();
        Key key = new Key(keyCode, buttonGo);

        //Text buttonContent = buttonGo.GetComponentInChildren<Text>();
        //buttonContent.text = key.ToString();
        Image keyImg = buttonGo.GetComponentInChildren<Image>();
        keyImg.sprite = GetKeyTexture(keyCode);

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

    private Sprite GetKeyTexture(KeyCode key)
    {
        foreach (var tex in keyTextures)
        {
            if (key.ToString() == tex.name)
            {
                return tex;
            }
        }

        return null;
    }
}
