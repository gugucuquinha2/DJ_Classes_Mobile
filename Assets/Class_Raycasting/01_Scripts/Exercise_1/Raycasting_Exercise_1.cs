using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycasting_Exercise_1 : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject child;

    private bool canRaycast;

    private Vector3 offset;
    private Vector3 screenObjectPoint;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        // cache the camera object for optimization
        cam = Camera.main;

        rb = GetComponent<Rigidbody>();
        canRaycast = true;

        // we can get the parachute (child of the falling object) right at the beginning
        // the bool "true" specifies that our "GetComponentsInChildren" is also supposed to search on inactive objects (such as our parachute), otherwise we get an error
        // in our array, we want the transform of the child, since "GetComponentsInChildren", searches in both the parent and the children:
        // - since we only have 2 objects, it will return 2 transforms, being the second (index 1) the transform of the child (parachute)
        Transform childTransform = GetComponentsInChildren<Transform>(true)[1];
        // get the gameObject associated to that transform 
        // notice that since a GameObject isn't a Component, we can't search directly for GameObjects using "GetComponentsInChildren" - we can only search for an actual component
        child = childTransform.gameObject;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            // when the touch starts
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // while moving the object around, make sure to disable physics, "parachute" and raycast as we're forcing the position change directly
                rb.isKinematic = true;
                canRaycast = false;
                child.SetActive(false);

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
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                // once we stop our drag, then reactivate the physics so the game can check how close to the ground it is again
                rb.isKinematic = false;
                canRaycast = true;
                rb.drag = 0;
            }
        }
    }


    void FixedUpdate()
    {
        // to improve the performance and so that our Raycast is only made when needed (before touching the ground)
        // we check for this boolean
        if (canRaycast)
        {
            // we debug the ray so we can see what's happening
            Debug.DrawRay(transform.position, Vector3.down * 6, Color.green);

            Ray ray = new Ray(transform.position, Vector3.down);

            // since we're using Tag check and not LayerMasks, we don't need any LayerMasks on our Raycast
            if (Physics.Raycast(ray, out RaycastHit hit, 6))
            {
                // this is another way of limiting Raycast detections
                // just like we'd do with the "OnCollisionEnter", etc, callbacks
                // NOTICE: Using LayerMasks is still preferrable, because with tags, the Raycast is still made every frame (in this case), we're simply checking if it collides with our "Ground" before applying the behaviour
                // LayerMasks are better because they actually prevent Raycast collision on the specified layers
                if (hit.transform.gameObject.CompareTag("Ground"))
                {
                    // once we detect the ground, we can say we can't raycast anymore, so the next frame, our FixedUpdate stops at the the first boolean check
                    // preventing unnacessary raycasts to be made and saving performance (we can check if it worked, by observing our "DrawRay" dissapear once the Raycast detects the ground)
                    canRaycast = false;

                    // we change the drag value of the Rigidbody of our falling object (will result in a sudden slow down of the falling object)
                    rb.drag = 6;
                    // and we active the gameObject of the child, so it looks like the parachute is making the object slow down
                    child.SetActive(true);
                }
            }
        }
    }
}
