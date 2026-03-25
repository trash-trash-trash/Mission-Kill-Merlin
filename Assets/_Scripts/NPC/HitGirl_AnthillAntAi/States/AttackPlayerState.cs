using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayerState : NPCAnthillStateBase
{
    [Header("Attack Settings")]
    [SerializeField] private int shotsPerBurst = 3;
    [SerializeField] private float timeBetweenShots = 0.25f;
    [SerializeField] private float windUpTime = 0.4f;
    [SerializeField] private float burstCooldown = 1.0f;

    [Header("Strafing")]
    [SerializeField] private float strafeChance = 0.5f;
    [SerializeField] private float strafeDistance = 4f;

    private Transform player;
    private EnergyBallPool pool;
    private Coroutine attackRoutine;

    public override void Enter()
    {
        base.Enter();

        player = scenarioBrain.playerTransform;
        pool = FindObjectOfType<EnergyBallPool>(); // fine for now

        scenarioBrain.navMeshAgent.isStopped = true;

        scenarioBrain.npcHeadLook.FlipLookingAt(scenarioBrain.playerTransform.position, true);
        attackRoutine = StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            scenarioBrain.debugText.SetText("ATTACKING PLAYER!!!");
            // --- WIND UP ---
            yield return new WaitForSeconds(windUpTime);

            // --- BURST FIRE ---
            for (int i = 0; i < shotsPerBurst; i++)
            {
                if (player != null && pool != null)
                {
                    pool.ShootEnergyBall(scenarioBrain.transform,scenarioBrain.shootPoint, player);
                }

                yield return new WaitForSeconds(timeBetweenShots);
            }
            
            scenarioBrain.debugText.SetText("COOLDOWN");

            // --- COOLDOWN ---
            yield return new WaitForSeconds(burstCooldown);
            
            float dist = Vector3.Distance(transform.position, player.position);

            if (dist > scenarioBrain.minimumAttackRange)
            {
                scenarioBrain.inRangeToAttack = false;
                break;
            }

            // --- DECISION PHASE ---
            bool shouldStrafe = Random.value < strafeChance;

            if (shouldStrafe)
            {
                yield return StartCoroutine(StrafeRoutine());
            }
        }
    }

    private IEnumerator StrafeRoutine()
    {
        scenarioBrain.debugText.SetText("STRAFING");

        scenarioBrain.navMeshAgent.isStopped = false;

        Vector3 toPlayer = (player.position - transform.position).normalized;

        // Get perpendicular direction (left/right)
        Vector3 strafeDir = Vector3.Cross(Vector3.up, toPlayer).normalized;
        if (Random.value > 0.5f)
            strafeDir *= -1;

        Vector3 targetPos = transform.position + strafeDir * strafeDistance;

        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, strafeDistance, NavMesh.AllAreas))
        {
            scenarioBrain.navMeshAgent.SetDestination(hit.position);
        }

        // Move while looking at player
        while (scenarioBrain.navMeshAgent.pathPending ||
               scenarioBrain.navMeshAgent.remainingDistance > 0.5f)
        {
            scenarioBrain.npcHeadLook.FlipLookingAt(player.position, true);
            yield return null;
        }

        scenarioBrain.navMeshAgent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();

        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        if (scenarioBrain.navMeshAgent.isOnNavMesh)
            scenarioBrain.navMeshAgent.isStopped = false;
    }
}