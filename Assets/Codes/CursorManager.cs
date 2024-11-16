using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    public static CursorManager instance;

    public Texture2D defaultCursor;

    private void Awake() {
        if(instance != null) {
            Debug.LogError("More than one CursorMangager in scene!");
        }
        instance = this;
    }

    private void Start() {
        SetCursorToDefault();    
    }

    public void SetCursorToTurret(Texture2D cursorTexture){
        ChangeCursor(cursorTexture);
    }

    public void SetCursorToDefault(){
        ChangeCursor(defaultCursor);
    }

    private void ChangeCursor(Texture2D cursorTexture){
        Vector2 hotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture,hotspot,CursorMode.Auto);
    }
}
