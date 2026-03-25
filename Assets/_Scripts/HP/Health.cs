// using System;
// using UnityEngine;
//
// public class Health : MonoBehaviour
// {
//     private int currentHP;
//
//     public int CurrentHP
//     {
//         get { return currentHP; }
//         set { currentHP = value; }
//     }
//
//     private int maxHP;
//
//     public int MaxHP
//     {
//         get { return maxHP; }
//         set { maxHP = value; }
//     }
//
//     private bool alive;
//     
//     public bool Alive { get; private set; } = true;
//
//     public event Action<int> AnnounceHP;
//
//     public void ChangeHP(int value)
//     {
//         if (!alive)
//             return;
//         
//         int hp = currentHP += value;
//
//         //die
//         if (hp <= 0)
//         {
//             alive = false;
//             hp = 0;
//         }
//         
//         else if (hp > 0)
//         {
//             if (hp > maxHP)
//                 hp = maxHP;
//
//             alive = true;
//         }
//         CurrentHP = hp;
//         AnnounceHP?.Invoke(currentHP);
//     }
//
//     public void ChangeMaxHP(int value)
//     {
//         int newMaxHp = maxHP += value;
//         
//     }
// }

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public enum HealthStatus
{
    Burning,
    Wet,
    Asleep,
    Dead
}

public class Health : MonoBehaviour
{
    
    //condense...
    public event Action<int> AnnounceHP;
    public event Action<int> AnnounceHPChangedBy;
    public event Action<WeaponSO> AnnounceHitByWeapon;
    public event Action<HealthStatus> AnnounceHealthStatus;
    
    [SerializeField]
    private int currentHP;
    
    [SerializeField]
    public int maxHP = 1;

    [SerializeField]
    private bool alive = false;

    public bool Alive
    {
        get => alive;
        set => alive = value;
    }

    [SerializeField]
    private List<HealthStatus> statuses = new();

    public IReadOnlyList<HealthStatus> Statuses => statuses;

    void OnEnable()
    {
        StartCoroutine(CheckBurning());
    }

    public void AddStatus(HealthStatus s)
    {
            statuses.Add(s);
            AnnounceHealthStatus?.Invoke(s);
    }
    
    public int CountStatus(HealthStatus s)
    {
        int count = 0;
        foreach (var status in statuses)
            if (status == s) count++;
        return count;
    }

    public void RemoveStatus(HealthStatus s)
    {
        if (statuses.Remove(s))
            AnnounceHealthStatus?.Invoke(s);
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
            CanChangeHP &= Alive;
            if (!Alive)
                AddStatus(HealthStatus.Dead);
            
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

    public virtual void HitByWeapon(WeaponSO weaponSo)
    {
        AnnounceHitByWeapon?.Invoke(weaponSo);

        //probably not
        if (weaponSo.weaponDamage == 0)
        {
            if(Statuses.Contains(HealthStatus.Asleep))
                RemoveStatus(HealthStatus.Asleep);
            else
                AddStatus(HealthStatus.Asleep);
        }
        
        ChangeHP(weaponSo.weaponDamage);
    }

    //TODO: Fix hardcoding values
    IEnumerator CheckBurning()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (!Alive)
                continue;

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
        }
    }
}

