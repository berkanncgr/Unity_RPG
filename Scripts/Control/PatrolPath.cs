using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U_RPG.Control
{
public class PatrolPath : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }



        private void OnDrawGizmos()
        {
            int i; float WaypointGizmoRadious;

            for (i=0; i<transform.childCount; i++)
            {
                int j=GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i),0.3f);
                Gizmos.DrawLine(GetWaypoint(i),GetWaypoint(j));
            }
        }

        //Call from AIController.
        public int GetNextIndex(int i)
        {
            if(i+1==transform.childCount) return 0;
            return i+1;
        }

        //Call from AIController.
        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }














    }

}

