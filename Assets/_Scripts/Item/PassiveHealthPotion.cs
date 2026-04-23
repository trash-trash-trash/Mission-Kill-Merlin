using UnityEngine;

public class PassiveHealthPotion : MonoBehaviour
{
   public int healthRestore = 10;
   
   public void OnTriggerEnter(Collider other)
   {
      if (other.GetComponentInChildren<Health>() != null)
      {
         Health hp =  other.GetComponentInChildren<Health>();

         int currentHP = hp.CurrentHP;
         hp.ChangeHP(healthRestore);
         
         //seems sloppy
         if(hp.CurrentHP != healthRestore)
            Destroy(gameObject);
      }
   }
}
