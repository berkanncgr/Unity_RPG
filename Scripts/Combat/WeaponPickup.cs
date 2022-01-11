using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U_RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float RespawnTime = 5;
    
        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);

                // When pickin up a weapon, respawn it again after few secons at same place.
                StartCoroutine(HideForSeconds(RespawnTime));
            }
        }

        private IEnumerator HideForSeconds(float Seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(Seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool ShouldShow)
        {
            GetComponent<Collider>().enabled = ShouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(ShouldShow);
            }
        }
    }
}