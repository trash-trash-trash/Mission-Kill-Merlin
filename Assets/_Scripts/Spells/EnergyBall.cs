using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnergyBallType
{
    Fire,
    Water,
    Lightning
}

public class EnergyBall : MonoBehaviour
{
    public Transform owner;
    
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 10f;

    [Header("Explosion")] public bool exploding = false;
    public float growRate = 10f;
    public float explodeForce = 10f;
    public float explodeTime = 1f;
    public float aftershockTime = 1f;

    private Vector3 dir;

    public List<Health> explodeHits = new List<Health>();

    public bool active = true;
    
    public EnergyBallType myEnergyType = EnergyBallType.Fire;
    public HealthStatus statusAffliction;
    
    
    public LayerMask layersToIgnore;
    
    public Dictionary<EnergyBallType, HealthStatus> energyBallTypeToHealthStatusDict =
        new()
        {
            [EnergyBallType.Fire] = HealthStatus.Burning
        };

    public event Action<GameObject> AnnounceExploded;
    
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void OnEnable()
    {
        transform.DetachChildren();
    }

    public void Init()
    {
        transform.position = startPoint.position;
        transform.localScale = originalScale;

        explodeHits.Clear();

        exploding = false;
        active = true;

        dir = (endPoint.position - startPoint.position).normalized;

        if (energyBallTypeToHealthStatusDict.TryGetValue(myEnergyType, out HealthStatus status))
        {
            statusAffliction = status;
        }
    }

    void FixedUpdate()
    {
        if (!exploding)
        {
            transform.position += dir * speed * Time.fixedDeltaTime;
        }
        else
        {
            transform.localScale += Vector3.one * growRate * Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!active || exploding ||  other.transform.IsChildOf(owner))
            return;
        
        Debug.Log("Fireball collided with "+other.name);
        Explode();
    }

    void OnTriggerStay(Collider other)
    {
        if (!active || !exploding ||  other.transform.IsChildOf(owner))
            return;

        Health h = other.GetComponent<Health>() ?? other.GetComponentInChildren<Health>();
        if (h && !explodeHits.Contains(h))
        {
            explodeHits.Add(h);
        }
    }

    public void Explode()
    {
        if (exploding) return;
        exploding = true;
        StartCoroutine(ExplodeRoutine());
    }

    IEnumerator ExplodeRoutine()
    {
        float t = 0f;
        while (t < explodeTime)
        {
            t += Time.deltaTime;
            yield return null;
        }

        foreach (Health h in explodeHits)
        {
            if (!h) continue;

            Rigidbody rb = h.GetComponent<Rigidbody>() ?? h.GetComponentInChildren<Rigidbody>();
            if (rb)
            {
                Vector3 dir = (h.transform.position - transform.position).normalized;
                rb.AddForce(dir * explodeForce, ForceMode.VelocityChange);
            }

            h.AddStatus(statusAffliction);
            Debug.Log("Exploded " + h.gameObject.name);
        }

        active = false;
        yield return new WaitForSeconds(aftershockTime);
        
        AnnounceExploded?.Invoke(gameObject);
        gameObject.SetActive(false);
    }
}