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
    public event Action<int> AnnounceHP;
    public event Action<int> AnnounceHPChangedBy;
    public event Action<ItemSO> AnnounceHitByWeapon;
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

    [SerializeField] private List<HealthStatus> statuses = new();

    public IReadOnlyList<HealthStatus> Statuses => statuses;

    void OnEnable()
    {
        StartCoroutine(CheckBurning());
    }

    public void AddStatus(HealthStatus s)
    {
        statuses.Add(s);
        AnnounceHealthStatus?.Invoke(statuses);
    }

    public int CountStatus(HealthStatus s)
    {
        int count = 0;
        foreach (var status in statuses)
            if (status == s)
                count++;
        return count;
    }

    public void RemoveStatus(HealthStatus s)
    {
        if (statuses.Remove(s))
            AnnounceHealthStatus?.Invoke(statuses);
    }

    public bool HasStatus(HealthStatus s) => statuses.Contains(s);
    
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

    public virtual void ChangeHP(int value)
    {
        if (!CanChangeHP)
            return;

        CurrentHP += value;
        AnnounceHPChangedBy?.Invoke(value);
    }

    public virtual void HitByWeapon(ItemSO aItemSo)
    {
        AnnounceHitByWeapon?.Invoke(aItemSo);

        //probably not
        if (aItemSo.weaponDamage == 0)
        {
            if (Statuses.Contains(HealthStatus.Asleep))
                RemoveStatus(HealthStatus.Asleep);
            else
                AddStatus(HealthStatus.Asleep);
        }

        ChangeHP(aItemSo.weaponDamage);
    }


    //TODO: Fix hardcoding values
    IEnumerator CheckBurning()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (!Alive)
                continue;

            int healCount = CountStatus(HealthStatus.Healing);
            for (int i = 0; i < healCount; i++)
            {
                yield return new WaitForFixedUpdate();
                ChangeHP(1);
            }

            if (Statuses.Contains(HealthStatus.Wet))
            {
                statuses.RemoveAll(s => s == HealthStatus.Burning);
                continue;
            }

            int burnCount = CountStatus(HealthStatus.Burning);
            for (int i = 0; i < burnCount; i++)
            {
                yield return new WaitForFixedUpdate();
                ChangeHP(-1);
            }

            int bleedCount = CountStatus(HealthStatus.Bleeding);
            for (int i = 0; i < bleedCount; i++)
            {
                yield return new WaitForFixedUpdate();
                ChangeHP(-1);
            }
        }
    }
}