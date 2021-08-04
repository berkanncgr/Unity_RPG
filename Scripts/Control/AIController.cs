using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using U_RPG.Combat;
using U_RPG.Core;
using U_RPG.Movement;

namespace U_RPG.Control
{
    // This class is controlling enemy behavior. This class is about enemy.
    public class AIController : MonoBehaviour
    {
        
        [SerializeField] float ChaseDistance=4f;
        [SerializeField] float SuspicionTime = 3f; // Enemy become suspicious when it looses playerr for 3 seconds.

        [SerializeField] PatrolPath PatrolPath; // Eemy patrol Path
        [SerializeField] float WaypointTolarance=1f;
        [SerializeField] float WaypointDwellTime = 3f; // Wait for 3 seconds in patrol points

        [Range(0,1)][SerializeField] float PatrolSpeedFraction=0.2f; // During patrol, move slover.
        Fighter Fighter;
        GameObject Player;
        Health Health;
        Vector3 GuardPosition;
        int CurrentWaypointIndex=0; // Patrol path indexes.

        float TimeSinceLastSawPlayer=Mathf.Infinity; // Time since enemy saw player.
        float TimeSinceArrivedWaypoint = Mathf.Infinity; // Time since enemy waits on patrol waypoint.

        // Start is called before the first frame update
        void Start()
        {
            Fighter=GetComponent<Fighter>();
            Player = GameObject.FindWithTag("Player");
            Health=GetComponent<Health>();
            GuardPosition=transform.position; // Locate the enemy position.              
        }

        // Update is called once per frame
        void Update()
        {
            if (Health.IsDead()) return; // If enemy is dead, dont do rest of code.

            if (InAttackRangeOfPlayer() && Fighter.CanAttack(Player)) // If enemy are in range of player and if not dead, enemy attacks player. 
            {
                TimeSinceLastSawPlayer = 0; // if enemy can attack player, time since last saw must be zero.
                AttackBehaviour(); // Attack to the player.
            }

            else if (TimeSinceLastSawPlayer < SuspicionTime)
            {
                SuspicionBehaviour(); // If player escapes, go to last known location and wait there for suspicion time.
            }

            else
            {
                PatrolBehaviour(); // End of suspicion time, start patoling on the patrorl path.
            }

            UpdateTimers(); //Incease TimeSinceLastSawPlayer and TimeSinceArrivedWaypoint.
        }

        private void UpdateTimers()
        {
            TimeSinceLastSawPlayer += Time.deltaTime;
            TimeSinceArrivedWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            // Return to patrol when enemy escapes
            Vector3 NextPosition=GuardPosition;
            if(PatrolPath!=null)
            {
                if(AtWayPoint())
                {
                    TimeSinceArrivedWaypoint=0; // From now, wait on patrol path point, and move to the next.
                    CycleWaypoint();
                }
            NextPosition=GetCurrentWayPoint();
            }    
            
            if(TimeSinceArrivedWaypoint>WaypointDwellTime) //
            GetComponent<Mover>().StartMoveAction(NextPosition,PatrolSpeedFraction);
        }

        private void SuspicionBehaviour()
        {
           // Start suspicion when player out of sight.
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            // Attack player if it is in range.
            TimeSinceLastSawPlayer=0;
            Fighter.Attack(Player);
        }

        private bool InAttackRangeOfPlayer()
        {
            // Calculate if enemy in the range of attack to player.
            return Vector3.Distance(Player.transform.position,transform.position)<ChaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color= Color.blue;
            Gizmos.DrawWireSphere(transform.position,ChaseDistance);
        }

        private bool AtWayPoint()
        {
            float DistanceAtWayPoint=Vector3.Distance(transform.position,GetCurrentWayPoint());
            return DistanceAtWayPoint < WaypointTolarance;
        }

        private void CycleWaypoint()
        {
            //Select next point of the patrol path.
            CurrentWaypointIndex=PatrolPath.GetNextIndex(CurrentWaypointIndex);
        }

        private Vector3 GetCurrentWayPoint()
        {
            // Returns current waypoint of patrol path for calculate next.
            return PatrolPath.GetWaypoint(CurrentWaypointIndex);
        }




    }
}

