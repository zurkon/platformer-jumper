using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController : MonoBehaviour
{
    Collision coll;
    Rigidbody2D body;
    AnimationController anim;

    [Header("Stats")]
    public float speed = 10f;
    public float jumpSpeed = 40f;
    public float slideSpeed = 3f;
    public float wallSlideLerp = 1.5f;

    [Space]

    [Header("Booleans")]
    public bool canMove;
    public bool wallSlide;
    public bool sliding;
    public bool wallJumped;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<AnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);

        Move(dir);
        anim.SetMovement(x, body.velocity.y);

        if(xRaw != 0 && !wallSlide)
        {
            anim.Flip(xRaw);
        }

        if( Input.GetButtonDown("Jump"))
        {
            if (coll.onGround)
            {
                Jump(Vector2.up);
            }
            else if( coll.onWall)
            {
                WallJump();
            }
        }

        if( !coll.onGround && coll.onWall)
        {
            
            if( body.velocity.y > 0)
            {
                wallSlide = false;
                return;
            }


            if( body.velocity.x > 0 && coll.onRightWall)
            {
                if ( xRaw > 0 )
                {
                    wallSlide = true;
                } else
                {
                    wallSlide = false;
                }
            }

            if (body.velocity.x < 0 && coll.onLeftWall)
            {
                if (xRaw < 0)
                {
                    wallSlide = true;
                }
                else
                {
                    wallSlide = false;
                }
            }

            if (wallSlide)
            {
                if (!sliding)
                {
                    body.velocity = new Vector2(body.velocity.x, 0);
                    sliding = true;
                }
                body.velocity = new Vector2(body.velocity.x, -slideSpeed);
            }

        }

        if (coll.onGround)
        {
            wallSlide = false;
            sliding = false;
            wallJumped = false;
        }

    }

    private void Move(Vector2 dir)
    {
        if (!canMove)
        {
            return;
        }

        if (!wallJumped)
        {
            body.velocity = new Vector2(dir.x * speed, body.velocity.y);
        }
        else
        {
            body.velocity = Vector2.Lerp(body.velocity, (new Vector2(dir.x * speed, body.velocity.y)), wallSlideLerp * Time.deltaTime);
        }

    }

    private void Jump(Vector2 dir)
    {
        body.velocity = new Vector2(body.velocity.x, 0);
        //Vector2 jumpVelocityToAdd = new Vector2(body.velocity.x, jumpSpeed);
        body.AddForce(dir * jumpSpeed, ForceMode2D.Impulse);
    }

    private void WallJump()
    {
        StartCoroutine(DisableMovement(0.05f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;
        Jump( (Vector2.up + wallDir ) );

        wallJumped = true;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

}
