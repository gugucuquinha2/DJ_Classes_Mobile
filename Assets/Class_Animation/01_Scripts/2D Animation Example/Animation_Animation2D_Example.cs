using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Animation2D_Example : MonoBehaviour
{
    private Animator animator;

    // input variables
    private float screenFirstHalf;
    private float hor;

    // default scale of our sprite
    public Vector3 spriteScale = new Vector3(10, 10, 1);

    // movement variables
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        screenFirstHalf = Screen.width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

        // manage anything related with inputs
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Movement(touch);
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
}
