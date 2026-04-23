using System.Collections.Generic;
using UnityEngine;

public class Glasses : MonoBehaviour
{
    public List<GameObject> glasses = new List<GameObject>();

    public Transform glassesTransform;

    void OnEnable()
    {
        EquipRandomGlasses();
    }

    public void EquipRandomGlasses()
    {
        // Pick random
        int index = Random.Range(0, glasses.Count);
        GameObject newGlasses = Instantiate(glasses[index], glassesTransform.position, glassesTransform.rotation,
            glassesTransform);
//            new Quaternion(glassesTransform.rotation.x, glassesTransform.rotation.y,glassesTransform.rotation.z, glassesTransform.rotation.w),  glassesTransform);
    }
}