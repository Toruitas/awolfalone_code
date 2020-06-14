using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Plant
{

    public override void FixedUpdate(){
        // could probably make this unneccesarily complex and robust to any number of sprites and stages, as it is basically identical in BeaverDam.cs, but 1 stage less.
        // Save optimizing for a later day.
        if(currentHealth < maxHealth){
            currentHealth += 0.001f;
        }
        if(currentHealth > 0.66 * maxHealth ){
            spriteR.sprite = sprites[2];
            currentGrowthStage = 2;
        }else if (currentHealth < 0.66 * maxHealth && currentHealth > 0.33 * maxHealth){
            spriteR.sprite = sprites[1];
            currentGrowthStage = 1;
        }else{
            spriteR.sprite = sprites[0];
            currentGrowthStage = 0;
        }
    }
}
