using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerX : MonoBehaviour, IDamagable
{
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float damagerPerHit = 10f;
    [SerializeField] float minTimeBetweenHits = 0.5f;
    [SerializeField] float maxAttackRange = 2f;
    [SerializeField] int enemyLayer = 10;
    [SerializeField] Weapon weaponInUse;

    GameObject currentTarget;
    float currentHealthPoints = 100f;
    CameraRaycaster cameraRaycaster;
    float lastHitTime = 0f;

    //Note: not sure why a get is used here, I'm not well versed but later want to look that up
    //  answer, helps protect the var and make it read only so it cant be assigned from anywhere
    public float healthAsPercentage{ get{ return currentHealthPoints / maxHealthPoints; } }


    private void Start()
    {
        RegisterForMouseClick();
        currentHealthPoints = maxHealthPoints;
        PutWeaponInHand();
    }

    private void PutWeaponInHand()
    {
        var weaponsPrefab = weaponInUse.GetWeaponPrefab();
        var weapon = Instantiate(weaponsPrefab);
        //TODO: move to correct place and child to hand
    }

    private void RegisterForMouseClick()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
    }


    void OnMouseClick(RaycastHit raycastHit, int layerHit){
        //TODO Double condition enemy layer and distance in one line
        if(layerHit == enemyLayer){
            var enemy = raycastHit.collider.gameObject;

            //Check enemy is in range
            if((enemy.transform.position - transform.position).magnitude > maxAttackRange){

                return;
            }

            currentTarget = enemy;

            var enemyComponent = enemy.GetComponent<EnemyX>();
            if(Time.time - lastHitTime > minTimeBetweenHits){
                enemyComponent.TakeDamage(damagerPerHit);
                lastHitTime = Time.time;
            }

        }
    }

    public void TakeDamage(float damage){

        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if(currentHealthPoints <= 0){  
            //print("Player is dead.");
            //Destroy(gameObject);    
        }
    }
}
