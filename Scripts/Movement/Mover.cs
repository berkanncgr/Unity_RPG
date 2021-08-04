using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using U_RPG.Core;


namespace U_RPG.Movement
{
    //This class is about enemy and player. Enemy and player both have this class.IF 
    public class Mover : MonoBehaviour, IAction
    {
        NavMeshAgent navMeshAgent;
        Health Health;
        [SerializeField] float MaxSpeed=6f;

        private void Start()
        {
             navMeshAgent=GetComponent<NavMeshAgent>();
             Health=GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled=!Health.IsDead(); // If not dead, enable nav mesh.
            UpdateAnimator(); //Update the animator.
        }

        private void MoveToCursor()
        {
            // Move to left mouse hit point. This function is called for the player only.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // bool HasHit = Physics.Raycast(ray,hit);
            if (Physics.Raycast(ray, out hit)) // if mouse hit is moveable location, move to there.
            {
                MoveTo(hit.point,MaxSpeed);
            }
        }

        public void MoveTo(Vector3 destination, float SpeedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed=MaxSpeed*Mathf.Clamp01(SpeedFraction);
            navMeshAgent.isStopped = false;
        }

        private void UpdateAnimator()
        {
            //Vector3 Velocity = NavMeshAgnet.velocity;
            Vector3 LocalVelocity = transform.InverseTransformDirection(GetComponent<NavMeshAgent>().velocity);
            //float Speed=LocalVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", LocalVelocity.z);
        }
    
        public void Cancel()
        {
            //Provides player stops moving when enough distance to enemy.
            navMeshAgent.isStopped=true; 
        }
        public void StartMoveAction(Vector3 Destination, float SpeedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this); // Update action log. Player is moving.
            //GetComponent<Fighter>().Cancel();
            MoveTo(Destination,SpeedFraction);
        }    
    
       
    
    
    
    
    
    
    



    }
}
