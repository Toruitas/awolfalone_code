using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    // Thanks Brackeys
    // https://www.youtube.com/watch?v=Vrld13ypX_I
    [System.Serializable]
    public class Wave{
        public string name;
        public int count;
        public float rate;
    }

    public enum SpawnState {
        spawning,
        waiting,
        counting
    }

    public Wave[] waves;
    private int nextWave = 0;
    public float timeBetweenWaves = 5.0f;
    private float waveCountdown;
    public SpawnState currentState = SpawnState.counting;
    private float searchCountdown = 1.0f;
    public GameObject[] spawnPoints;
    public GameObject[] gameObjsToSpawn;
    public int maxDeer = 75;
    public int deerCount;
    public int currentWave = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(spawnPoints.Length == 0){
            Debug.LogError("No spawn points referenced");
        }
        waveCountdown = timeBetweenWaves;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if(currentState == SpawnState.waiting){
        //     // check if player has killed all enemies
        //     if(!EnemyIsAlive()){
        //         // begin a new round
        //         WaveCompleted();
        //     }else{
        //         return;
        //     }
        // }


        if(waveCountdown <= 0){
            if(currentState != SpawnState.spawning){
                // start spawn
                deerCount = GameObject.FindGameObjectsWithTag("Deer").Length;
                if(deerCount < maxDeer){
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
        }else{
            waveCountdown -= Time.deltaTime;
        }
    }

    IEnumerator SpawnWave( Wave _wave){
        currentWave ++;

        currentState = SpawnState.spawning;
        // first increase the difficulty!
        _wave.count += 15;

        //spawn
        for (int i=0; i<_wave.count; i++){
            // loop through count # of times to spawn the number of enemies we want to spawn from this spot
            if(i%2==0){
                SpawnDeer(gameObjsToSpawn[0]);  // male deer
            }else{
                SpawnDeer(gameObjsToSpawn[1]);  // female deer
            }
            yield return new WaitForSeconds(1.0f/_wave.rate);  // wait a bit before spawning next part of this wave
        }

        // currentState = SpawnState.waiting; // wait for last round's baddies to be killed. Comment this out later as do not need.
        // Instead just call wavecompleted directly
        WaveCompleted();
        yield break; // return nothing
    }

    void SpawnDeer(GameObject deerToSpawn){
        Debug.Log("Spawning enemy: " + deerToSpawn.name);
        
        GameObject _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(deerToSpawn, _sp.transform.position, _sp.transform.rotation);
    }

    bool EnemyIsAlive(){
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f){
            searchCountdown = 1.0f;
            if(GameObject.FindGameObjectsWithTag("Deer").Length == 0){
                return false;
            }
        }
        return true;
    }

    void WaveCompleted(){
        currentState = SpawnState.counting;
        waveCountdown = timeBetweenWaves;
        if(nextWave +1 > waves.Length-1){
            nextWave = 0;
            Debug.Log("All Waves Complete! Looping...");
            // Now add difficulty by adding more deer.

        }else{
            nextWave++;
        }
    }


    void WinOrLose(){

    }

    
}
