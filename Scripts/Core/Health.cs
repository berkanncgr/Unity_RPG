using UnityEngine;

namespace U_RPG.Core
{
    // This class uses by enemy and player.
    public class Health : MonoBehaviour
    {

    [SerializeField] float HealthPoints=100f;
    bool bIsDead=false;

    public bool IsDead()
    {
        return bIsDead;
    }

    public void TakeDamage(float Damage)
    {
        HealthPoints=Mathf.Max(HealthPoints-Damage,0); // Take damage from attacks. 
        if(HealthPoints ==0) Die(); // If healt points equals zero, Die.
        GetComponent<ActionScheduler>().CancelCurrentAction(); //Update tthe action log. Player is not moving.
    }

   private void Die()
   {
       if(bIsDead) return; // Dont repeat the death animation over over again.
       
       bIsDead=true;
       GetComponent<Animator>().SetTrigger("die"); // Play death animation.
   }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    }
}