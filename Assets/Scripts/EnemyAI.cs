using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    private NavMeshSurface _surface;

    [Header("Settings")]
    public float wanderRadius = 10.0f;
    public float minIdleTime = 1.0f;
    public float maxIdleTime = 3.0f;

    private float _idleTimer;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _surface = FindAnyObjectByType<NavMeshSurface>();
    }


    private void Start()
    {
        SetNewDestination();
    }

    private void Update()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _idleTimer -= Time.deltaTime;

            if (_idleTimer <= 0f)
                SetNewDestination();
        }
    }

    private void SetNewDestination()
    {
        Vector3 newPos = GetRandomPointOnNavMesh(transform.position, wanderRadius);
        _agent.SetDestination(newPos);

        _idleTimer = Random.Range(minIdleTime, maxIdleTime);
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 origin, float distance)
    {
        for (int i = 0; i < 40; i++)
        {
            Vector3 random = origin + Random.insideUnitSphere * distance;

            if (_surface != null)
            {
                var b = _surface.navMeshData.sourceBounds;

                random = new Vector3(
                    Mathf.Clamp(random.x, b.min.x, b.max.x),
                    random.y,
                    Mathf.Clamp(random.z, b.min.z, b.max.z)
                );
            }

            if (NavMesh.SamplePosition(random, out NavMeshHit hit, 10f, NavMesh.AllAreas))
                return hit.position;
        }

        if (NavMesh.SamplePosition(origin, out NavMeshHit safeHit, 10f, NavMesh.AllAreas))
            return safeHit.position;

        return origin;
    }
}
