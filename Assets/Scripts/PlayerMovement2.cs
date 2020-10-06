using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    const float MidAirMultiplier = 0.5f;
    const float SpeedLimit = 15f;
    const float InitialSpeed = 8f;
    const float JumpBufferTime = 0.1f;
    const float JumpDistance = 4f;

    // each second the player speeds up by 0.25 m/s
    const float Accel = 2.5f;
    
    public int zDirection;
    public bool needToJump;

    // jump buffer
    public float jumpBuffer;
    public bool isGrounded = true;

    CapsuleCollider col;
    Rigidbody rig;

    void Start()
    {
        col = GetComponent<CapsuleCollider>();
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            needToJump = true;
        }

        // this stuff handles z axis movement        
        zDirection = Input.GetKeyDown(KeyCode.A) ? -1 : (Input.GetKeyDown(KeyCode.D) ? 1 : zDirection);

        if (Input.GetKeyUp(KeyCode.A) && zDirection == -1)
        {
            zDirection = Input.GetKey(KeyCode.D) ? 1 : 0;
        }
        else if (Input.GetKeyUp(KeyCode.D) && zDirection == 1)
        {
            zDirection = Input.GetKey(KeyCode.A) ? -1 : 0;
        }
    }

    void FixedUpdate()
    {
        HandleKeyboardMouseInputs();
    }

    void HandleKeyboardMouseInputs()
    {
        bool isGroundedNow = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), transform.localScale.y / 2.0f + 0.01f);

        // always reset buffer to JumpBufferTime if not grounded
        if (!isGrounded && isGroundedNow || !isGroundedNow)
        {
            jumpBuffer = JumpBufferTime;
        }
        isGrounded = isGroundedNow;

        if (needToJump && jumpBuffer <= 0)
        {
            rig.AddForce(new Vector3(0, (float)Math.Sqrt(Physics.gravity.y * -2 * JumpDistance), 0), ForceMode.VelocityChange);
        }
        needToJump = false;
        jumpBuffer -= Time.fixedDeltaTime;

        if (zDirection == 0)
        {
            rig.velocity = new Vector3(rig.velocity.x, rig.velocity.y, 0);
        }
        else
        {        
            if (rig.velocity.z == 0 || rig.velocity.z * zDirection < 0)
            {
                rig.velocity = new Vector3(rig.velocity.x, rig.velocity.y, 0);
                rig.AddForce(new Vector3(0, 0, zDirection * InitialSpeed * (isGrounded ? 1 : 0.5f)), ForceMode.VelocityChange);
            }
            var vel = new Vector3(0, 0, zDirection * Accel * (isGrounded ? 1 : 0.5f));
            rig.AddForce(vel, ForceMode.Acceleration);
        }

    }
}