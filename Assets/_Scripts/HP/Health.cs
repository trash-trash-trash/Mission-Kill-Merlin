using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public enum HealthStatus
{
    Fine,
    Bleeding,
    BleedingAesthetic,
    Burning,
    Healing,
    Wet,
    Asleep,
    Lightning,
    Dead
}

public class Health : MonoBehaviour
{
    //condense...
    public event Action<bool> AnnounceAlive;
    
    public event Action<int> AnnounceHP;
    public event Action<int> AnnounceHPChangedBy;
    
    public event Action<List<HealthStatus>> AnnounceHealthStatus;

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
    
    public int CurrentHP
    {
        get { return currentHP; }
        private set
        {
            currentHP = Mathf.Clamp(value, 0, maxHP);
            Alive = currentHP > 0;

            AnnounceHP?.Invoke(currentHP);
        }
    }
    
    public void AddEffect(EffectStatus effect)
    {
        //water removes fire :)
        if (effect.type == HealthStatus.Wet)
        {
            RemoveEffect(HealthStatus.Burning);
        }

        effects.Add(effect);
    }

    public void RemoveEffect(HealthStatus status)
    {
        
        effects.RemoveAll(e => e.type == status);
    }

    public bool HasEffect(HealthStatus type)
    {
        foreach (EffectStatus effect in effects)
        {
            if (effect.type == type)
                return true;
        }

        return false;
    }

    public virtual void ChangeHP(int value)
    {
        if (!CanChangeHP)
            return;

        CurrentHP += value;
        AnnounceHPChangedBy?.Invoke(value);

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