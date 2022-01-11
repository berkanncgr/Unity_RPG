using U_RPG.Combat;
using U_RPG.Movement;
using UnityEngine;
using U_RPG.Resources;
using System;
using UnityEngine.EventSystems;

namespace U_RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        enum CursorType
        {
            None,
            Movement,
            Combat,
            PickUp,

            UI
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType cursorType;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] CursorMappings = null;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {   
            if(InteractWithUI()) return;

            if (health.IsDead()) 
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            if(InteractWithPickUp())

            SetCursor(CursorType.None);
        }

        private bool InteractWithPickUp()
        {
            SetCursor(CursorType.PickUp);
            throw new NotImplementedException();
        }

        private bool InteractWithUI()
        {   
            if(EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }

            return false;
        }

        // Start to fight if possible
        private bool InteractWithCombat()
        {
            RaycastHit[] Hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit Hit in Hits)
            {
                CombatTarget Target = Hit.transform.GetComponent<CombatTarget>();
                if (Target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(Target.gameObject))
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(Target.gameObject);
                }

                SetCursor(CursorType.Combat);
                return true;
            }
            return false;
        }

 

        // Go to left mouse click.
        private bool InteractWithMovement()
        {
            RaycastHit Hit;
            bool bHasHit = Physics.Raycast(GetMouseRay(), out Hit);
            if (bHasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(Hit.point, 1f);
                }


                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping cursorMapping = GetCursorMapping(type);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
            

        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in CursorMappings)
            {
                if(mapping.cursorType == type)
                {
                    return mapping;
                }
            }

            return CursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}