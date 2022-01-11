using System;
using UnityEngine;

namespace U_RPG.Core
{
    public class PeristentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject PersistentObjectPrefab;

        static bool bHasSpawned = false;

        private void Awake() {
            if (bHasSpawned) return;

            SpawnPersistentObjects();

            bHasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject PersistentObject = Instantiate(PersistentObjectPrefab);
            DontDestroyOnLoad(PersistentObject);
        }
    }
}