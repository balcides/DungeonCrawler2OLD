using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

    bool isIndirectMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        //TODO: Allows Player to remap later (keybind)
        if(Input.GetKeyDown(KeyCode.G)){
            isIndirectMode = !isIndirectMode; //toggle mode
            currentClickTarget = transform.position; //clear the click target
        }

        if(isIndirectMode){
            ProcessDirectMovement();

        }else{
            ProcessMouseMovement();
        }
    }


    // makes it so you can control with keyboard
    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");   
        float v = Input.GetAxis("Vertical");   

        //calcualte carema relative direction to move:
        Transform moveCamera = Camera.main.transform;
        Vector3 cameraForward = Vector3.Scale(moveCamera.forward, new Vector3(1,0,1)).normalized;
        Vector3 movement = v * cameraForward + h * moveCamera.right;

        thirdPersonCharacter.Move(movement, false, false);
    }


    //control with mouse
    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;
                    thirdPersonCharacter.Move(currentClickTarget - transform.position, false, false);
                    break;
                case Layer.Enemy:
                    print("Not moving to Enemy");
                    break;
                default:
                    print("Unexpected Layer found");
                    return;
            }
        }
        var playerToClickPoint = currentClickTarget - transform.position;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius)
        {
            thirdPersonCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }
}