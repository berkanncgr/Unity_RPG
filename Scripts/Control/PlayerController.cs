using UnityEngine;
using U_RPG.Movement;
using U_RPG.Combat;
using U_RPG.Core;

namespace U_RPG.Control
{   
    public class PlayerController : MonoBehaviour
    {
        Health Health;
    private void Start()
    {
        Health=GetComponent<Health>();
    }
        private void Update()
        {
            if(Health.IsDead()) return; // If we are dead do nothing.
            if(InteactWithComat()) return; // İf attacked, dont execute the lower lines of code.
            if(InteractWithMovement()) return; // if moved, dont execute the lower lines of code.
        }


        private bool InteactWithComat()
        {
            RaycastHit[] hits=Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target=hit.transform.GetComponent<CombatTarget>();
                if(target== null) continue;
                
                //If mouse hit is null, continue search.
                if(!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;
                if(Input.GetMouseButtonDown(0)) // If its enemy, call attack func.
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                    return true; // Find enemy to attack, return true.
            }
            return false; // Couldnt find enemy to attack, retrun false.
        }
        private bool InteractWithMovement()
        {
            RaycastHit hit;
            if (Physics.Raycast(GetMouseRay(), out hit)) // İf there is a moveable place
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point,1f);// move there and return true;
                }
                return true;
            }
            return false; // if there is not, return false.
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

    }
}