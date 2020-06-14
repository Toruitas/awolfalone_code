using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState{
    walk,
    run,
    attack,
    howl,
    idle
}

public class Player : MonoBehaviour
{

    public float speed = 3f;
    public float runMultiplier = 1.5f;
    public float runtimeRunMult;
    private Rigidbody2D playerRigidBody;
    private Vector3 movementChange;
    private Animator animator;
    public PlayerState currentState; 
    private bool running = true;

    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.idle;
        playerRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        // transform.position = startingPosition.initialValue;
        runtimeRunMult = runMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        // Much of the project code comes from the Mister Taft Creates "Make a Game like Zelda using Unity and C#" Youtube series.
        movementChange = Vector3.zero; 
        movementChange.x = Input.GetAxisRaw("Horizontal");
        movementChange.y = Input.GetAxisRaw("Vertical");

        // GetButton can be used on a held button. GetButtonDown only on the initial down.
        if(Input.GetButtonDown("Run")){
            running = false;
        }else if (Input.GetButtonUp("Run")){
            running = true;
        }      
    }

    void FixedUpdate(){
        if (currentState == PlayerState.walk || currentState == PlayerState.idle || currentState == PlayerState.run){
            UpdateAnimationAndMove();
        }
    }

    void UpdateAnimationAndMove(){
        if(movementChange != Vector3.zero){
            // if run button is pressed:
            if(running){
                currentState = PlayerState.run;
            }else{
                currentState = PlayerState.walk;
            }
            animator.SetFloat("moveX", movementChange.x);
            animator.SetFloat("moveY", movementChange.y);
            animator.SetBool("running", running);
            animator.SetBool("moving", true);
            MoveCharacter(running);
        }else{
            animator.SetBool("moving", false);
            // don't reset the animator floats so the idle animation stays where it is
            currentState = PlayerState.idle;
        }
    }

    void MoveCharacter(bool running){
        runtimeRunMult = runMultiplier;
        if(!running){
             runtimeRunMult =  1.0f;  // no run multiplier
        }
        movementChange.Normalize();
        playerRigidBody.MovePosition(
            transform.position + movementChange * speed * runtimeRunMult * Time.fixedDeltaTime  // this moves it a very very small amount
        );
    }
}
