using System;
using Anthill.AI;
using UnityEngine;

public class PlayerBrain : MonoBehaviour, ISense
{
    public Health hp;
    public MouseLook mouseLook;
    public PlayerMove move;

    public bool canInteract = false;
    public bool channelling = false;
    public bool interactMenu = false;
    public bool alive = true;
    public bool escaped = false;
    
    public event Action<bool> AnnounceCanInteract;
    
    public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
        aWorldState.Set(PlayerScenario.Channelling, channelling);
        aWorldState.Set(PlayerScenario.InteractMenu, interactMenu);
        aWorldState.Set(PlayerScenario.Alive, hp.Alive);
        aWorldState.Set(PlayerScenario.Escaped, escaped);
    }
    
    public void CanInteract(bool input)
    {
        canInteract = input;
        AnnounceCanInteract?.Invoke(input);
    }

    public void OnEnable()
    {
        int damage = hp.maxHP * 90 / 100;
        hp.ChangeHP(-damage);
        Debug.Log($"maxHP: {hp.maxHP}, currentHP: {hp.CurrentHP}, damage: {damage}");
    }
}
