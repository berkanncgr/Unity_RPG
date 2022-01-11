using UnityEngine;
using U_RPG.Resources;

namespace U_RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float Speed = 1;
        [SerializeField] bool bIsHoming = true;
        [SerializeField] GameObject HitEffect = null;
        [SerializeField] float MaxLifeTime = 10;
        [SerializeField] GameObject[] DestroyOnHit = null;
        [SerializeField] float LifeAfterImpact = 2;

        Health Target = null;
        GameObject Instigator = null;
        float Damage = 0;

        private void Start()
        {   
            // Projectile targeting himself to target
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (Target == null) return;

            // projectile fallows target.
            if (bIsHoming && !Target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            // Projectile not follows the target
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }

        public void SetTarget(Health Target, GameObject Instigator, float Damage)
        {
            this.Target = Target;
            this.Damage = Damage;
            this.Instigator = Instigator;

            // Destroy afer few seconds.
            Destroy(gameObject, MaxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = Target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return Target.transform.position;
            }
            return Target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != Target) return;
            if (Target.IsDead()) return;

            // If the arrow collided with actor that has a Health component, give damage to that actor.
            Target.TakeDamage(Instigator, Damage);

            Speed = 0;

            if (HitEffect != null)
            {
                Instantiate(HitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in DestroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, LifeAfterImpact);

        }

    }

}