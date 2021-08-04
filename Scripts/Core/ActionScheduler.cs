using UnityEngine;

namespace U_RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {

        IAction CurrentAction;

        public void StartAction(IAction Action)
        {
            // Cancells attack if moving is start; Cancells moving if attack is start.
            if (CurrentAction==Action) return;
            if(CurrentAction!=null)
            {
                CurrentAction.Cancel();
                print("Cancellinng "+CurrentAction);
            } 
            CurrentAction=Action;
        }
        
        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    }
}
