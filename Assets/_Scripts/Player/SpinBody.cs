using UnityEngine;

public class SpinBody : MonoBehaviour
{
    public PlayerMove playerMove;

    public Transform bodyTransform;
    public float rotationSpeed = 10f;

    void OnEnable()
    {
        playerMove.AnnounceMoveVector += OnMove;
    }

    void OnMove(Vector3 worldMove)
    {
        Vector3 moveDir = new Vector3(worldMove.x, 0f, worldMove.z);
        if (moveDir.sqrMagnitude < 0.001f) return; 

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        bodyTransform.rotation =
            Quaternion.Slerp(bodyTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void OnDisable()
    {
        playerMove.AnnounceMoveVector -= OnMove;
    }
}