using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{
    public class EnemyX : MonoBehaviour, IDamagable
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float attackRadius = 4f;
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float secondsBetweenShots = 0.5f;
        [SerializeField] Vector3 aimOffset = Vector3.up;
        [SerializeField] GameObject projectileToUse = null;
        [SerializeField] GameObject projectileSocket = null;
        GameObject player = null;

        bool isAttacking = false;
        float currentHealthPoints = 100f;
        AICharacterControl aiCharacterControl = null;

        //Note: not sure why a get is used here, I'm not well versed but later want to look that up
        //  answer, helps protect the var and make it read only so it cant be assigned from anywhere
        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0) { Destroy(gameObject); }
        }


        private void Start()
        {

            player = GameObject.FindGameObjectWithTag("Player");
            aiCharacterControl = GetComponent<AICharacterControl>();
            currentHealthPoints = maxHealthPoints;
        }


        private void Update()
        {

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {

                isAttacking = true;
                InvokeRepeating("FireProjectile", 0f, secondsBetweenShots); //TODO switch to coroutines

            }

            if (distanceToPlayer > attackRadius)
            {

                isAttacking = false;
                CancelInvoke();

            }

            if (distanceToPlayer <= chaseRadius)
            {
                aiCharacterControl.SetTarget(player.transform);

            }
            else
            {
                aiCharacterControl.SetTarget(transform);
            }
        }

        void FireProjectile()
        {
            //Note: we don't worry about performance until it's time to look at the profiler, even though we are calling this on Update()
            // but later when refactoring it's not a bad idea to still give it our best. 
            //TODO : Set refectoring and group this shooting later
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
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

}
