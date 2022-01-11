using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace U_RPG.Cinematics

{
    public class CinematicTrigger : MonoBehaviour
    {
        bool bAlreadyTriggered = false;
        
        private void OnTriggerEnter(Collider other) 
        {
            // Make sure box collider triggers only the player and make sure anim only play once.
            if(!bAlreadyTriggered && other.gameObject.tag == "Player")
            {
                bAlreadyTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}