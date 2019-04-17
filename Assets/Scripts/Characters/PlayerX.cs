using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{
    public class PlayerX : MonoBehaviour, IDamagable
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float damagerPerHit = 10f;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] int enemyLayer = 10;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        [SerializeField] Weapon weaponInUse;

        GameObject currentTarget;
        float currentHealthPoints = 100f;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;

        //Note: not sure why a get is used here, I'm not well versed but later want to look that up
        //  answer, helps protect the var and make it read only so it cant be assigned from anywhere
        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }


        private void Start()
        {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            OverrideAnimatorController();
        }


        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }


        private void OverrideAnimatorController()
        {
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT_ATTACK"] = weaponInUse.GetAttackAnimClip();
        }


        private void PutWeaponInHand()
        {
            var weaponsPrefab = weaponInUse.GetWeaponPrefab();
            GameObject weaponSocket = DominantHand();
            var weapon = Instantiate(weaponsPrefab, weaponSocket.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
        }

        private GameObject DominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            //handle 0
            //handle hand
            //handle greater
            Assert.AreNotEqual(numberOfDominantHands, 0, "No DominantHand found on player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "More than one DominantHand scripts on Player. Please remove one.");
            return dominantHands[0].gameObject;
        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        }

        //TODO: refector to reduce number of lines
        void OnMouseClick(RaycastHit raycastHit, int layerHit)
        {
            //TODO Double condition enemy layer and distance in one line
            if (layerHit == enemyLayer)
            {
                var enemy = raycastHit.collider.gameObject;

                //Check enemy is in range
                if ((enemy.transform.position - transform.position).magnitude > maxAttackRange)
                {

                    return;
                }

                currentTarget = enemy;

                var enemyComponent = enemy.GetComponent<EnemyX>();
                if (Time.time - lastHitTime > minTimeBetweenHits)
                {
                    enemyComponent.TakeDamage(damagerPerHit);
                    lastHitTime = Time.time;
                }

            }
        }

        public void TakeDamage(float damage)
        {

            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0)
            {
                //print("Player is dead.");
                //Destroy(gameObject);    
            }
        }
    }

}
