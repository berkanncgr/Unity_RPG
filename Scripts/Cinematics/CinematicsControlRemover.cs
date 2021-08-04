using UnityEngine;
using UnityEngine.Playables;
using U_RPG.Core;
using U_RPG.Control;

namespace U_RPG.Cinematics
{
  public class CinematicsControlRemover : MonoBehaviour
    {
        GameObject Player;
        private void Start()
        {
            Player = GameObject.FindWithTag("Player");
            
            // Disables controllin the player while first timeline animation is playing.
            GetComponent<PlayableDirector>().played+=DisableControl; 
            // Enables controlling the player when first timeline animation ends.
            GetComponent<PlayableDirector>().stopped+=EnableControl;
        }
        void DisableControl(PlayableDirector pd)
        {
            // Disables controllin the player while first timeline animation is playing.
            
            Player.GetComponent<ActionScheduler>().CancelCurrentAction();
            Player.GetComponent<PlayerController>().enabled=false;
        }

        void EnableControl(PlayableDirector pd)
        {
            // Enables controlling the player when first timeline animation ends.
            Player.GetComponent<PlayerController>().enabled = true;
        }
    }  
}
