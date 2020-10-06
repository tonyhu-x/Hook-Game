using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    const float SpeedLimit = 15f;
    const float InitialSpeed = 8f;
    
    // each second the player speeds up by 0.25 m/s
    const float Accel = 2.5f;
    public int zDirection;

    CapsuleCollider col;
    Rigidbody rig;

    void Start()
    {
        col = GetComponent<CapsuleCollider>();
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
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
        bool isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), transform.localScale.y / 2.0f + 0.01f);

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