using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]

public class CursorAffordance : MonoBehaviour
{
    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D targetCursor = null;
    [SerializeField] Texture2D unknownCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0,0);
    [SerializeField] const int walkableLayerNumber =  9;
    [SerializeField] const int enemyLayerNumber =  10;

    CameraRaycaster cameraRayCaster;

    // Start is called before the first frame update
    void Start()
    {
        cameraRayCaster = GetComponent<CameraRaycaster>();
        cameraRayCaster.notifyLayerChangeObservers += OnLayerChanged; //registering
    }

    // Update is called once per frame
    void OnLayerChanged(int newLayer)
    {
        switch(newLayer){

            case walkableLayerNumber:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case enemyLayerNumber:
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                return;
        }
    }
}

//TODO: consider deregistering OnLayerChanged on leaving all game scenes