using UnityEngine;

namespace U_RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {

        [SerializeField] GameObject PersistentObjectPreFab;
        static bool bHasSpawned=false;
        private void Awake()
        {
            if(bHasSpawned) return;

            SpawnPersistentObjects(); 
            bHasSpawned=true;       
        }


        private void SpawnPersistentObjects()
        {
            GameObject PersistentOject=Instantiate(PersistentObjectPreFab);
            DontDestroyOnLoad(PersistentOject);
        }


























    } 
}
