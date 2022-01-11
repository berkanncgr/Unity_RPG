using UnityEngine;
using UnityEngine.Playables;
using U_RPG.Core;
using U_RPG.Control;

namespace U_RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject Player;

        private void OnEnable()
        {
            // Disables controllin the player while first timeline animation is playing.
            GetComponent<PlayableDirector>().played += DisableControl;

            // Enables controlling the player when first timeline animation ends.
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable()
        {
          
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        private void Awake()
        {
            Player = GameObject.FindWithTag("Player");
        }

        void DisableControl(PlayableDirector pd)
        {
            // Disables controllin the player while first timeline animation is playing.
            Player.GetComponent<ActionScheduler>().CancelCurrentAction();
            Player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector pd)
        {
            // Enables controlling the player when first timeline animation ends.
            Player.GetComponent<PlayerController>().enabled = true;
        }
    }
}