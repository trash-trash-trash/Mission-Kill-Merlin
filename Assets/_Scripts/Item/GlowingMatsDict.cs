using System.Collections.Generic;
using UnityEngine;

public class GlowingMatsDict : MonoBehaviour
{
    public static GlowingMatsDict Instance { get; private set; }

    public Material bleedMat;
    public Material burningMat;
    public Material healMat;
    public Material lightningMat;

    public Dictionary<Effects, Material> materialsDict = new Dictionary<Effects, Material>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // DontDestroyOnLoad(gameObject);
        materialsDict.Add(Effects.Bleeding, bleedMat);
        materialsDict.Add(Effects.Burning, burningMat);
        materialsDict.Add(Effects.Healing, healMat);
        materialsDict.Add(Effects.Lightning, lightningMat);
    }

    public Material GetMaterial(Effects status)
    {
        materialsDict.TryGetValue(status, out Material mat);
        return mat;
    }
}
