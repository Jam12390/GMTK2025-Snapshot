using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class movement : MonoBehaviour
{
    //placeholder stats
    public int speed = 10;
    public int jumpForce = 20;
    public int remainingJumps = 1;
    public int maxJumps = 1;
    public int stationaryYFrames = 0;
    private bool onFloor = false;
    public bool onWall = false;
    private string wallCollisionDirection = "";
    public List<int> dashSpeed = new List<int>();
    public int dashCount = 1;
    public int maxDashCount = 1;
    public bool dashing = false;
    //end placeholder stats
    public Rigidbody2D rb;
    public List<string> movementDirection = new List<string> { "", "" };
    public List<int> movementDelayFrames = new List<int> { 0, 0, 0 }; //xDir, yDir, dashing, jumping. TODO: Change this to a dictionary and convert frame delay into time.deltatime to be more consistent

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        movementDelayFrames.Add(0); //for some reason this is the only way that unity is registering that there is a third index to the list
        dashSpeed.Add(50);
        dashSpeed.Add(50);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movementDelayFrames[0] <= 0)
        {
            rb.linearVelocityX = Input.GetKey(KeyCode.A) && !dashing ? speed * -1 : rb.linearVelocityX;
            rb.linearVelocityX = Input.GetKey(KeyCode.D) && !dashing ? speed : rb.linearVelocityX;
            rb.linearVelocityX = -0.25 < rb.linearVelocityX && rb.linearVelocityX < 0.25 ? 0 : rb.linearVelocityX;
        }
        else
        {
            movementDelayFrames[0] -= 1;
        }

        rb.linearVelocityX /= !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) ? 1.1f : 1;
        rb.linearVelocityX /= dashing ? 1.2f : 1;
        rb.linearVelocityY /= dashing ? 1.32f : 1;

        if (-speed < rb.linearVelocityX && rb.linearVelocityX < speed && rb.linearVelocityY < speed) //since speed is constant if velocity in all directions is less than speed we aren't dashing
        {
            dashing = false;
        }

        if (movementDelayFrames[1] <= 0 && Input.GetKey(KeyCode.Space) && remainingJumps > 0 && movementDelayFrames[3] <= 0 && (!onWall || onFloor)) //logic for jumping whilst either on the floor or not on a wall
        {
            rb.linearVelocityY = jumpForce;
            remainingJumps -= 1;
            movementDelayFrames[3] += 15;
        }
        else if (movementDelayFrames[1] <= 0 && Input.GetKey(KeyCode.Space) && onWall && !onFloor)
        {
            switch (wallCollisionDirection)
            {
                case "r":
                    rb.linearVelocityX = speed;
                    rb.linearVelocityY = jumpForce;
                    break;
                case "l":
                    rb.linearVelocityX = -speed;
                    rb.linearVelocityY = jumpForce;
                    break;
            }
            movementDelayFrames[0] = 10; //stop the player from immediately being able to move back towards the same wall
            movementDelayFrames[3] = 15; //stop the player from immediately jumping and wasting a jump
        }
        else
        {
            movementDelayFrames[1] -= movementDelayFrames[1] > 0 ? 1 : 0;
            movementDelayFrames[3] -= movementDelayFrames[3] > 0 ? 1 : 0;
        }

        movementDirection[0] = Input.GetKey(KeyCode.W) ? "u" : Input.GetKey(KeyCode.S) ? "d" : "";
        movementDirection[1] = Input.GetKey(KeyCode.D) ? "r" : Input.GetKey(KeyCode.A) ? "l" : "";

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && dashCount > 0 && movementDelayFrames[2] <= 0)
        {
            rb.linearVelocityY = movementDirection[0] == "u" ? dashSpeed[1] : movementDirection[0] == "d" ? dashSpeed[1] * -1 : rb.linearVelocityY;
            movementDelayFrames[1] += rb.linearVelocityY == dashSpeed[1] ? 15 : 0;
            movementDelayFrames[3] += rb.linearVelocityY == dashSpeed[1] || rb.linearVelocityY == -dashSpeed[1] ? 15 : 0;
            rb.linearVelocityX = movementDirection[1] == "r" ? dashSpeed[0] : movementDirection[1] == "l" ? dashSpeed[0] * -1 : rb.linearVelocityX;
            movementDelayFrames[0] += rb.linearVelocityX == dashSpeed[0] ? 15 : 0;
            movementDelayFrames[2] += 15;
            dashCount -= 1;
            dashing = true;
        }
        else if (movementDelayFrames[2] > 0)
        {
            movementDelayFrames[2] -= 1;
        }

        if (rb.linearVelocityY <= 0.05 && rb.linearVelocityY >= -0.05)
        {
            stationaryYFrames += 1;
        }
        else
        {
            stationaryYFrames = 0;
            onFloor = false;
        }

        if (stationaryYFrames >= 5)
        {
            onFloor = true;
            remainingJumps = maxJumps;
        }
        else
        {
            onFloor = false;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            onFloor = true;
            remainingJumps = maxJumps;
            dashCount = maxDashCount;
        }
        if (collision.gameObject.tag == "lWall")
        {
            onWall = true;
            wallCollisionDirection = "l";
        }
        else if (collision.gameObject.tag == "rWall")
        {
            onWall = true;
            wallCollisionDirection = "r";
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            onFloor = false;
            remainingJumps -= 1;
        }
        if (collision.gameObject.tag == "lWall" || collision.gameObject.tag == "rWall")
        {
            onWall = false;
            wallCollisionDirection = "";
        }
    }
}
