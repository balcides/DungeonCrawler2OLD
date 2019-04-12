using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 5f;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;

    bool isIndirectMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
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


    // //control with mouse
    // private void ProcessMouseMovement()
    // {
    //     if (Input.GetMouseButton(0))
    //     {
    //         clickPoint = cameraRaycaster.hit.point;
    //         switch (cameraRaycaster.currentLayerHit)
    //         {
    //             case Layer.Walkable:
    //                 currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
    //                 break;
    //             case Layer.Enemy:
    //                 currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
    //                 print("Not moving to Enemy");
    //                 break;
    //             default:
    //                 print("Unexpected Layer found");
    //                 return;
    //         }
    //     }
    //     WalkToDestination();
    // }

    private void WalkToDestination()
    {
        var playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= 0)
        {
            thirdPersonCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    private Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.15f);

        //(Optional) Draw Attack Sphere
        Gizmos.color = new Color(255f, 0f, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    }
}