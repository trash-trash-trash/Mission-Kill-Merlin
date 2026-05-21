using UnityEngine;

public class WeaponBaseState : MonoBehaviour
{
    public WeaponBrain weaponBrain;

    public virtual void OnEnable()
    {
        weaponBrain = GetComponentInParent<WeaponBrain>();
    }
}