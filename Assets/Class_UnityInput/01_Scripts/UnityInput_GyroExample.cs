using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInput_GyroExample : MonoBehaviour
{
    // movement variables
    public float speed;

    private Gyroscope gyro;
    private Quaternion gyroRotMultiplier;
    private GameObject container;

    // Start is called before the first frame update
    void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;

        // setup container rotation, to match gyroscope orientation
        // this way we avoid disorienting the actual cube 
        container = GetComponentInParent<GameObject>();
        container.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
        gyroRotMultiplier = new Quaternion(0, 0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // MOVEMENT
        MoveCube();

        // ROTATION
        RotateCube();
    }

    private void MoveCube()
    {
        // if there's fingers touching the screen...
        if (Input.touchCount > 0)
        {
            float screenFirstHalf = Screen.width * 0.5f;

            // ... grab the first one and check which half of the screen is being touched
            if (Input.GetTouch(0).position.x < screenFirstHalf)
            {
                // apply the movement to this cube's transform
                transform.position += transform.forward * (speed * Time.deltaTime);
            }
            else
            {
                transform.position -= transform.forward * (speed * Time.deltaTime);
            }
        }
    }

    private void RotateCube()
    {
        // rotate the cube based on the parent's rotation (localRotation) and considering the gyro orientation
        transform.localRotation = gyro.attitude * gyroRotMultiplier;
    }
}
