using UnityEngine;

public class LineArc : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private int resolution = 30;

    [SerializeField] private float distance = 10f;   // how far B is
    [SerializeField] private float arcHeight = 3f;   // how high the arc goes

    public Transform startPoint;
    public Transform forwardReference; // usually camera or player

    public bool drawingArc = false;

    public float forceMultiplier = 5f;

    void Update()
    {
        if (!drawingArc)
        {
            line.positionCount = 0; 
            return;
        }
        DrawArc();
    }
    
    //AI to draw line based on physics
    public Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 end, float height)
    {
        float gravity = Physics.gravity.y;

        // Calculate displacement
        Vector3 displacement = end - start;
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);

        // Time to reach peak
        float timeUp = Mathf.Sqrt(-2 * height / gravity);
        float timeDown = Mathf.Sqrt(2 * (displacement.y - height) / gravity);
        float totalTime = timeUp + timeDown;

        // Velocity
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / totalTime;

        return velocityXZ + velocityY;
    }

    void DrawArc()
    {
        Vector3 start = startPoint.position;
        Vector3 end = start + forwardReference.forward * distance * forceMultiplier;

        Vector3 velocity = CalculateLaunchVelocity(start, end, arcHeight * forceMultiplier);

        line.positionCount = resolution;

        float timestep = 0.1f; // smaller = smoother, more accurate
        
        Vector3 prevPoint = start;

        for (int i = 0; i < resolution; i++)
        {
            float t = i * timestep;

            Vector3 point = start 
                            + velocity * t 
                            + 0.5f * Physics.gravity * t * t;

            if (Physics.Linecast(prevPoint, point, out RaycastHit hit))
            {
                line.positionCount = i + 1;
                line.SetPosition(i, hit.point);
                break;
            }

            line.SetPosition(i, point);
            prevPoint = point;
        }
    }
    
    public void ThrowRigidBodyAlongArc(Rigidbody rbToThrow)
    {
        Vector3 A = startPoint.position;
        Vector3 B = A + forwardReference.forward * distance;

        Vector3 velocity = CalculateLaunchVelocity(A, B, arcHeight) * forceMultiplier;

        rbToThrow.linearVelocity = velocity;
    }
}
