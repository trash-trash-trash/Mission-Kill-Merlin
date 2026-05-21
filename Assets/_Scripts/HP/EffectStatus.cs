using UnityEngine;

public enum Effects
{
    None,
    Bleeding,
    BleedingAesthetic,
    Burning,
    Healing,
    Wet,
    Asleep,
    Lightning,
    Dead
}

[System.Serializable]
public class EffectStatus
{
    public Effects effect;
    public int amount = 1;     // how much HP change per tick
    public int charges = 1;    // how many ticks left
    public float interval = 1f;

    private float timer;

    public bool Tick(Health health, float deltaTime)
    {
        timer += deltaTime;

        if (timer < interval)
            return false;

        timer = 0f;

        health.ChangeHP(amount);

        charges--;

        return charges <= 0;
    }
}