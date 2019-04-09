using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursorx : MonoBehaviour
{
    CameraRaycaster cameraRayCaster;

    // Start is called before the first frame update
    void Start()
    {
        cameraRayCaster = GetComponent<CameraRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(cameraRayCaster.layerHit);
    }
}
