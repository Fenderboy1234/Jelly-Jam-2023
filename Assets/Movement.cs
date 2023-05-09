using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    // The Rigidbody attached to the GameObject.
    private Rigidbody body;
    /// <summary>
    /// Speed scale for the velocity of the Rigidbody.
    /// </summary>
    public float speed;
    /// <summary>
    /// Rotation Speed scale for turning.
    /// </summary>
    public float rotationSpeed;
    /// <summary>
    /// The upwards jump force of the player.
    /// </summary>
    public float jumpForce;
    // The vertical input from input devices.
    private float vertical;
    // The horizontal input from input devices.
    private float horizontal;
    // Whether or not the player is on the ground.
    private bool isGrounded;
    // Initialization function
    void Start()
    {
        // Obtain the reference to our Rigidbody.
        body = GetComponent<Rigidbody>();

    }
    // Fixed Update is called a fix number of frames per second.
    void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        if (Input.GetAxis("Jump") > 0)
        {
            if (isGrounded)
            {
                body.AddForce(transform.up * jumpForce);
            }
        }
        Vector3 velocity = (transform.forward * vertical) * speed * Time.fixedDeltaTime;
        velocity.y = body.velocity.y;
        body.velocity = velocity;
        transform.Rotate((transform.up * horizontal) * rotationSpeed * Time.fixedDeltaTime);
    }
    // This function is a callback for when an object with a collider collides with this objects collider.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            isGrounded = true;
        }
    }
    // This function is a callback for when the collider is no longer in contact with a previously collided object.
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            isGrounded = false;
        }
    }
}