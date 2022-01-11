using System;
using System.Collections;
using System.Collections.Generic;
using U_RPG.Combat;
using U_RPG.Core;
using U_RPG.Movement;
using UnityEngine;
using U_RPG.Resources;
using GameDevTV.Utils;

namespace U_RPG.Control
{
    // This class is controlling enemy behavior. This class is about enemy.

    public class AIController : MonoBehaviour
    {
        [SerializeField] float ChaseDistance = 5f;
        [SerializeField] float SuspicionTime = 3f; // Enemy become suspicious when it looses playerr for 3 seconds.
        [SerializeField] PatrolPath patrolPath; // Eemy patrol Path
        [SerializeField] float WaypointTolerance = 1f;
        [SerializeField] float WaypointDwellTime = 3f; // Wait for 3 seconds in patrol points
        [Range(0,1)]
        [SerializeField] float PatrolSpeedFraction = 0.2f; // During patrol, move slover.

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject Player;

        LazyValue<Vector3> GuardPosition;
        float TimeSinceLastSawPlayer = Mathf.Infinity; // Time since enemy saw player.
        float TimeSinceArrivedAtWaypoint = Mathf.Infinity; // Time since enemy waits on patrol waypoint.
        int CurrentWaypointIndex = 0; // Patrol path indexes.

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            Player = GameObject.FindWithTag("Player");

            GuardPosition = new LazyValue<Vector3>(GetPosition);


        }


        private void Start()
        {
            // Locate the enemy (self) position.
            GuardPosition.ForceInit();
        }

        private Vector3 GetPosition()
        {
            return transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(Player)) // If enemy are in range of player and if not dead, enemy attacks player. 
            {   
                //TimeSinceLastSawPlayer=0;
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
            TimeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            // Return to patrol when enemy escapes
            Vector3 NextPosition = GuardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    TimeSinceArrivedAtWaypoint = 0; // From now, wait on patrol path point, and move to the next.
                    CycleWaypoint();
                }
                NextPosition = GetCurrentWaypoint();
            }

            if (TimeSinceArrivedAtWaypoint > WaypointDwellTime)
            {
                mover.StartMoveAction(NextPosition, PatrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float DistanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return DistanceToWaypoint < WaypointTolerance;
        }

        private void CycleWaypoint()
        {
            //Select next point of the patrol path.
            CurrentWaypointIndex = patrolPath.GetNextIndex(CurrentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            // Returns current waypoint of patrol path for calculate next.
            return patrolPath.GetWaypoint(CurrentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            // Start suspicion when player out of sight.
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            // Attack player if it is in range.
            TimeSinceLastSawPlayer = 0;
            fighter.Attack(Player);
        }

        private bool InAttackRangeOfPlayer()
        {
            // Calculate if enemy in the range of attack to player.
            float DistanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
            return DistanceToPlayer < ChaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, ChaseDistance);
        }
    }
}