using U_RPG.Core;
using U_RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using U_RPG.Resources;

namespace U_RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform Target;
        [SerializeField] float MaxSpeed = 6f;

        NavMeshAgent navMeshAgent;
        Health health;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();

            // If character is moving, update animation.
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = MaxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 Velocity = navMeshAgent.velocity;
            Vector3 LocalVelocity = transform.InverseTransformDirection(Velocity);
            float Speed = LocalVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", Speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object State)
        {
            SerializableVector3 Position = (SerializableVector3)State;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = Position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}