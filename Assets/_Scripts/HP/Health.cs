using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //condense...
    public event Action<bool> AnnounceAlive;

    public event Action<int> AnnounceHP;

    public event Action<List<EffectStatus>> AnnounceHealthStatus;

    [SerializeField] private int currentHP;

    [SerializeField] public int maxHP = 1;

    [SerializeField] private bool alive = false;

    public bool canTakeDamage = true;
    public bool canTakeStatus = true;

    public bool Alive
    {
        get => alive;
        set => alive = value;
    }

    public List<EffectStatus> effects = new();

    public bool CanChangeHP { get; private set; } = true;

    private void Awake()
    {
        ChangeHP(maxHP);
    }

    public int CurrentHP
    {
        get { return currentHP; }
        private set
        {
            currentHP = Mathf.Clamp(value, 0, maxHP);
            Alive = currentHP > 0;

            AnnounceHP?.Invoke(currentHP);

            AnnounceAlive?.Invoke(alive);
        }
    }

    public void AddEffect(EffectStatus effect)
    {
        //water removes fire :)
        if (effect.effect == Effects.Wet)
        {
            RemoveEffect(Effects.Burning);
        }

        effects.Add(effect);

        AnnounceHealthStatus?.Invoke(effects);
    }

    public void RemoveEffect(Effects status)
    {
        effects.RemoveAll(e => e.effect == status);

        AnnounceHealthStatus?.Invoke(effects);
    }

    public bool HasEffect(Effects type)
    {
        foreach (EffectStatus effect in effects)
        {
            if (effect.effect == type)
                return true;
        }

        return false;
    }

    public virtual void ChangeHP(int value)
    {
        if (!CanChangeHP)
            return;

        CurrentHP += value;

        if (CurrentHP <= 0)
        {
            Alive = false;
            AnnounceAlive?.Invoke(false);
        }
    }

    void Update()
    {
        if (!Alive) return;

        for (int i = effects.Count - 1; i >= 0; i--)
        {
            bool finished = effects[i].Tick(this, Time.deltaTime);

            if (finished)
            {
                effects.RemoveAt(i);
            }
        }
    }
}