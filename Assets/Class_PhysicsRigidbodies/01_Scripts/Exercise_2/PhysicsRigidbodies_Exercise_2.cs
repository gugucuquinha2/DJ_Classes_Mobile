using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsRigidbodies_Exercise_2 : MonoBehaviour
{
    // movement variables
    public float force;

    private Rigidbody rb;

    private float touchTimeStart;
    private float touchTimeEnd;
    private float timeDelta;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Swipe detection
        SwipeDetection();
    }

    private void SwipeDetection()
    {
        // if there's tocuhes...
        if (Input.touchCount > 0)
        {
            // ... and the touch just began
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // record its time and position
                touchTimeStart = Time.time;
                startPos = Input.GetTouch(0).position;
            }

            // ... if the touch ended
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                // record the time and position
                touchTimeEnd = Time.time;
                endPos = Input.GetTouch(0).position;

                // calculate the swipe direction and how fast is the swipe
                timeDelta = touchTimeEnd - touchTimeStart;
                direction = endPos - startPos;

                // Apply a force on our object
                rb.AddForce(direction.x, direction.y, force / timeDelta);
            }
        }
    }
}
