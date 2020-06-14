using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeaverState{
    idle,
    swim
}

public class Beaver : MonoBehaviour
{
    public Transform[] path;
    public int currentPoint;
    public BeaverState currentState;
    public Transform currentGoal;
    public float roundingDistance;
    public Rigidbody2D myRigidbody;
    public Animator anim;
    public float moveSpeed;
    private Vector2 startingPlacement;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        currentState = BeaverState.swim;
        anim = GetComponent<Animator>();
        anim.SetBool("moving", true);
    }

    void FixedUpdate()
    {
        CheckDistance();
    }

    public void CheckDistance(){
        // lifted from the Mister Taft Creates patrol log script
        if(Vector3.Distance(transform.position, path[currentPoint].position) > roundingDistance){
            // move towards current point in path
            Vector3 temp = Vector3.MoveTowards(transform.position, path[currentPoint].position, moveSpeed * Time.deltaTime);
            ChangeAnim(temp-transform.position);  
            // use the rigidbody to move
            myRigidbody.MovePosition(temp);
        }else{
            ChangeGoal();
        }
    }

    private void ChangeGoal(){
        if(currentPoint == path.Length-1){
            currentPoint = 0;
            currentGoal = path[0];
        }else{
            currentPoint++;
            currentGoal = path[currentPoint];
        }
    }

    public void ChangeAnim(Vector2 direction){
        direction = direction.normalized;
        anim.SetFloat("moveX", direction.x);
        anim.SetFloat("moveY", direction.y);
    }
}
