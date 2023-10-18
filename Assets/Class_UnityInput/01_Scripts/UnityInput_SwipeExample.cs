using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInput_SwipeExample : MonoBehaviour
{
    // movement variables
    public float speed;

    private float  touchTimeStart;
    private float touchTimeEnd;
    private float timeDelta;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // if there's tocuhes...
        if(Input.touchCount > 0)
        {
            // ... and the touch just began
            if(Input.GetTouch(0).phase == TouchPhase.Began)
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
            }
        }

        // use that direction and speed to specify where to move the cube and how fast
        transform.position += direction.normalized * (speed / timeDelta) * Time.deltaTime;

    }

}
