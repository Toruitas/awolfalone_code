using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspenTree : Plant
{
    public bool hasEagle;
    // Start is called before the first frame update
    public override void Start()
    {
        // only 10% of aspen trees have an eagle
        if(Random.Range(0, 10)==1){
            hasEagle = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void FixedUpdate(){

        if(currentHealth < maxHealth){
            currentHealth += 0.001f;
        }
        if(currentHealth > 0.66 * maxHealth ){
            spriteR.sprite = sprites[2];
            currentGrowthStage = 2;
            GetComponent<BoxCollider2D>().enabled = true;
            if(hasEagle){
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }else if (currentHealth < 0.66 * maxHealth && currentHealth > 0.33 * maxHealth){
            spriteR.sprite = sprites[1];
            currentGrowthStage = 1;
            GetComponent<BoxCollider2D>().enabled = true;
            if(hasEagle){
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }else{
            spriteR.sprite = sprites[0];
            currentGrowthStage = 0;
            GetComponent<BoxCollider2D>().enabled = false;
            if(hasEagle){
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
