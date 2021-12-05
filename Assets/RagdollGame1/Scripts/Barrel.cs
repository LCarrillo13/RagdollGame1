using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public float radius = 5.0F;
    public float power = 10.0F;

    
    /// <summary>
    /// Explodes barrel, applying ExplosionForce to all Rigidbodies in the radius
    /// </summary>
    void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.CompareTag("Player"))
        {
            Explode();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    
}
