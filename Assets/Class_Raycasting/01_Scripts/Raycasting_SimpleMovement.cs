using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycasting_SimpleMovement : MonoBehaviour
{
    // movement variables
    public float speed;
    private float hor = 0;
    private float ver = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            hor = Input.GetTouch(0).deltaPosition.x;
            ver = Input.GetTouch(0).deltaPosition.y;
        }
        else
        {
            hor = 0;
            ver = 0;
        }

        // setup a translation (direction) for the movement
        // since the values range from [-1, 1] they are a good way of setting our direction
        Vector3 translation = new Vector3(hor, 0, ver);

        // apply the movement to this cube's transform
        transform.position += translation * (speed * Time.deltaTime);
    }
}
