using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInput_Exercise_2 : MonoBehaviour
{
    // movement variables
    public float speed;
    private Vector3 translation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // MOVEMENT
        MoveCube();
    }

    private void MoveCube()
    {
        // get the Axis value of the virtual buttons
        // returns a range of values from [-1, 1], depending of the pressed key
        // EXAMPLE for the "Horizontal" virtual button:
        // - if "left arrow" (the negative button) is pressed, value will gradually move to -1
        // - if "right arrow" (the positive button) is pressed, value will gradually move to 1
        // - if no button is pressed, value will return to 0
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        // if there's touches in the screen we want to simulate the above behavior...
        if (Input.touchCount > 0)
        {
            // ...check how much the finger moved on x and y since last frame (deltaPosition)
            hor = Input.GetTouch(0).deltaPosition.x;
            ver = Input.GetTouch(0).deltaPosition.y;
        }

        // setup a translation (direction) for the movement
        // since the values range from [-1, 1] they are a good way of setting our direction
        translation = new Vector3(hor, 0, ver);

        // apply the movement to this cube's transform
        transform.position += translation * (speed * Time.deltaTime);
    }
}
