using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class CharacterBase : MonoBehaviour, ICharacter
{
    public Character character;
    public Health hp;

    public Character ReturnCharacter()
    {
        return character;
    }

    public CharacterBase ReturnCharacterBase()
    {
        return this;
    }
}