using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] float projectileSpeed; 
    [SerializeField] GameObject shooter; //so it can be inspected when paused

    const float DESTROY_DELAY = 0.01f;
    float damageCaused = 10f;

    //Note; this is written to keep the property public(accessible) without being available in the inspector
    public void SetDamage(float damage){

        damageCaused = damage;
    }

    public void SetShooter(GameObject shooter){
        this.shooter = shooter;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        var layerCollidedWith = collisionInfo.gameObject.layer;
        if (layerCollidedWith != shooter.layer)
        {
            DamageIfDamageable(collisionInfo);
        }
    }

    private void DamageIfDamageable(Collision collisionInfo)
    {
        Component damageableComponent = collisionInfo.gameObject.GetComponent(typeof(IDamagable));

        if (damageableComponent)
        {
            (damageableComponent as IDamagable).TakeDamage(damageCaused);
        }
        Destroy(gameObject, DESTROY_DELAY);
    }

    public float GetDefaultLaunchSpeed(){
        return projectileSpeed;
    }
}
