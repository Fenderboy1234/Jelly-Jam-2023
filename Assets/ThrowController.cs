using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectThrow;

    [Header("Settings")]
    public int totalThrow;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;


    //Boppa Zone------------------------------------------------------
    //I want to add the players rb velocity to the throw
    public Rigidbody playerRb;
    //I only want to be able to pick things up if they're close enough
    public float pickupRange;
    //I want to know what I can interact with
    public LayerMask interactableObject;
    //----------------------------------------------------------------
    private void Start()
    {
        readyToThrow = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrow > 0)
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        GameObject projectile = Instantiate(objectThrow, attackPoint.position, cam.rotation);

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce+ playerRb.velocity;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrow--;

        Invoke(nameof(ResetThrows), throwCooldown);
    }
    
    private void ResetThrows()
    {
        readyToThrow = true;
    }

    //----------------------------
    //This is for finding interactable objects
    private void FindPickup()
    {
        if (false/*Physics.Raycast(cam.position,cam.forward, out hit, pickupRange)*/)
        { 

        }
    }
    //----------------------------
}
