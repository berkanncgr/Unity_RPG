using System;
using U_RPG.Core;
using U_RPG.Saving;
using U_RPG.Stats;
using UnityEngine;
using GameDevTV.Utils;

namespace U_RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {   
        
        [SerializeField] float RegenerationPersentage = 70;

        // Healthpoints is a LazyValue because avoiding race condition.
        LazyValue<float> HealthPoints;


        bool bIsDead = false;

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;    
        }

        private void Awake()
        {   
            // Set the health based on level in prgression
            HealthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {   
            // Get health value based on level.
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void Start()
        {   
           HealthPoints.ForceInit();
        }
        
        // Regen health when level up.
        private void RegenerateHealth()
        {
            float RegenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * RegenerationPersentage / 100;
            HealthPoints.value = Mathf.Max(HealthPoints.value,RegenHealthPoints);
        }

        public bool IsDead()
        {
            return bIsDead;
        }

        public void TakeDamage(GameObject Instigator, float Damage)
        {   
            print(gameObject.name + "took damage: " + Damage);
            HealthPoints.value = Mathf.Max(HealthPoints.value - Damage, 0);

            // Gain experinece when kill enemy.
            if(HealthPoints.value == 0)
            {
                Die();
                AwardExperience(Instigator);
            }
        }   

        public float GetHealthPoints()
        {
            return HealthPoints.value;
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (HealthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void Die()
        {
            if (bIsDead) return;

            bIsDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            // Gain experience based on enemy level and enemy class.
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public object CaptureState()
        {
            return HealthPoints;
        }

        public void RestoreState(object state)
        {
            HealthPoints.value = (float) state;
            
            if (HealthPoints.value <= 0)
            {
                Die();
            }
        }
    }
}