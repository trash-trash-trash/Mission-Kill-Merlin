using UnityEngine;

public class RotateTransform : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 100f;
    [SerializeField] private bool spinRight = true;

    public bool rotating = true;

    void Update()
    {
        if (rotating)
        {
            float direction = spinRight ? 1f : -1f;
            transform.Rotate(0f, spinSpeed * direction * Time.deltaTime, 0f);
        }
    }
}