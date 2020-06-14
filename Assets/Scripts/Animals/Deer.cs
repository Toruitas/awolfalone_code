using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum DeerState{
    idle,
    eat,
    walk,
    flee,
    attack,
    dying
}

public class Deer : MonoBehaviour
{
    
    public Rigidbody2D myRigidbody;
    public Animator anim;
    private NavMeshAgent agent;
    public Transform wolf;
    
    [Header("State Machine")]
    public DeerState currentState;

    [Header("Eating Stuff")]
    public float eatRadius; // inside this radius, deer eats
    private float timeToHunger;
    private GameObject closestPlant = null;
    public float bitesPerTick = 0.02f;
    public float smellRange = 25.0f;
    public int layer = 8;

    [Header("Running away stuff")]
    private float reflexTime;
    public float detectRadius;
    public float speed;
    public float runSpeedMultiplier = 5.0f;
    public float deerStuckFleeingMaxTime = 10.0f;
    public Vector3 fleeTarget;
    public SpriteRenderer spriteR;
    public BoxCollider2D coll2D;
    public float flashSpeed = 0.0005f;

    public void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        wolf = GameObject.FindWithTag("Player").transform;
        speed = Random.Range(0.5f, 2.5f);
        agent.speed = speed;
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        currentState = DeerState.idle;

        // randomly generate hunger and reflex times
        // TODO re-factor random numbers to have a normal distribution (bell curve), rather than just this simple random
        timeToHunger = Random.Range(0.0f, 3.0f);
        reflexTime = Random.Range(0.0f, 0.2f);
        // InvokeRepeating("FindClosestPlant", timeToHunger, timeToHunger); 
        FindClosestPlant();
        
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public void FixedUpdate(){
        if(currentState != DeerState.dying){
            if(currentState != DeerState.flee){
                if(currentState == DeerState.walk){
                    StartEatingClosestPlant();
                }
                if(currentState == DeerState.eat){
                    Eating();
                }
                CheckFlee();
            }
            ChangeAnim();
        }
    }

    public void FindClosestPlant(){
        // altered from https://www.youtube.com/watch?v=YLE3v8bBnck

        closestPlant = null;  // clear

        // FIRST if there's food in the physics circle, go to that one.
        // Approach from https://forum.unity.com/threads/find-object-with-tag-in-range.190938/
        SniffOutFood();

        // SECOND if there aren't any bits of food in the physics circle, go through all the plants to find one nearby.
        if(!closestPlant){
            Debug.Log("No nearby food, searching for closest.");
            SniffOutFoodFar();
        }

        agent.speed = speed;
        agent.SetDestination(closestPlant.transform.position);
        DebugDrawPath(agent.path.corners);
        currentState = DeerState.walk;
        anim.SetBool("moving", true);


        // ChangeState(DeerState.walk);

        // Debug.Log("Deer position " + transform.position);
        // Debug.Log("Closest plant position "+ closestPlant.transform.position);
        Debug.DrawLine(transform.position, closestPlant.transform.position);
    }

    void SniffOutFoodFar(){
        float distanceToClosestPlant = Mathf.Infinity;
        GameObject[] allPlants = GameObject.FindGameObjectsWithTag("Plant");

        foreach (GameObject currentPlant in allPlants){
            float distanceToPlant = Vector3.Distance(currentPlant.transform.position, transform.position);
            if(distanceToPlant < distanceToClosestPlant && currentPlant.GetComponent<Plant>().currentGrowthStage >= 1){
                distanceToClosestPlant = distanceToPlant;
                closestPlant = currentPlant;
            }
        }        
    }

    public void StartEatingClosestPlant(){
        // And if the deer has arrived at its destination, it starts eating.
        // if(Vector3.Distance(agent.path.corners[0],transform.position) < eatRadius){
        if(agent.path.corners.Length >= 1){
            if(Vector3.Distance(transform.position, closestPlant.transform.position) < eatRadius){
                agent.ResetPath();
                currentState = DeerState.eat;
                anim.SetBool("moving", false);
            }
        }
        
    }

    public void Eating(){
        // if the plant is within range, 
        if(Vector3.Distance(transform.position, closestPlant.transform.position) <= eatRadius){
            // if plant has positive health
            if(closestPlant.GetComponent<Plant>().currentHealth >= 0){  // plants have 0, 1, 2 stages. Deer don't like to eat 0.
                // subtract the plant's health
                closestPlant.GetComponent<Plant>().currentHealth -= bitesPerTick;
            }else{
                // if plant is below 0, choose a new target plant and set path, after being idle for timeToHunger
                StartCoroutine(GetHungryCoroutine());
            }
        }else{
            // If deer is eating but not in range, it's bugged! But set it to find a new plant to eat anyways.
            FindClosestPlant();
        }
    }

