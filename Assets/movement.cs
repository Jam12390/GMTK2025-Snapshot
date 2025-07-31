using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class movement : MonoBehaviour
{
    //placeholder stats
    public int speed = 10;
    public int jumpForce = 20;
    public int jumpCount = 1;
    public int maxJumps = 1;
    public List<int> dashSpeed = new List<int>();
    public int dashCount = 1;
    public int maxDashCount = 1;
    public bool dashing = false;
    //end placeholder stats
    public Rigidbody2D rb;
    public List<string> movementDirection = new List<string> { "", "" };
    public List<int> movementDelayFrames = new List<int> { 0, 0, 0 }; //xDir, yDir, dashing, jumping - Should probably change this to a dictionary

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        movementDelayFrames.Add(0);
        dashSpeed.Add(50);
        dashSpeed.Add(50);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (movementDelayFrames[0] <= 0)
        //{
        //    rb.linearVelocityX = Input.GetKey(KeyCode.A) ? speed * -1 : rb.linearVelocityX < 0 ? rb.linearVelocityX / 1.1f : rb.linearVelocityX;
        //    rb.linearVelocityX = Input.GetKey(KeyCode.D) ? speed : rb.linearVelocityX > 0 ? rb.linearVelocityX / 1.1f : rb.linearVelocityX;
        //}
        //else
        //{
        //    movementDelayFrames[0] -= 1;
        //}

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

        //rb.linearVelocityX = (rb.linearVelocityX < 0.25 && rb.linearVelocityX > -0.25) ? 0 : rb.linearVelocityX;

        if (movementDelayFrames[1] <= 0 && Input.GetKey(KeyCode.Space) && jumpCount > 0 && movementDelayFrames[3] <= 0)
        {
            rb.linearVelocityY = jumpForce;
            jumpCount -= 1;
            movementDelayFrames[3] += 15;
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
        Debug.Log(Input.GetKey(KeyCode.LeftShift));
        Debug.Log(movementDirection[0]);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            jumpCount = maxJumps;
            dashCount = maxDashCount;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            jumpCount -= 1;
        }
    }
}
