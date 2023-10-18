using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInput_Exercise_1 : MonoBehaviour
{
    // movement variables
    public float rotSpeed;

    private float hor = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if there's fingers touching the screen...
        if (Input.touchCount > 0)
        {
            float screenFirstHalf = Screen.width * 0.5f;

            // ... grab the first one and check which half of the screen is being touched
            if (Input.GetTouch(0).position.x < screenFirstHalf)
            {
                // Mathf.Lerp is a mathematical function that interpolates between 2 values, with a given speed
                // in this case our goal is to smoothly get the "hor" to -1 or 1 (simulating the Input.GetAxis("Horizontal") on PC)
                hor = Mathf.Lerp(hor, -1, Time.deltaTime * rotSpeed);
            }
            else
            {
                hor = Mathf.Lerp(hor, 1, Time.deltaTime * rotSpeed);
            }
        }
        // when no finger is touching the screen, then we interpolate the value back to 0, so the cube stop
        else
        {
            hor = Mathf.Lerp(hor, 0, Time.deltaTime * rotSpeed);
        }

        Vector3 translation = new Vector3(0, hor, 0);
        transform.Rotate(translation);
    }
}
