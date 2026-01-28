using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    protected Vector3 destination;
    protected UnityEngine.AI.NavMeshAgent agent;

    protected void agentSetup() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    protected Vector3 getRandomPoint(float radius) {
        return getRandomNavPoint(transform.position, radius);
    }
    protected Vector3 getRandomNavPoint(Vector3 point, float radius) {
        for (int i = 0; i < 16; ++i)
        {
            Vector3 randomPoint = point + radius * UnityEngine.Random.insideUnitSphere;
            randomPoint.y = transform.position.y - 0.3f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                if (Mathf.Abs(hit.position.y - transform.position.y) < 1.2f 
                    && checkDestinationReachable(hit.position)) {
                    return hit.position;
                }
            }
        }
        return transform.position;
    }
    protected bool checkDestinationReachable(Vector3 destination) {
        var path = new NavMeshPath();
        agent.CalculatePath(destination, path);
        switch (path.status)
        {
            case NavMeshPathStatus.PathComplete:
                return true;
            default:
                return false;
        }
    }
    protected bool hasReachedDest() {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                    new Vector2(destination.x, destination.z)) < 1;
    }

    protected float TimerF(float val)
    {
        if (val > 0)
        {
            val -= Time.deltaTime;
            if (val <= 0) val = 0;
        }
        return val;
    }
}
