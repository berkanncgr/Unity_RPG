using System;
using System.Collections;
using U_RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace U_RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int SceneToLoad = -1;
        [SerializeField] Transform SpawnPoint;
        [SerializeField] DestinationIdentifier Destination;
        [SerializeField] float FadeOutTime = 1f;
        [SerializeField] float FadeInTime = 2f;
        [SerializeField] float FadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        // Fadeout canvas effect, save the game and load the other map, spawn player at spawn point of other portal.
        private IEnumerator Transition()
        {
            if (SceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            
            yield return fader.FadeOut(FadeOutTime);

            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(SceneToLoad);

            savingWrapper.Load();
            
            Portal OtherPortal = GetOtherPortal();
            UpdatePlayer(OtherPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(FadeWaitTime);
            yield return fader.FadeIn(FadeInTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.SpawnPoint.position;
            player.transform.rotation = otherPortal.SpawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        // Every portal has a matched portal for warping. Find the match from enum.
        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {   
                // A portal cannot match with itself. The other portal must be diffrent from itself.
                if (portal == this) continue;
                if (portal.Destination != Destination) continue;

                return portal;
            }

            return null;
        }
    }
}