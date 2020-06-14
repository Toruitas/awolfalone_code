using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HealthbarController : MonoBehaviour
{
    [Header("Ecosystem Health")]
    public GameObject[] trees;
    public float healthOfEnvironment = 0.0f;
    public float totalHealth = 0.0f;
    public Image healthbar;
    public float totalMaxHealth;

    [Header("Win Timer")]
    public float levelTimer = 0.0f;
    public float timerWinInSeconds = 600.0f;  // 15 minutes seems reasonable

    // Start is called before the first frame update
    void Start()
    {
        trees = GameObject.FindGameObjectsWithTag("Plant");
        

        InvokeRepeating("changeHealth", 1.0f, 1.0f);
        
        foreach(GameObject tree in trees){
            totalMaxHealth += tree.GetComponent<Plant>().maxGrowthStage;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        levelTimer += Time.deltaTime;
        // Debug.Log("total environmental health" + healthOfEnvironment);
    }

    public void changeHealth(){
        
        totalHealth = 0.0f;
        // totalMaxHealth = 0.0f;
        foreach(GameObject tree in trees){
            // get health value
            totalHealth += tree.GetComponent<Plant>().currentGrowthStage;  // we use growth stages for a more noticable bar movement.
            // totalMaxHealth += tree.GetComponent<Plant>().maxGrowthStage;  // we use growth stages for a more noticable bar movement.
        }
        healthOfEnvironment = totalHealth/totalMaxHealth;
        healthbar.fillAmount = healthOfEnvironment;

        // Game over
        if (healthOfEnvironment < 0.005f){
            SceneManager.LoadScene("ClosingPlea", LoadSceneMode.Single);
        }

        if(levelTimer > timerWinInSeconds){
            SceneManager.LoadScene("WinningPlea", LoadSceneMode.Single);
        }
    }


}
