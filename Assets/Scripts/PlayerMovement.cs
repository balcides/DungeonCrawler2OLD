using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

    bool isIndirectMode = false; //TODO: Consider making static later

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        //TODO: Allows Player to remap later (keybind)
        if(Input.GetKeyDown(KeyCode.G)){
            isIndirectMode = !isIndirectMode; //toggle mode
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
        Transform m_Cam = Camera.main.transform;
        Vector3 m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1,0,1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * m_Cam.right;

        m_Character.Move(m_Move, false, false);
    }


    //control with mouse
    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            switch (cameraRaycaster.layerHit)
            {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;
                    m_Character.Move(currentClickTarget - transform.position, false, false);
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
            m_Character.Move(playerToClickPoint, false, false);
        }
        else
        {
            m_Character.Move(Vector3.zero, false, false);
        }
    }
}