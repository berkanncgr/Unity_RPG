using UnityEngine;
using U_RPG.Resources;

namespace U_RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController AnimatorOverride = null;
        [SerializeField] GameObject EquippedPrefab = null;
        [SerializeField] float WeaponDamage = 5f;
        [SerializeField] float PercentageBonus = 5f;
        [SerializeField] float WeaponRange = 2f;
        [SerializeField] bool bIsRightHanded = true;
        [SerializeField] Projectile Projectile = null;

        const string WeaponName = "Weapon";

        // When pick up a weapon from ground, destroy old one and equip weapon to correct hand.
        public void Spawn(Transform RightHand, Transform LeftHand, Animator animator)
        {
            DestroyOldWeapon(RightHand, LeftHand);

            if (EquippedPrefab != null)
            {
                // Handle where to spawn, right hand or left hand.
                Transform HandTransform = GetTransform(RightHand, LeftHand);
                GameObject weapon = Instantiate(EquippedPrefab, HandTransform);
                weapon.name = WeaponName;
            }

            // override animations based on a weapon class.
            var OverrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (AnimatorOverride != null)
            {
                animator.runtimeAnimatorController = AnimatorOverride; 
            }

            // Fix the bug. Change the animator override after weapon change.
            else if (OverrideController != null)
            {
                animator.runtimeAnimatorController = OverrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform RightHand, Transform LeftHand)
        {
            Transform OldWeapon = RightHand.Find(WeaponName);
            if (OldWeapon == null) // Means we dont have a weapon in right hand.
            {
                OldWeapon = LeftHand.Find(WeaponName);
            }
            if (OldWeapon == null) return; // Means we dopnt have a weapon in left hand either. There is npthing to destroy.

            OldWeapon.name = "DESTROYING"; // Prevent us bugs when we gonna equip a new weapon.
            Destroy(OldWeapon.gameObject);
        }

        // Is this wepon left hand or right hand?
        private Transform GetTransform(Transform RightHand, Transform LeftHand)
        {
            Transform HandTransform;
            if (bIsRightHanded) HandTransform = RightHand;
            else HandTransform = LeftHand;
            return HandTransform;
        }

        // Is weapon has a projectile? Bow maybe?
        public bool HasProjectile()
        {
            return Projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform LeftHand, Health Target, GameObject Instigator, float CalculatedDamage)
        {       
            Projectile projectileInstance = Instantiate(Projectile, GetTransform(rightHand, LeftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(Target, Instigator, CalculatedDamage);
        }

        public float GetDamage()
        {
            return WeaponDamage;
        }

        public float GetRange()
        {
            return WeaponRange;
        }

        public float GetPercentageBonus()
        {
            return PercentageBonus;
        }
    }
}
