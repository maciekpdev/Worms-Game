using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : IWeapon
{
    public GameObject explosionEffect;
    private Vector3 mousePos;
    private Camera mainCamera;
    private Rigidbody rigidBody;
    public float force = 15f;
    public float explosionForce = 50f;
    public int damagePower = 10;
    public float radius = 5f;
    public int destroyDelay = 2;

    public int explosionDamage = 30;

    public override IEnumerator activate(ChangePlayerDelegate postAttackCallback)
    {
        Collider capsuleCollider = GetComponent<Collider>();

        if (capsuleCollider != null)
        {
            capsuleCollider.enabled = true;
        }


        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rigidBody = GetComponent<Rigidbody>();

        float playerAndCamPosDiffZ = transform.position.z - mainCamera.transform.position.z;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * playerAndCamPosDiffZ);
        Vector3 rotation = mousePos - transform.position;
        rigidBody.isKinematic = false;
        rigidBody.velocity = rotation.normalized * force;

        yield return new WaitForSeconds(2);
        destroy();
        yield return new WaitForSeconds(2);
        postAttackCallback();
    }

    public override int damage()
    {
        return damagePower;
    }

    void destroy()
    {
        var effect = Instantiate(explosionEffect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.gameObject.tag == "Destructible")
            {
                Destroy(nearbyObject.gameObject);
            }

            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                continue;
            }

            if (nearbyObject.gameObject.tag == "Player")
            {
                nearbyObject.GetComponent<PlayerHealth>().healthChange(-explosionDamage);
            }

            rb.AddExplosionForce(explosionForce, transform.position, radius, 0f, ForceMode.Impulse);

        }
        Destroy(effect, 1.0f);
        Destroy(gameObject);
    }

}
