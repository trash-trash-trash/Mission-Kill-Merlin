using System;
using UnityEngine;

public class MagicPotion : MonoBehaviour
{
    [Header("Fine is empty")]
    public HealthStatus magicPotionType;
    
    public event Action<HealthStatus> AnnounceBottleType;

    public void SetBottle()
    {
        AnnounceBottleType?.Invoke(magicPotionType);
    }
}
