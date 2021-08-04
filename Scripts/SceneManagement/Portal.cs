using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace U_RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier { A, B, C, D, E } // Paryying the portals. Portal A's exit's is other Portal A.
        [SerializeField] int SceneToLoad=-1;    [SerializeField] float FadeOutTime = 1f;
        [SerializeField] float FadeInTime = 2f;     [SerializeField] float FadeWaitTime = 0.5f; 
        [SerializeField] Transform SpawnPoint; [SerializeField] DestinationIdentifier Destination;
        
        private void OnTriggerEnter(Collider other) 
        { // When player in portal, exit the other portal.
           if(other.gameObject.tag == "Player")
           {
               StartCoroutine(Transition());
           }        
        }
        private IEnumerator Transition()
        {
            //White Transition effect when warping.
            DontDestroyOnLoad(gameObject);
            Fader Fader=FindObjectOfType<Fader>();
            yield return Fader.FadeOut(FadeOutTime);
            yield return SceneManager.LoadSceneAsync(SceneToLoad); // Wapring here.
            Portal OtherPortal=GetOtherPoral();
            UpdatePlayer(OtherPortal);
            yield return new WaitForSeconds(FadeWaitTime);
            yield return Fader.FadeIn(FadeInTime);
            Destroy(gameObject);
        }
        private Portal GetOtherPoral()
        {
            // Each portal has a spessific exit portal. Find the true exit portal.
            foreach(Portal Portal in FindObjectsOfType<Portal>())
            {
                if(Portal==this) { continue; }
                if(Portal.Destination!= Destination) { continue; }

                return Portal;
            }
            return null;
        }
        
        private void UpdatePlayer (Portal OtherPortal)
        {

            GameObject Player=GameObject.FindWithTag("Player");
            Player.GetComponent<NavMeshAgent>().Warp(OtherPortal.SpawnPoint.position);
            Player.transform.rotation = OtherPortal.SpawnPoint.rotation;
            // Player.transform.position=OtherPortal.SpawnPoint.position;
            //Player.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}

