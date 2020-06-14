using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{

    public int currentGrowthStage = 2; //  0,1,2
    public int maxGrowthStage = 2;
    public float currentHealth = 10.0f;
    public float maxHealth = 10.0f;
    // Start is called before the first frame update
    public SpriteRenderer spriteR;
    public Sprite[] sprites;
    
    public virtual void Start()
    {
        // currentGrowthStage = Random.Range(5.0f, 10.0f);
        spriteR = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void FixedUpdate(){
        // could probably make this unneccesarily complex and robust to any number of sprites and stages, as it is basically identical in BeaverDam.cs, but 1 stage less.
        // Save optimizing for a later day.
        if(currentHealth < maxHealth){
            currentHealth += 0.00001f;
        }
        if(currentHealth > 0.66 * maxHealth ){
            spriteR.sprite = sprites[2];
            currentGrowthStage = 2;
            GetComponent<BoxCollider2D>().enabled = true;
        }else if (currentHealth < 0.66 * maxHealth && currentHealth > 0.33 * maxHealth){
            spriteR.sprite = sprites[1];
            currentGrowthStage = 1;
            GetComponent<BoxCollider2D>().enabled = true;
        }else{
            spriteR.sprite = sprites[0];
            currentGrowthStage = 0;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // if below a certain threshold disable the box collider 2d?.


}
