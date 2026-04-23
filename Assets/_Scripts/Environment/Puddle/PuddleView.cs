using System.Collections.Generic;
using UnityEngine;

public class PuddleView : MonoBehaviour
{
    //improve...
    
    public Puddle puddle;
    
    //put this somewhere else
    public Material healthMaterial;
    public Material fireMaterial;
    public Material waterMaterial;

    public Renderer renderer;
    
    public Dictionary<HealthStatus, Material> healthMaterialsDict;

    void Awake()
    {
        healthMaterialsDict = new Dictionary<HealthStatus, Material>()
        {
            { HealthStatus.Healing, healthMaterial },
            { HealthStatus.Burning, fireMaterial },
            { HealthStatus.Wet, waterMaterial }
        };
    }

    void OnEnable()
    {
        puddle.AnnounceSet += SetMaterial;
    }

    private void SetMaterial(Puddle aObj)
    {
        if (healthMaterialsDict.TryGetValue(aObj.data.associatedStatus, out Material mat))
        {
            renderer.material = mat;
        }
    }

    void OnDisable()
    {
        puddle.AnnounceSet -= SetMaterial;
    }
}
