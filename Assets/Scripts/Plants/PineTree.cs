using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineTree : Plant
{
    public GameObject[] birds;
    private GameObject bird;
    private float flyDelay;
    public float flyDelayRemaining;
    public bool canFly = true;

    public override void Start()
    {
        base.Start();
        // choose from our 2 lovely songbirds. This tree will only launch this color bird for the whole game.
        int birdIndex = Random.Range(0, (birds.Length));
        bird = birds[birdIndex];
        flyDelay = Random.Range(15, 25);
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
        flyDelayRemaining -= Time.deltaTime;
        if(flyDelayRemaining<=0 & currentGrowthStage == 2){
            canFly = true;
            flyDelayRemaining = flyDelay;
        }
        LaunchBird();
    }

    public void LaunchBird(){
        if(canFly){
            // launch in a random direction
            Vector3 tempVector = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
            GameObject current = Instantiate(bird, transform.position, Quaternion.identity);
            current.GetComponent<Songbird>().Launch(tempVector);
            canFly = false;
        }
    }
}
