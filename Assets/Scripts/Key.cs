using UnityEngine;

public class Key
{
    public Key(KeyCode keyCode, GameObject gameObject)
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