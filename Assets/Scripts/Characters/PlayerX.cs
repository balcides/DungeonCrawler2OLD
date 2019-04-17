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
        [SerializeField] int enemyLayer = 10;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        [SerializeField] Weapon weaponInUse;

        Animator animator;
        GameObject currentTarget;
        float currentHealthPoints = 100f;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;

        //Note: not sure why a get is used here, I'm not well versed but later want to look that up
        //  answer, helps protect the var and make it read only so it cant be assigned from anywhere
        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        //Note: Never thought about putting public scripts at the top
        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0)
            {
                //print("Player is dead.");
                //Destroy(gameObject);    
            }
        }

        private void Start()
        {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupRuntimeAnimator();
        }


        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }


        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
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


        void OnMouseClick(RaycastHit raycastHit, int layerHit)
        {
            if (layerHit == enemyLayer)
            {
                var enemy = raycastHit.collider.gameObject;
                if(IsTargetInRange(enemy)){
                    AttackTarget(enemy);
                }
            }
        }


        private void AttackTarget(GameObject target)
        {
            var enemyComponent = target.GetComponent<EnemyX>();
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                animator.SetTrigger("Attack"); //TODO: set const
                enemyComponent.TakeDamage(damagerPerHit);
                lastHitTime = Time.time;
            }
        }


        private bool IsTargetInRange(GameObject target)
        {
            //Check enemy is in range
            float distanceToTaget = (target.transform.position - transform.position).magnitude;
            return distanceToTaget <= weaponInUse.GetMaxAttackRange();
        }


    }

}
