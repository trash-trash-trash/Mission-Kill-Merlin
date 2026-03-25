using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public Dictionary<CharacterBase, MemoryData> memories = new();

    public List<CharacterBase> knownCharacters = new List<CharacterBase>();
    public List<MemoryData> memoriesList = new List<MemoryData>();

    public bool heardSound = false;
    public bool deadAlly = false;
    public bool sleepingAlly = false;
    public bool awareOfPlayer = false;
    public event Action<List<MemoryData>> AnnounceMemory; 

    public void AddOrUpdateMemory(MemoryData mem)
    {
        // Check if we already have a memory of this character
        if (memories.TryGetValue(mem.character, out MemoryData existing))
        {
            // Update only if something has changed
            if (existing.memoryType != mem.memoryType || 
                (mem.character.transform.position - existing.position).sqrMagnitude > 0.01f)
            {
                existing.memoryType = mem.memoryType;
                existing.position = mem.position;
                existing.timeMemoryAdded = Time.time;
                existing.decayTime = mem.decayTime;
            }
        }
        else
        {
            memories[mem.character] = mem;
        }
        
        // if (mem.memoryType == MemoryEnum.Sound)
            StartCoroutine(DecayMemory(mem));
        
        // Recalculate status flags
        RecalculateStatusFlags();
    }

    IEnumerator DecayMemory(MemoryData mem)
    {
        while (mem.decayTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            mem.decayTime -= 1f;
        }

        RemoveMemory(mem.character);
    }

    public void RemoveMemory(CharacterBase character)
    {
        if (memories.TryGetValue(character, out MemoryData mem))
        {
            memoriesList.Remove(mem);
            memories.Remove(character);
            RecalculateStatusFlags(); 
        }
    }

    private void RecalculateStatusFlags()
    {
        sleepingAlly = memories.Values.Any(mem => mem.memoryType == MemoryEnum.SleepingChar);
        deadAlly = memories.Values.Any(mem => mem.memoryType == MemoryEnum.DeadChar);
        heardSound = memories.Values.Any(mem => mem.memoryType == MemoryEnum.Sound);
        awareOfPlayer = memories.Values.Any(mem => mem.memoryType == MemoryEnum.LastSeenPlayer);
        
        knownCharacters = new List<CharacterBase>(memories.Keys);
        memoriesList = new List<MemoryData>(memories.Values);
        
        AnnounceMemory?.Invoke(memoriesList);
    }

    public bool GetMostRecentMemoryOfType(MemoryEnum type, out MemoryData mem)
    {
        mem = null;
        float latestTime = float.MinValue;

        foreach (var memory in memories.Values)
        {
            if (memory.memoryType == type && memory.timeMemoryAdded > latestTime)
            {
                mem = memory;
                latestTime = memory.timeMemoryAdded;
            }
        }

        return mem != null;
    }
}
