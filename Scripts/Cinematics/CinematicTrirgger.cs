using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


namespace U_RPG.Cinematics
{
    public class CinematicTrirgger : MonoBehaviour
    {
        bool control=false;
        private void OnTriggerEnter(Collider other)
        {
            // Make sure box collider triggers only the player and make sure anim only play once.
            if(!control && other.gameObject.tag=="Player") 
           {
                GetComponent<PlayableDirector>().Play();
                control=true;
           }
        }
    }
}

