using System.Collections.Generic;
using UnityEngine;

public class MagicPotionView : MonoBehaviour
{
    public MagicPotion mP;
    
    public Renderer potionRenderer;

    void Start()
    {
        mP.AnnounceBottleType += SetBottle;
        
        SetBottle(mP.magicPotionType);
    }

    private void SetBottle(Effects potionType)
    {
        if (GlowingMatsDict.Instance.materialsDict.TryGetValue(potionType, out Material material))
        {
            potionRenderer.enabled = true;
            potionRenderer.material = material;
        }
        else
            potionRenderer.enabled = false;
    }

    void OnDisable()
    {
        mP.AnnounceBottleType -= SetBottle;
    }
}
