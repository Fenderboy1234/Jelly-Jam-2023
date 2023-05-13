using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeNavAgent : MonoBehaviour
{
    private Vector3 randomPoint;

    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        randomPoint = RandomNavmeshLocation(20f);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x != randomPoint.x && this.transform.position.z != randomPoint.z)
            agent.destination = randomPoint;
        else
            randomPoint = RandomNavmeshLocation(20);

        Debug.Log(randomPoint + " " + this.transform.position);
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        UnityEngine.AI.NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
