using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Clothes : MonoBehaviour
{
    public Character clothesCharacter;

    private Color skinColor;
    
    public Dictionary<Character, Color> clothesColorDict = new Dictionary<Character, Color>();

    //fix later with models
    public List<MeshRenderer> skinRenderers = new List<MeshRenderer>();
    public List<MeshRenderer> clothesRenderers = new List<MeshRenderer>();
    
    private bool clothed = true;

    public bool Clothed
    {
        get { return clothed; }
        set { clothed = value; }
    }

    public SkinTones skinTones;

    public event Action<Character> AnnounceCharacterClothes;

    public void Awake()
    {
        clothesColorDict.Add(Character.Civilian, Color.green);
        clothesColorDict.Add(Character.Guard, Color.blue);
        clothesColorDict.Add(Character.Target, Color.red);
        clothesColorDict.Add(Character.Player, Color.black);

        int rand = Random.Range(0, skinTones.skinTones.Count);
        skinColor = skinTones.skinTones[rand];

        foreach (MeshRenderer meshRenderer in skinRenderers)
        {
            meshRenderer.material.color = skinColor;
        }

        ChangeClothes(clothesCharacter);
    }

    public void ChangeClothes(Character characterClothes)
    {
        clothesCharacter = characterClothes;
        
        foreach (MeshRenderer meshRenderer in clothesRenderers)
        {
            meshRenderer.material.color = clothesColorDict[characterClothes];;
        }
    }

    public void Undress()
    {
        Clothed = false;
        
        foreach (MeshRenderer meshRenderer in clothesRenderers)
        {
            meshRenderer.material.color = skinColor;
        }
    }
}
