using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//this script has been poorly named and is now unofficially the SlimeEverythingController.

public class SlimeNavAgent : MonoBehaviour
{
    public GameObject playerStuff;
    public SphereCollider playerInteractRadiusContainer;
    public GameObject ballStuff;
    public SphereCollider jellyInteractRadiusContainer;

    private Vector3 randomPoint;
    Vector3 goTo;

    private ThrowController throwController;

    NavMeshAgent agent;

    bool fetchStart;
    bool carryingSomething;

    float waitASec;

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
        waitASec = 1;
    }

    // Update is called once per frame
    void Update()
    {
        setState();
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
        if (waitASec > 0 &&throwController.holdingSomethingFetchable)
        {
            agent.destination = playerInteractRadiusContainer.ClosestPoint(this.transform.position);
        }
        else
        {
            waitASec -= Time.deltaTime;
        }
        if (!carryingSomething && waitASec <= 0 && Vector3.Distance(this.transform.position, ballStuff.transform.position) > 1)
        {
            agent.destination = ballStuff.transform.position;
        }
        else if (waitASec <= 0)
        {
            carryingSomething = true;
            ballStuff.transform.position = this.transform.position + new Vector3 (0,1,0);
        }
        if (carryingSomething && Vector3.Distance(this.transform.position, playerStuff.transform.position) > 5)
        {
            agent.destination = playerInteractRadiusContainer.ClosestPoint(this.transform.position);
            Debug.Log(agent.destination);
        }
        else if(carryingSomething)
        {
            Debug.Log("Ended");
            waitASec = 1;
            carryingSomething = false;
            fetchStart = false;
        }
        //If the player is still holding the ball I want to follow him, otherwise if he's released the ball and I don't have it I want to go grab it

    }

    void setState()
    {
        if (throwController.holdingSomethingFetchable)
        {
            Debug.Log("Started");
            fetchStart = true;
        }
        if (fetchStart)//throwController.interactableObject.tag == "Fetchable")
                slimeState = State.Fetch;
        else
            slimeState = State.MuckAbout;

    }
}
