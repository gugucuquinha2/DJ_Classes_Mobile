using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Animation2D_Jump_Example : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    // input variables
    private float screenFirstHalf;
    private float hor;
    private bool canJump = true;

    // default scale of our sprite
    public Vector3 spriteScale = new Vector3(10, 10, 1);

    // movement and jump variables
    private float maxDistanceToGround = 0;
    private float groundYPos = -2.385f;
    public float speed = 5;
    public float forceMultiplier = 3;

    // swipe variables
    private float touchTimeStart;
    private float touchTimeEnd;
    private float timeDelta;
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        screenFirstHalf = Screen.width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // continuous check to see how close the player is to the ground
        CheckDistanceToGround();

        // manage anything related with inputs
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            Movement(touch);
            Jump(touch);
        }
        else if (hor != 0)
        {
            hor = 0;
        }

        // since it is a 2D sprite, we only care about horizontal movement (left/right)
        Vector3 translation = new Vector3(hor, 0, 0);
        transform.position += translation * (Time.deltaTime * speed);

        // set the float of our animator parameter
        // the method "Mathf.Abs()" is used to return the absolute value of a variable, meaning that it will always return the positive value of a number:
        // - our "Run" parameter expects that values lower than "0.01" mean our character is idle
        // - but in fact our movement values range from -1 to 1, while 0 means we're not moving
        // - so to counter the fact that -1 is considered lower than "0.01" (our animator considers that to be idle), 
        // we need to make sure that it assumes a positive value for movement that returns negative values - SO WE USE "Mathf.Abs()"
        animator.SetFloat("Run", Mathf.Abs(hor));
    }

    private void Movement(Touch _touch)
    {
        // only consider this method if we're not jumping
        if (!canJump)
        {
            return;
        }

        // ... grab the first one and check which half of the screen is being touched
        if (_touch.position.x < screenFirstHalf)
        {
            // Mathf.Lerp is a mathematical function that interpolates between 2 values, with a given speed
            // in this case our goal is to smoothly get the "hor" to -1 or 1 (simulating the Input.GetAxis("Horizontal") on PC)
            hor = Mathf.Lerp(hor, -1, Time.deltaTime * 10);

            // ...we flip our sprite left
            transform.localScale = new Vector3(-spriteScale.x, spriteScale.y, spriteScale.z);
        }
        else
        {
            hor = Mathf.Lerp(hor, 1, Time.deltaTime * 10);

            // ...we flip our sprite right
            transform.localScale = new Vector3(spriteScale.x, spriteScale.y, spriteScale.z);
        }
    }
    
    private void Jump(Touch _touch)
    {
        // make sure a new jump is only possible if we're on the ground
        if (transform.position.y <= groundYPos)
        {
            canJump = true;
        } // if not we exit this method
        else
        {
            return;
        }

        // DETECT THE SWIPE
        if (_touch.phase == TouchPhase.Began)
        {
            // record its time and position
            touchTimeStart = Time.time;
            startPos = _touch.position;
        }
        else if (_touch.phase == TouchPhase.Ended)
        {
            // record the time and position
            touchTimeEnd = Time.time;
            endPos = _touch.position;

            // calculate the swipe direction and how fast it is
            timeDelta = touchTimeEnd - touchTimeStart;
            direction = endPos - startPos;

            if (direction.x > 0)
            {
                // ...we flip our sprite right
                transform.localScale = new Vector3(spriteScale.x, spriteScale.y, spriteScale.z);
            }
            else
            {
                // ...we flip our sprite left
                transform.localScale = new Vector3(-spriteScale.x, spriteScale.y, spriteScale.z);
            }

            // only consider swipes that are going significantly upwards
            if (direction.normalized.y > 0.4f)
            {
                // make sure we can't jump while on the air and reset variables related to jumping because of the new jump
                canJump = false;
                maxDistanceToGround = 0;

                // get the force of the jump based on how big and fast is the swipe
                float dis = Vector2.Distance(endPos, startPos);
                float jumpForce = Mathf.Clamp(((dis / 3000) / timeDelta) * forceMultiplier, 7, 12);

                // Apply the jump impulse (force) on our character
                rb.AddForce(direction.normalized * jumpForce, ForceMode2D.Impulse);

                // call the jump animation
                animator.SetTrigger("Jump");
            }
        }
    }

    private void CheckDistanceToGround()
    {
        if (!canJump)
        {
            // check if the distance of our character to the ground is the maximum it has ever reached
            if (Mathf.Abs(transform.position.y - groundYPos) >= maxDistanceToGround)
            {
                //... by saving it into a variable and continuously comparing it to the current distance
                maxDistanceToGround = transform.position.y - groundYPos;
            }
            // in the frame that that distance is not bigger, that means we're falling to the ground
            else
            {
                // set the fall animation
                animator.SetBool("Fall", true);
            }

            // once we reach the ground again, stop the falling animation
            if (transform.position.y <= groundYPos)
            {
                animator.SetBool("Fall", false);
            }
        }        
    }
}
