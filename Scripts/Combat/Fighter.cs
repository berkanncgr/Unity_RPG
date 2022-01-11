using UnityEngine;
using U_RPG.Movement;
using U_RPG.Core;
using U_RPG.Saving;
using U_RPG.Resources;
using U_RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using System;

namespace U_RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable , IModifierProvider
    {
        [SerializeField] float TimeBetweenAttacks = 1f;
        [SerializeField] Transform RightHandTransform = null;
        [SerializeField] Transform LeftHandTransform = null;
        [SerializeField] Weapon DefaultWeapon = null;

        Health Target;
        float TimeSinceLastAttack = Mathf.Infinity;
        LazyValue<Weapon> CurrentWeapon;

        // Begining of the game, give default weapon to characters and enemies.
        private void Awake() {
            CurrentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            AttachWeapon(DefaultWeapon);
            return DefaultWeapon;
        }

        private void Start() 
        {
           CurrentWeapon.ForceInit();
        }

        private void Update()
        {
            TimeSinceLastAttack += Time.deltaTime; // Provides a delay attack between attack.
            
            // If there is no target to attack or target is dead, dont attack.
            if (Target == null) return;
            if (Target.IsDead()) return;

            if (!GetIsInRange()) // if there is a target, and destination is big enough, move to target.
            {
                GetComponent<Mover>().MoveTo(Target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel(); // When close enough, stop.
                AttackBehaviour(); // And then attack.
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            CurrentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();

            // Spawn weapon when pick up weapon from ground and override animation.
            weapon.Spawn(RightHandTransform, LeftHandTransform, animator);
        }


        public Health GetTarget()
        {   
            // Combat target
            return Target;
        } 

        private void AttackBehaviour()
        {
            transform.LookAt(Target.transform); // Turn face to target.
            if (TimeSinceLastAttack > TimeBetweenAttacks) // There must be a delay attack between attack.
            {
                // This will trigger the Hit() event.
                TriggerAttack();
                TimeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack"); // StopAttack animation triger is now false.
            GetComponent<Animator>().SetTrigger("attack"); // Because attacking started.
        }

        // Animation Event
        void Hit()
        {
            if(Target == null) { return; }
            

            float Damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (CurrentWeapon.value.HasProjectile())
            {
                CurrentWeapon.value.LaunchProjectile(RightHandTransform, LeftHandTransform, Target, gameObject, Damage);
            }
            else
            {
                Target.TakeDamage(gameObject, Damage);
            }
        }
        // Animation Event
        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            //If target is range (both for player and enemy) return true.
            return Vector3.Distance(transform.position, Target.transform.position) < CurrentWeapon.value.GetRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            // Returns true if player or enemy can attack.
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            //Update the action log. Player is not moving now, it is attacking.
            GetComponent<ActionScheduler>().StartAction(this);

            // Every time we attack, we have to know where enemy is.
            Target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            Target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack"); // Animation trigger attack is false now, because player is not attackin.
            GetComponent<Animator>().SetTrigger("stopAttack"); // Animation trigger StopAttack is true now, player is not attacking.
        }

        public object CaptureState()
        {
            return CurrentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

        // calculate additive damage to weapon
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return CurrentWeapon.value.GetDamage();
            }
        }

        // calculate percentage damage to weapon
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return CurrentWeapon.value.GetPercentageBonus();
            }
        }
    }
}