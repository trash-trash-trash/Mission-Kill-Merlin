using System.Collections.Generic;
using UnityEngine;

public class GlowingMatsDict : MonoBehaviour
{
    public static GlowingMatsDict Instance { get; private set; }

    public Material bleedMat;
    public Material burningMat;
    public Material healMat;
    public Material lightningMat;

    public Dictionary<HealthStatus, Material> materialsDict = new Dictionary<HealthStatus, Material>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // DontDestroyOnLoad(gameObject);
        materialsDict.Add(HealthStatus.Bleeding, bleedMat);
        materialsDict.Add(HealthStatus.Burning, burningMat);
        materialsDict.Add(HealthStatus.Healing, healMat);
        materialsDict.Add(HealthStatus.Lightning, lightningMat);
    }

    public Material GetMaterial(HealthStatus status)
    {
        materialsDict.TryGetValue(status, out Material mat);
        return mat;
    }
}
