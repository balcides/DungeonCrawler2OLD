using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;   //note other classes can set
    float damageCaused = 10f;

    //Note; this is written to keep the property public(accessible) without being available in the inspector
    public void SetDamage(float damage){

        damageCaused = damage;
    }


    void OnCollisionEnter(Collision collisionInfo)
    {
        
        Component damageableComponent = collisionInfo.gameObject.GetComponent(typeof(IDamagable));

        if(damageableComponent){
            (damageableComponent as IDamagable).TakeDamage(damageCaused);
        
        }
        Destroy(gameObject, 0.01f);
    }
}
