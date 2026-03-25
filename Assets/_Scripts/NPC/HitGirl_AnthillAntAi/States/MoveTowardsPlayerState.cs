using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsPlayerState : NPCAnthillStateBase
{
    [SerializeField] private float randomRadius = 2.5f;
    [SerializeField] private float repathDelay = 0.5f;
    [SerializeField] private float currentDist = 0f;
    private Coroutine moveRoutine;

    public override void Enter()
    {
        base.Enter();
        scenarioBrain.debugText.SetText("MOVING TO PLAYER");
        scenarioBrain.navMeshAgent.isStopped = false;
        scenarioBrain.npcHeadLook.FlipLookingAt(scenarioBrain.playerTransform.position, true);
        moveRoutine = StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            if (scenarioBrain.playerTransform == null)
            {
                scenarioBrain.alert = false;
                yield break;
            }

            float dist = Vector3.Distance(transform.position, scenarioBrain.playerTransform.position);

            currentDist = dist;
            // If we're close enough → stop and let FSM transition to attack
            if (dist <= scenarioBrain.minimumAttackRange)
            {
                scenarioBrain.navMeshAgent.isStopped = true;
                scenarioBrain.inRangeToAttack = true;
                yield break;
            }

            // Pick a random point near player
            Vector3 randomOffset = Random.insideUnitSphere * randomRadius;
            randomOffset.y = 0;

            Vector3 targetPos = scenarioBrain.playerTransform.position + randomOffset;

            if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, randomRadius, NavMesh.AllAreas))
            {
                scenarioBrain.navMeshAgent.SetDestination(hit.position);
            }

            // Look at player while moving

            yield return new WaitForSeconds(repathDelay);
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        if (scenarioBrain.navMeshAgent.isOnNavMesh)
            scenarioBrain.navMeshAgent.isStopped = false;
    }
}