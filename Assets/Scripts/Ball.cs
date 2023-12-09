using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : IWeapon
{
    private Vector3 mousePos;
    private Camera mainCamera;
    private Rigidbody rigidBody;
    public float force = 15f;
    public int damagePower = 10;

    public override void activate(int destroyDelay) {
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
    }

    public override int damage() {
        return damagePower;
    }

}