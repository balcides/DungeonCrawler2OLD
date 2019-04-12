using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof (ThirdPersonCharacter))]
[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (AICharacterControl))]
public class PlayerMovement : MonoBehaviour
{



    ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster = null;
    Vector3 currentDestination, clickPoint;
    AICharacterControl aiCharacterControl = null;
    GameObject walkTarget = null;

    //TODO: move this to one script and consolidate from cursor affordance (low priority)
    [SerializeField] const int walkableLayerNumber = 9;
    [SerializeField] const int enemyLayerNumber = 10;


    bool isIndirectMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
        aiCharacterControl = GetComponent<AICharacterControl>();
        walkTarget = new GameObject("walkTarget");

        cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
    }

    void ProcessMouseClick(RaycastHit raycastHit, int layerHit){
        
        switch(layerHit){
            case enemyLayerNumber:
                //navigate to the enemy
                GameObject enemy = raycastHit.collider.gameObject;
                aiCharacterControl.SetTarget(enemy.transform);
                break;

            case walkableLayerNumber:
                //navigate to point on the ground
                walkTarget.transform.position = raycastHit.point;
                aiCharacterControl.SetTarget(walkTarget.transform);
                break;

            default:
                Debug.Log("Dont know how to handle mouse clikc or player momevement");
                break;
        }

    }

    // makes it so you can control with keyboard
    // TODO: Make this get called again (But not for the coarse)
    void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");   
        float v = Input.GetAxis("Vertical");   

        //calcualte carema relative direction to move:
        Transform moveCamera = Camera.main.transform;
        Vector3 cameraForward = Vector3.Scale(moveCamera.forward, new Vector3(1,0,1)).normalized;
        Vector3 movement = v * cameraForward + h * moveCamera.right;

        thirdPersonCharacter.Move(movement, false, false);
    }


    private void OnDrawGizmos() {
    /*
    
        Kept as a good example for GUI Gizmo debugging (and I like how it looks and works)

     */
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawLine(transform.position, currentDestination);
        // Gizmos.DrawSphere(currentDestination, 0.1f);
        // Gizmos.DrawSphere(clickPoint, 0.15f);

        // //(Optional) Draw Attack Sphere
        // Gizmos.color = new Color(255f, 0f, 0, 0.5f);
        // Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    }
}