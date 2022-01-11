using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using UnityEngine;

namespace U_RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        [SerializeField] GameObject LevelUpParticleEffect = null;

        [SerializeField] bool bShouldUseModifiers = false;

        LazyValue<int> CurrentLevel;

        public event Action OnLevelUp;

        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();

            CurrentLevel = new LazyValue<int>(CalculateLevel);
        }


        private void OnEnable()
        {   
            // Bind method.
            if (experience != null)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.OnExperienceGained -= UpdateLevel;
            }
        }


        private void Start()
        {
            CurrentLevel.ForceInit();
        }

        // Get experience point and calculate level based on experince points.
        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float CurrentXP= experience.GetPoints();
            int PenultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp,characterClass);

            for (int level = 1; level <= PenultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > CurrentXP)
                {
                    return level;
                }
            }

            return PenultimateLevel + 1;
        }

        // When experience gained, update level if necessary.
        private void UpdateLevel() 
        {
            int newLevel = CalculateLevel();
            if(newLevel > CurrentLevel.value)
            {
                CurrentLevel.value = newLevel;
                LevelUpEffect();
                OnLevelUp();
            }
        }

        // Play particle effect on level up.
        private void LevelUpEffect()
        {
            Instantiate(LevelUpParticleEffect, transform);
        }

        // Return the stat of given character class.
        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveStat(stat)) * (1+GetPercentageStat(stat)/100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        // Add additiveStat to weapon damage
        private float GetAdditiveStat(Stat stat)
        {   
            float Total =0 ;
            foreach (IModifierProvider Provider in GetComponents<IModifierProvider>())
            {
                foreach (float Modifier in Provider.GetAdditiveModifiers(stat))
                {
                    Total += Modifier;
                }
            }
        
            return Total;
        }

        // Add percentage damage to weapon damage.
        private float GetPercentageStat(Stat stat)
        {
            if (bShouldUseModifiers == false) return 0;

            float Total = 0;
            foreach (IModifierProvider Provider in GetComponents<IModifierProvider>())
            {
                foreach (float Modifier in Provider.GetPercentageModifiers(stat))
                {
                    Total += Modifier;
                }
            }

            return Total;   
        }

        // Return level.
        public int GetLevel()
        {
            return CurrentLevel.value;
        }
    }
}