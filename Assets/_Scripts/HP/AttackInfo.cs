using System;
using UnityEngine;

[Serializable]
public class AttackInfo
{
    public CharacterBase attackOwner;
    public CharacterBase target;
    public EffectStatus effectStatus;
    public int damage;

    public float windUp = 0.4f;
    public float activeTime = 1;
    public float cooldown = 0.3f;

    public bool applyRBForce = true;

    public float horizontalForce = 5f;
    public float verticalForce = 2f;

    public ForceMode forceMode = ForceMode.Impulse;
}
