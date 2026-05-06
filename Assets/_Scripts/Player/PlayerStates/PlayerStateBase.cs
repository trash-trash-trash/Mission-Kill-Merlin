using UnityEngine;

public class PlayerStateBase : MonoBehaviour
{
   public PlayerBrain playerBrain;

   public virtual void EnterState()
   {
      playerBrain = GetComponentInParent<PlayerBrain>();
   }

   public virtual void SetStateText(string text)
   {
      playerBrain.stateText.text = text;
   }

   public virtual void ExitState()
   {
      
   }
}
