using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverDam : Plant
{

    // public GameObject beaver;

    // public void Start(){
    //     base.Start();
    //     beaver = gameObject.GetComponent<GameObject>();
    // }
    public override void FixedUpdate(){
        // could probably make this unneccesarily complex and robust to any number of sprites and stages. Optimizing for a later day.
        // base.Start();
        if(currentHealth < maxHealth){
            currentHealth += 0.001f;
        }
        if(currentHealth > 0.5 * maxHealth ){
            spriteR.sprite = sprites[1];
            currentGrowthStage = 1;
            GetComponent<BoxCollider2D>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }else{
            spriteR.sprite = sprites[0];
            currentGrowthStage = 0;
            GetComponent<BoxCollider2D>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}
