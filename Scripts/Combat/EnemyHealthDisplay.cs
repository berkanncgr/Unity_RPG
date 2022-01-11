using System;
using U_RPG.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace U_RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        // Show enemy health.
        private void Update()
        {
            if (fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "N/A";
                return;
            }
            Health health = fighter.GetTarget();
            GetComponent<Text>().text = String.Format("{0:0}%", health.GetPercentage());
        }
    }
}