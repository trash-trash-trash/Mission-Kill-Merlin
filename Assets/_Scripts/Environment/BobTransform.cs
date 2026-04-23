using UnityEngine;

public class BobTransform : MonoBehaviour
{
    public float height = 0.25f;
    public float speed = 1f;

    private Vector3 startPos;

    public bool bobbing = true;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        if (bobbing)
        {
            float yOffset = Mathf.Sin(Time.time * speed) * height;
            transform.localPosition = startPos + Vector3.up * yOffset;
        }
    }
}