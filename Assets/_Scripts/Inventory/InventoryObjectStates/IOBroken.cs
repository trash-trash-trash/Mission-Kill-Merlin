using UnityEngine;

public class IOBroken : IOStateBase
{
    public AudioSource audioSource;

    public override void OnEnable()
    {
        base.OnEnable();
        audioSource.Play();
        inventoryObjectBrain.HandleEquipped(false);
        inventoryObjectBrain.sound.EmitSound(null);

        //for potions only...?

        MagicPotion mp = GetComponentInParent<MagicPotion>();
        if (mp != null)
        {
            if (mp.magicPotionType != Effects.None)
            {
                PuddleData newPuddleData = new PuddleData()
                {
                    owner = null,
                    //hack
                    associatedStatus = mp.magicPotionType,
                    secondsRemainingCharge = 10,
                    expiryTimerDecayRate = 1,
                    originalSecondsRemainingCharge = 10
                };

                PuddleController.Instance.SpawnPuddle(newPuddleData,
                    inventoryObjectBrain.gameObject.transform.position);
            }
        }
    }
}