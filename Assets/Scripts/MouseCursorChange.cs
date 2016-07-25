using UnityEngine;
using System.Collections;

public class MouseCursorChange : MonoBehaviour {

    public Texture2D cursorTex;

    void Awake()
    {
        Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.Auto);
    }
}
