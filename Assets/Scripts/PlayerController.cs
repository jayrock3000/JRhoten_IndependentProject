using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float
        horizontalSpeed,
        verticalSpeed;

    public string
      playerFacing;

    private float
        horizontalInput,
        verticalInput,
        speedMultiplier,
        maxSpeed,
        thrustAmount;

    private string
        thrustDirection,
        lastThrustDirection;

    private string[] direction = { "up", "upright", "right", "downright", "down", "downleft", "left", "upleft", "neutral" };
    
    // Start is called before the first frame update
    void Start()
    {
        horizontalSpeed = 0.0f;
        verticalSpeed = 0.0f;
        maxSpeed = 30.0f;
        speedMultiplier = 0.6f;

        playerFacing = "neutral";

        // Thrust is a quick acceleration that happens when the player changes directions
        thrustAmount = 0.0f;
        thrustDirection = "initial";
        lastThrustDirection = "null";
    }

    string getDirection(float horizontalMovement, float verticalMovement)
    {
        bool isFacingVertical = false;
        bool isFacingUp = false;
        bool isFacingHorizontal = false;
        bool isFacingRight = false;

        if (Mathf.Abs(horizontalMovement) > maxSpeed/2)
        {
            isFacingHorizontal = true;

            if (horizontalMovement > 0)
                isFacingRight = true;
        }

        if (Mathf.Abs(verticalMovement) > maxSpeed/2)
        {
            isFacingVertical = true;

            if (verticalMovement >= 0)
                isFacingUp = true;
        }

        // Not sure if we want to include a neutral position while skating, we'll see
        /*
        if (isFacingHorizontal == false && isFacingVertical == false)
            return "neutral";
        */

        if (isFacingHorizontal == false)
        {
            if (isFacingVertical == true && isFacingUp == true)
                return "up";

            else
                return "down";
        }

        if (isFacingVertical == false)
        {
            if (isFacingHorizontal == true && isFacingRight == true)
                return "right";

            else
                return "left";
        }

        if (isFacingUp == true)
        {
            if (isFacingRight == true)
                return "upright";
            else
                return "upleft";
        }

        if (isFacingRight == true)
            return "downright";
        else
            return "downleft";
    }

    void slowDown(float slowAmount)
    {
        if (horizontalSpeed > 0)
            horizontalSpeed -= slowAmount * (Mathf.Abs(horizontalSpeed) / maxSpeed);

        else if (horizontalSpeed < 0)
            horizontalSpeed += slowAmount * (Mathf.Abs(horizontalSpeed) / maxSpeed);

        if (verticalSpeed > 0)
            verticalSpeed -= slowAmount * (Mathf.Abs(verticalSpeed) / maxSpeed);

        else if (verticalSpeed < 0)
            verticalSpeed += slowAmount * (Mathf.Abs(verticalSpeed) / maxSpeed);
        
    }
    
    
    void determineThrust()
    {
        thrustDirection = getDirection(horizontalInput, verticalInput);

        if (thrustDirection.CompareTo(lastThrustDirection) != 0)
        {
            lastThrustDirection = thrustDirection;
            thrustAmount = 20.0f;
            print(thrustAmount);
        }

        else
        {
            lastThrustDirection = thrustDirection;
            thrustAmount = 0.0f;
        }
    }
    

    void FixedUpdate()
    {
        // Player Input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // See if they've changed directions
        if (horizontalInput != 0 && verticalInput != 0)
            determineThrust();

        // Calculate speed

        if (horizontalSpeed <= maxSpeed && horizontalInput > 0)
        {
            horizontalSpeed += (horizontalInput + thrustAmount);
        }

        if (horizontalSpeed >= -maxSpeed && horizontalInput < 0)
        {
            horizontalSpeed += (horizontalInput - thrustAmount);
        }

        if (verticalSpeed <= maxSpeed && verticalInput > 0)
        {
            verticalSpeed += verticalInput + thrustAmount;
        }

        if (verticalSpeed >= -maxSpeed && verticalInput < 0)
        {
            verticalSpeed += verticalInput - thrustAmount;
        }

        // Determine which direction they're facing
        playerFacing = getDirection(horizontalSpeed, verticalSpeed);

        // Slow them down gradually
        slowDown(0.5f);

        // Reset the thrust amount
        thrustAmount = 0.0f;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * verticalSpeed * speedMultiplier);
        transform.Translate(Vector3.right * Time.deltaTime * horizontalSpeed * speedMultiplier);

        //int test = -1;
        //print(Mathf.Abs(test));
        /*
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * Time.deltaTime * 25.0f * verticalInput);
        transform.Translate(Vector3.right * Time.deltaTime * 25.0f * horizontalInput);
        */
    }
}
