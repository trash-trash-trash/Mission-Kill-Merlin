using UnityEngine;

public enum AttackType
{
    Nothing,
    Stun,
    Damage,
    Effect
}

public class AttackInfo
{
    public CharacterBase attackOwner;
    public AttackType attackType;
    public int damage;
    public HealthStatus effect;
}