    private IEnumerator GetHungryCoroutine(){
        currentState = DeerState.idle;
        yield return new WaitForSeconds(timeToHunger);
        FindClosestPlant();
    }

    public void ChangeAnim(){
        if(agent.path.corners.Length>1){
            Vector3 direction = agent.path.corners[1]-agent.path.corners[0];
            direction = direction.normalized;
            anim.SetFloat("moveX", direction.x);
            anim.SetFloat("moveY", direction.y);
        }
    }

    // TODO add a flee script
    // Add a reflex time, so the wolf can sometimes actually kill a deer.
    public void CheckFlee(){
        // if player in detectRadius
        if(Vector3.Distance(transform.position, wolf.transform.position) <= detectRadius){
            Vector3 moveDirection = transform.position - wolf.transform.position;
            StartCoroutine(FleeCoroutine(moveDirection));
        }

        if(currentState == DeerState.flee){
            // check if has fled far enough
            if(Vector3.Distance(transform.position, fleeTarget) <= eatRadius){
                StopFleeing();
            }
        }
        
        // character.Move(moveDirection.normalized * speed * Time.deltaTime);
    }

    private IEnumerator FleeCoroutine(Vector3 moveDirection){
        yield return new WaitForSeconds(reflexTime);
        currentState = DeerState.flee;
        // flee distance
        float fleeDistance = Random.Range(1.0f, 7.0f);
        agent.speed = speed * runSpeedMultiplier;
        agent.SetDestination(moveDirection * fleeDistance);
        anim.SetBool("moving", true);
        anim.SetBool("running", true);

        // now set a reasonable time-out, just in case the deer gets stuck running somewhere it can't get
        yield return new WaitForSeconds(deerStuckFleeingMaxTime);
        StopFleeing();
    }

    private void StopFleeing(){
        anim.SetBool("running", false);
        anim.SetBool("moving", false);
        currentState = DeerState.idle;
        FindClosestPlant();
    }
    
    // This method from the navmesh2d docs
    public static void DebugDrawPath(Vector3[] corners)
    {
        if (corners.Length < 2) { return; }
        for (int i = 0; i < corners.Length - 1; i++)
        {
            Debug.DrawLine(corners[i], corners[i + 1], Color.blue);
        }
        Debug.DrawLine(corners[0], corners[1], Color.red);
    }


    public void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player") && !other.isTrigger){
            currentState = DeerState.dying;
            agent.ResetPath();
            agent.enabled = false;
            // flash the sprite a couple times and play the death sound
            StartCoroutine(FlashDie(flashSpeed));
        }
    }

    IEnumerator FlashDie(float x) {
        // https://answers.unity.com/questions/1190102/make-a-2d-sprite-flash.html
        // coll2D.enabled = false;
        for (int i = 0; i < 3; i++) {
            spriteR.enabled = false;
            yield return new WaitForSeconds(x);
            spriteR.enabled = true;
            yield return new WaitForSeconds(x);
        }
        Destroy(gameObject);
    }

    void SniffOutFood()
    {
        // Approach from https://forum.unity.com/threads/find-object-with-tag-in-range.190938/
        // layer = LayerMask.NameToLayer("Food");
        Collider2D[] foodinrange = Sniff(transform.position, smellRange, layer);
        float distanceToClosestPlant = Mathf.Infinity;
        // Debug.Log(foodinrange.Length);

        foreach (Collider2D currentPlantColl in foodinrange){
            GameObject currentPlant = currentPlantColl.gameObject;
            if (currentPlant.tag == "Plant"){
                float distanceToPlant = Vector3.Distance(currentPlant.transform.position, transform.position);
                if(distanceToPlant < distanceToClosestPlant && currentPlant.GetComponent<Plant>().currentGrowthStage >= 1){
                    distanceToClosestPlant = distanceToPlant;
                    closestPlant = currentPlant;
                }
            }
        }

        
    }


    Collider2D[] Sniff(Vector2 nosePosition, float range, int layer)
    {
        // Approach from https://forum.unity.com/threads/find-object-with-tag-in-range.190938/
        int layerMask = 1 << layer;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(nosePosition, range, layerMask);
        // Debug.Log(hitColliders.Length);
        return hitColliders;
    }
}
