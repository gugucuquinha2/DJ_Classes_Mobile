using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsRigidbodies_Explosion_Example : MonoBehaviour
{
    private Vector3 explosionPosition;
    public float explosionRadius;
    public float explosionForce;

    private float startTime;
    private float endTime;

    private void Start()
    {
        // sincer our cubes are at the positon (0, 0, 0). Our explosion position will be in the middle of them, for greater effect
        explosionPosition = Vector3.zero;
    }

    void Update()
    {
        // if there's tocuhes...
        if (Input.touchCount > 0)
        {
            // ... and the touch just began
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // record its start time
                startTime = Time.time;
            }
            // ... if the touch ended
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                // record its ending time
                endTime = Time.time;

                // calculate how long the touch lasted
                float touchDuration = endTime - startTime;

                Debug.Log(touchDuration);

                // cause an explosion passing the duration as a multiplier for the explosion force
                Explode(touchDuration);
            }
        }
    }

    private void Explode(float _explosionMultiplier)
    {
        // This new method "Physics.OverlapSphere" creates a detection area (sphere) which returns all colliders within its radius (in this case the "explosionRadius")
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        //this array of colliders will allow us to look for any Rigidbody in their GameObjects
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody rb = colliders[i].GetComponent<Rigidbody>();

            // only apply the explosion, if the object has a rigidbody (!= null), 
            //otherwise an error will occur (because we would be trying to add a force to a non-existant rigidbody)
            if(rb != null)
            {
                // the explosion requires a force, a position (same as our overlap sphere) and
                // a radius (again, same as before in our overlap sphere - so it's consistent with our detected objects)
                rb.AddExplosionForce(explosionForce * _explosionMultiplier, explosionPosition, explosionRadius);
            }
        }
    }
}
