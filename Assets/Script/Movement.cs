using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Joystick joystick;

    [SerializeField] float speed = 10;
    [SerializeField] float maxVelocityChange = 10f;
    [SerializeField] float tiltAmount = 10f;


    private Vector3 velocityVector = Vector3.zero; //initial velocity
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {

        //Joystick input
        float xMovementInput = joystick.Horizontal;
        float zMovementInput = joystick.Vertical;

        //Calculating velocity vectors
        Vector3 movementHorizontal = transform.right * xMovementInput;
        Vector3 movementVertical = transform.forward * zMovementInput;

        //Calculate the final movement velocity vector
        Vector3 movementVelocityVector = (movementHorizontal + movementVertical).normalized * speed;

        //Apply movement
        Move(movementVelocityVector);

        //Object tilting
        transform.rotation = Quaternion.Euler(joystick.Vertical * speed * tiltAmount, 0, -1 * joystick.Horizontal * speed * tiltAmount);
    }

    void Move(Vector3 movementVelocityVector)
    {
        velocityVector = movementVelocityVector;
    }

    private void FixedUpdate()
    {
        if (velocityVector != Vector3.zero)
        {
            //Get RigidBody current velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (velocityVector - velocity);

            //Apply a force to change target velocity
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;

            rb.AddForce(velocityChange,ForceMode.Acceleration);
        }

        
    }
}
