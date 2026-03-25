using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;
using UnityEngine.AI;

public class NPCAnthillBase : MonoBehaviour, ISense, IInteractable, IHear
{
    public CharacterBase characterBase;
    public Clothes clothes;
    public NPCInventory inventory;
    public Health hp;
    public Memory memory;
    public NavMeshAgent navMeshAgent;
    public NPCHeadLook npcHeadLook;
    public PatrolPaths patrolPaths;
    public Patrol patrol;
    public Sight sight;
    public Sound sound;

    public Transform playerTransform;
    public Transform shootPoint;

    public DebugStateText debugText;

    //civs run away when alerted, aggros fight
    public bool aggro = false;

    public bool spawned = false;
    public bool alive = false;
    public bool awake = false;
    public bool susMid = false;
    public bool susSeenPlayer = false;
    public bool alert = false;
    public bool doJob = false;
    public bool idle = false;
    public bool shoved = false;
    public bool hasWeapon = false;
    public bool inRangeToAttack = false;

    private bool canHear = true;

    public float minimumAttackRange = 15f;

    public bool CanHear
    {
        get { return canHear; }
        set { canHear = value; }
    }

    void Start()
    {
        characterBase.AnnounceShoved += Shoved;
        characterBase.AnnounceSlept += Slept;
        memory.AnnounceMemory += CheckMemory;
        sight.AnnounceCanSeeCharacter += CheckCharacter;
        sight.AnnounceCanSeePlayer += SeenPlayer;
        hp.AnnounceHP += CheckAlive;
        hp.AnnounceHealthStatus += SetStatus;

        if (inventory.equippedWeapon != null)
            hasWeapon = true;
        
        hp.ChangeHP(hp.maxHP);
    }

    private void CheckAlive(int aObj)
    {
        if (aObj <= 0)
            alive = false;
        else
            alive = true;
    }

    private void CheckMemory(List<MemoryData> aObj)
    {
        if (memory.awareOfPlayer)
        {
            if (playerTransform.GetComponentInChildren<Health>().Alive)
            { 
                susSeenPlayer = true;
                alert = true;
            }
            else
            {
                susSeenPlayer = true;
                alert = false;
            }
        }
        else
        {
            alert = false;
            susSeenPlayer = false;
        }
    }

    private void SetStatus(HealthStatus newStatus)
    {
        if (newStatus == HealthStatus.Asleep || newStatus == HealthStatus.Dead)
        {
            navMeshAgent.enabled = false;
            CanHear = false;
            sight.CanSee = false;

            if (newStatus == HealthStatus.Asleep)
                awake = false;
            else if (newStatus == HealthStatus.Dead)
                alive = false;
        }
        else
        {
            alive = true;
            awake = true;
            CanHear = true;
            sight.CanSee = true;
        }
    }

    private void Slept()
    {
        awake = false;
    }

    private void CheckCharacter(CharacterBase obj)
    {
        //change here if want to track objects in memory
        if (obj.hp == null)
            return;

        if (!obj.hp.Alive)
        {
            //repetition
            MemoryData newMemory = new MemoryData()
            {
                memoryType = MemoryEnum.DeadChar,
                character = obj,
                position = obj.transform.position,
                timeMemoryAdded = Time.time,
                //max?
                decayTime = 30
            };

            if (obj.character == Character.Player)
            {
                alert = false;
                susSeenPlayer = false;
            }
            
            memory.AddOrUpdateMemory(newMemory);
            
            if (memory.deadAlly)
                alert = true;
        }

        else if (obj.hp.HasStatus(HealthStatus.Asleep))
        {
            MemoryData newMemory = new MemoryData()
            {
                memoryType = MemoryEnum.SleepingChar,
                character = obj,
                position = obj.transform.position,
                timeMemoryAdded = Time.time,
                decayTime = 30
            };

            memory.AddOrUpdateMemory(newMemory);
        }
        
        else if (obj.character == Character.Player)
        {
            MemoryData newMemory = new MemoryData()
            {
                memoryType = MemoryEnum.LastSeenPlayer,
                character = obj,
                position = obj.transform.position,
                timeMemoryAdded = Time.time,
                decayTime = 30
            };
            
            playerTransform = obj.transform;
            memory.AddOrUpdateMemory(newMemory);
        }
        else
        {
            MemoryData newMemory = new MemoryData()
            {
                memoryType = MemoryEnum.Character,
                character = obj,
                position = obj.transform.position,
                timeMemoryAdded = Time.time,
                decayTime = 3
            };

            memory.AddOrUpdateMemory(newMemory);
        }
        
    }

    private void SeenPlayer(Player player, bool canSeePlayer)
    {
        if (canSeePlayer)
        {
            //  if (player.AggroAction)
            //alert = true;
        }
    }

    private void Shoved()
    {
        shoved = true;
    }

    public void HeardSound(SoundData sound)
    {
        if (!CanHear)
            return;

        MemoryData mem = new MemoryData()
        {
            character = sound.soundSource,
            memoryType = MemoryEnum.Sound,
            decayTime = sound.soundDecayTime,
            position = sound.soundOrigin,
            timeMemoryAdded = Time.time,
            soundData = sound
        };
        memory.AddOrUpdateMemory(mem);
    }

    public void FlipCanHear(bool input)
    {
        CanHear = input;
    }

    public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
        aWorldState.Set(NPCBaseScenario.Spawned, spawned);
        aWorldState.Set(NPCBaseScenario.Alive, alive);
        aWorldState.Set(NPCBaseScenario.Awake, awake);
        aWorldState.Set(NPCBaseScenario.SusMid, susMid);
        aWorldState.Set(NPCBaseScenario.SusSeenPlayer, susSeenPlayer);
        aWorldState.Set(NPCBaseScenario.Alert, alert);
        aWorldState.Set(NPCBaseScenario.DoingJob, doJob);
        aWorldState.Set(NPCBaseScenario.Idle, idle);
        aWorldState.Set(NPCBaseScenario.Shoved, shoved);
        aWorldState.Set(NPCBaseScenario.HeardSound, memory.heardSound);
        aWorldState.Set(NPCBaseScenario.SleepingAlly, memory.sleepingAlly);
        aWorldState.Set(NPCBaseScenario.HasWeapon, hasWeapon);
        aWorldState.Set(NPCBaseScenario.InRangeToAttack, inRangeToAttack);
    }

    public void Interact(IInteract interactee, CharacterActions actionType)
    {
        //fix
        if (actionType == CharacterActions.Undress)
        {
            if (!awake || !hp.Alive)
                clothes.Undress();
        }
        else if (actionType == CharacterActions.WakeUp)
            if (!awake)
            {
                hp.RemoveStatus(HealthStatus.Asleep);
            }
    }

    public GameObject ReturnSelf()
    {
        return gameObject;
    }

    private bool canInteract = true;

    public bool CanInteract
    {
        get { return canInteract; }
        set { canInteract = value; }
    }

    public bool ReturnCanInteract()
    {
        return CanInteract;
    }
}