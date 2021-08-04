using UnityEngine;
using U_RPG.Movement;
using U_RPG.Core;

namespace U_RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Health Target;
        [SerializeField] float WeaponRange=2f;
        [SerializeField] float TimeBetweenAttacks=2f;
        float TimeSinceLastAttack=Mathf.Infinity;
        private void Update()
        {
            TimeSinceLastAttack+=Time.deltaTime; // Provides a delay attack between attack.
            
            // If there is no target to attack or target is dead, dont attack.
            if(Target==null) return; 
            if(Target.IsDead()) return;

            if (!GetIsInRange()) // if there is a target, and destination is big enough, move to target.
            {
                GetComponent<Mover>().MoveTo(Target.transform.position,1f);
            }
            else
            {
                GetComponent<Mover>().Cancel(); // When close enough, stop.
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(Target.transform); // Turn face to target.
            if(TimeSinceLastAttack > TimeBetweenAttacks) // There must be a delay attack between attack.
            {
                GetComponent<Animator>().ResetTrigger("StopAttack"); // StopAttack animation triger is now false.
                GetComponent<Animator>().SetTrigger("attack"); // Because attacking started.
                TimeSinceLastAttack=0f;
            // Now Triggering the Hit() event.
            }        
        }
        void Hit()
        //Animation Event
        {
            if (Target == null) return;
            Target.TakeDamage(5);
        }

        private bool GetIsInRange()
        {
            //If target is range (both for player and enemy) return true.
            return Vector3.Distance(transform.position, Target.transform.position) < WeaponRange;
        }

        public void Attack(GameObject CombatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this); //Update the action log. Player is not moving now, it is attacking.
            //print("Attack Func Called");
            Target=CombatTarget.GetComponent<Health>(); // Every time we attack, we have to know where enemy is.
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
            GetComponent<Animator>().SetTrigger("StopAttack"); // Animation trigger StopAttack is true now, player is not attacking.
        }

        public bool CanAttack(GameObject CombatTarget)
        {

            // Returns true if player or enemy can attack.
            if(CombatTarget == null) return false;

            Health TargetToTest=GetComponent<Health>();
            return TargetToTest!=null && !TargetToTest.IsDead();
        }













    }
}
