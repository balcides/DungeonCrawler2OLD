using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyX : MonoBehaviour, IDamagable
{
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attackRadius = 4f;
    [SerializeField] float chaseRadius = 6f;
    GameObject player = null;

    float currentHealthPoints = 100f;
    AICharacterControl aiCharacterControl = null;

    //Note: not sure why a get is used here, I'm not well versed but later want to look that up
    //  answer, helps protect the var and make it read only so it cant be assigned from anywhere
    public float healthAsPercentage{ get { return currentHealthPoints / maxHealthPoints; } }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }


    private void Start() {

        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
    }


    private void Update() {
        
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if(distanceToPlayer <= attackRadius){

            print(gameObject.name + " attacking player");
            //TODO: spawning projectile
        }

        if (distanceToPlayer <= chaseRadius)
        {
            aiCharacterControl.SetTarget(player.transform);

        }else{
 
            aiCharacterControl.SetTarget(transform);
        }
    }


    void OnDrawGizmos()
    {
        //Draw Movement Gizmos
        Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        //Draw Chase Gizmos
        Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }

}
