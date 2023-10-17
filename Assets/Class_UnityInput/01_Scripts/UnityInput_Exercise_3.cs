using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInput_Exercise_3 : MonoBehaviour
{
    // movement variables
    public float rotSpeed;

    // prefab variables
    public GameObject spherePrefab;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // ROTATION
        RotateCube();

        // SHOOT SPHERE
        ShootSphere();
    }

    private void RotateCube()
    {
        // The accelererometer works with the phone laid horizontally - make sure to place it properly to test builds properly
        float yRot = -Input.acceleration.x * (rotSpeed * Time.deltaTime);
        Vector3 translation = new Vector3(0, yRot, 0);
        
        transform.Rotate(translation);
    }

    private void ShootSphere()
    {
        // every time we press the "Left Mouse Button" (also works with touch inputs - simpler alternative to Input.touches[0]) we instantiate a new bullet
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(spherePrefab, transform.position, transform.rotation);
        }
    }
}
