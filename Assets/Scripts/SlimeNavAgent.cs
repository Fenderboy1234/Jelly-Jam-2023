using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//this script has been poorly named and is now unofficially the SlimeEverythingController.

public class SlimeNavAgent : MonoBehaviour
{
    public GameObject playerStuff;
    public GameObject ballStuff;
    public GameObject foodStuff;
    public SphereCollider jellyInteractRadiusContainer;
    public SphereCollider playerInteractRadiusContainer;
    public SphereCollider theZone;



    private Vector3 randomPoint;
    private Vector3 goTo;
    private Vector3 startPoint;

    //other scripts
    private ThrowController throwController;
    private FoodChecker foodChecker;

    NavMeshAgent agent;

    bool fetchStart;
    bool carryingSomething;
    bool foodStart;
    bool playerInZone;
    bool beingHeld;

    float waitASec;

    enum State
    {
        MuckAbout,
        Fetch,
        Food,
        Pet,
        CommitDie
    }

    State slimeState;
    // Start is called before the first frame update
    void Start()
    {
        //just gettin some components and stuff
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        throwController = playerStuff.GetComponent<ThrowController>();
        foodChecker = jellyInteractRadiusContainer.GetComponent<FoodChecker>();
        //Set Initial destination for jelly
        randomPoint = RandomPointInBounds(theZone.bounds);
        //this is used later to prevent the jelly from just 
        //grabbing the ball out of the air when its tossed.
        waitASec = 1;

        startPoint = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForFetch();
        CheckForFood();
        IsPlayerInZone();
        SetState();
        if (!beingHeld)
        {
            /*
            if (this.transform.position.y <= -2)
            {
                this.transform.position = startPoint;
            }
            */
            agent.enabled = true;
            switch (slimeState)
            {
                case State.MuckAbout:
                    //MuckAbout();
                    break;
                case State.Fetch:
                    Fetch();
                    break;
                case State.Food:
                    Eat();
                    break;
                case State.Pet:
                    break;
                case State.CommitDie:
                    GoCommitDie();
                    break;
            }
        }
        else
        {
            agent.enabled = false;
        }
    }
    /*
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
    */
    public Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            this.transform.position.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    //methods associated with states
    void MuckAbout()
    {
        if (Vector3.Distance(this.transform.position, randomPoint) > 2)
        {
            agent.destination = randomPoint;
        }
        else
        {
            randomPoint = RandomPointInBounds(theZone.bounds);
        }

    }

    void Eat()
    {
        if (Vector3.Distance(this.transform.position, foodStuff.transform.position) > 1)
            agent.destination = foodStuff.transform.position;
        else
        {
            foodStuff.SetActive(false);
            foodStart = false;
        }
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
        if (!carryingSomething && waitASec <= 0 && Vector3.Distance(this.transform.position, ballStuff.transform.position) > 2)
        {
            agent.destination = ballStuff.transform.position;
        }
        else if (waitASec <= 0)
        {
            carryingSomething = true;
            ballStuff.transform.localPosition = this.transform.position + new Vector3 (0,0,1);
        }
        if (carryingSomething && Vector3.Distance(this.transform.position, playerStuff.transform.position) > 5)
        {
            agent.destination = playerInteractRadiusContainer.ClosestPoint(this.transform.position);    
        }
        else if(carryingSomething)
        {
            waitASec = 1;
            carryingSomething = false;
            fetchStart = false;
        }
        //If the player is still holding the ball I want to follow him, otherwise if he's released the ball and I don't have it I want to go grab it

    }

    void GoCommitDie()
    {
       // agent.destination = new Vector3(0, 0, 0);
    }
    //----------------------------------------------------------------------------------------Past this line thar be checking for things
    //I need this for when the player throws the ball and the fetch state isn't actually over
    void CheckForFetch()
    {
        if (throwController.holdingSomethingFetchable && theZone.bounds.Contains(this.transform.position) && theZone.bounds.Contains(playerStuff.transform.position))
        {
            fetchStart = true;
        }
    }
    void CheckForFood()
    {
        if (foodChecker.isFood == true)
        {
            foodStuff = foodChecker.maybeFood;
            foodStart = true;
        }
    }

    void IsPlayerInZone()
    {
        if (!theZone.bounds.Contains(this.transform.position))
            playerInZone = false;
        else
            playerInZone = true;

    }


    //-----------------------------------------------------------------------------------------------checking for things ends here
    void SetState()
    {

        if (foodStart && playerInZone)
            slimeState = State.Food;
        else if (fetchStart && !foodStart && playerInZone)
            slimeState = State.Fetch;
        else if (!playerInZone)
            slimeState = State.MuckAbout; //change this later
        else
            slimeState = State.MuckAbout;

    }
   
}
