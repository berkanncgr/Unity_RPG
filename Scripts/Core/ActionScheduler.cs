using UnityEngine;

namespace U_RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction CurrentAction;

        // Switch between actions. Attack or move. Becouse same button attached to that actions.
        public void StartAction(IAction action)
        {
            if (CurrentAction == action) return;
            if (CurrentAction != null)
            {
                CurrentAction.Cancel();
            }
            CurrentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}