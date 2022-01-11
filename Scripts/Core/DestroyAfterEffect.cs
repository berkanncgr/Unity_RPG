using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U_RPG.Core
{   

    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject TargetToDestroy = null;

        // Destory the projectile effect when projectile hits something.
        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {   
                if(TargetToDestroy !=null)
                {
                    Destroy(TargetToDestroy);
                }

                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}