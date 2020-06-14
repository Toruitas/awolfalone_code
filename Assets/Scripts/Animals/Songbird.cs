using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Songbird : MonoBehaviour
{
    [Header("Movement Stuff")]
    public float moveSpeed;
    public Vector2 directionToMove;

    [Header("Lifetime")]
    public float lifetime;
    private float lifetimeRemaining;
    public Rigidbody2D myRigidbody;
    public Animator anim;

    void Start()
    {
        lifetime = lifetime * Random.Range(0.75f,2.0f);
        lifetimeRemaining = lifetime;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        lifetimeRemaining -= Time.deltaTime;
        if(lifetimeRemaining<=0){
            Destroy(this.gameObject);
        }
    }

    public void Launch(Vector2 initialVelocity){
        // initial velocity is a unit vector for direction
        initialVelocity.Normalize();
        ChangeAnim(initialVelocity);
        myRigidbody.velocity = initialVelocity * moveSpeed;
    }

    public void ChangeAnim(Vector3 direction){
        // direction = direction.normalized;
        anim.SetFloat("moveX", direction.x);
        anim.SetFloat("moveY", direction.y);
    }
}
