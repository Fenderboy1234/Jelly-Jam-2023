using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//this script has been poorly named and is now unofficially the SlimeEverythingController.

public class SlimeNavAgent : MonoBehaviour
{
    public GameObject playerStuff;
    public GameObject ballStuff;

    private Vector3 randomPoint;

    private ThrowController throwController;

    NavMeshAgent agent;

    enum State
    {
        MuckAbout,
        Fetch,
        Food,
        Pet
    }

    State slimeState;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        throwController = playerStuff.GetComponent<ThrowController>();
        randomPoint = RandomNavmeshLocation(20f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (slimeState)
        {
            case State.MuckAbout:
                MuckAbout();
                break;
            case State.Fetch:
                Fetch();
                break;
            case State.Food:
                break;
            case State.Pet:
                break;
        }

        //I need to know whether the player
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

    void MuckAbout()
    {
        if (this.transform.position.x != randomPoint.x && this.transform.position.z != randomPoint.z)
            agent.destination = randomPoint;
        else
            randomPoint = RandomNavmeshLocation(20);

    }

    void Fetch()
    {
        ballStuff = throwController.interactableObject;
        //If the player is still holding the ball I want to follow him, otherwise if he's released the ball and I don't have it I want to go grab it

    }

    void setState()
    {

        if (throwController.interactableObject.tag == "Fetchable")
                slimeState = State.Fetch;
        else
            slimeState = State.MuckAbout;

    }
}
