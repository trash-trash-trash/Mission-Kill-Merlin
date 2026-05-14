using System.Collections.Generic;
using UnityEngine;

public class PuddleView : MonoBehaviour
{
    //improve...

    public Puddle puddle;

    //put this somewhere else

    public Renderer renderer;

    void OnEnable()
    {
        puddle.AnnounceSet += SetMaterial;
    }

    private void SetMaterial(Puddle aObj)
    {
        renderer.material = GlowingMatsDict.Instance.GetMaterial(aObj.data.associatedStatus);
        //
        //
        // if (GlowingMatsDict.Instance.materialsDict.TryGetValue(aObj.data.associatedStatus, out Material mat))
        // {
        //     renderer.material = mat;
        // }
    }

    void OnDisable()
    {
        puddle.AnnounceSet -= SetMaterial;
    }
}