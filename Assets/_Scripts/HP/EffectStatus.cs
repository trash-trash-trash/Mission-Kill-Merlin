using UnityEngine;

[System.Serializable]
public class EffectStatus
{
    public HealthStatus type;
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