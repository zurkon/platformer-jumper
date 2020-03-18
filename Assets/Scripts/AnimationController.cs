using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;
    private PlatformerController controller;
    private Collision coll;
    private SpriteRenderer sr;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<PlatformerController>();
        coll = GetComponent<Collision>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("onGround", coll.onGround);
        anim.SetBool("canMove", controller.canMove);
        anim.SetBool("wallSlide", controller.wallSlide);
    }

    public void SetMovement(float x,  float yVel)
    {
        anim.SetFloat("HorizontalAxis", Mathf.Abs(x));
        anim.SetFloat("VerticalVelocity",yVel);
    }

    public void Flip(float x)
    {
        transform.localScale = new Vector2( x, transform.localScale.y);
    }

}
