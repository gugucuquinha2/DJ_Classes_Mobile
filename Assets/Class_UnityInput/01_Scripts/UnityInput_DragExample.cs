using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInput_DragExample : MonoBehaviour
{

    private Vector3 offset;
    private Vector3 screenObjectPoint;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        // cache the camera object for optimization
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            // when the touch starts
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // convert the object's position into the screen position
                screenObjectPoint = cam.WorldToScreenPoint(transform.position);

                // get the touch position, but considering the actual object's depth (Z axis - we're in 3D)
                Vector3 touchPos = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, screenObjectPoint.z);

                // calculate the offset between the object position and the actual mouse position (in world space)
                // the purpose of this is to make sure the object stays in position no matter where you grab it from (otherwise it will jump to your touch location)
                offset = transform.position - cam.ScreenToWorldPoint(touchPos);
            }
            // when we're dragging the finger around the screen
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // get the touch position in world space
                Vector3 touchPos = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, screenObjectPoint.z);

                // set the position of the object
                // if you want the object to snap to the touch position, just remove the "offset" calculation
                transform.position = cam.ScreenToWorldPoint(touchPos) + offset;
            }
        }
    }

}
