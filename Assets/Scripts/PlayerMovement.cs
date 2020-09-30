using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //TODO: Make it so this is physics based 
    Vector3 MoveVector = new Vector3(0, 0, 0);
    public float xvel = 0;
    public float zvel = 0;
    public float acc = 0.001f;
    public float speedlimit = 0.5f;
    public float InitialSpeed = 0.1f;
    public float JumpBufferTime = 0.1f;
    public float jumpForce = 0;
    public float MidAirDrift = 0.5f;
    bool IsGrounded;
    bool WasGrounded;
    float TimeOfLastLanding = 0;

    float TimeSinceLastLanding;
    float jump;
    bool SpaceDown;
    bool DDown;
    bool ADown;

    CapsuleCollider collider;
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        SpaceDown = Input.GetKey(KeyCode.Space);
        ADown = Input.GetKey(KeyCode.A);
        DDown = Input.GetKey(KeyCode.D);
    }

    void FixedUpdate()
    {
        HandleKeyBoardMouseInputs();
    }


    void HandleKeyBoardMouseInputs()
    {
        jump = 0;
        WasGrounded = IsGrounded;
        IsGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), transform.localScale.y / 2.0f + 0.0001f);
        if (ADown)
        {
            if (zvel == 0 && IsGrounded)
            {
                zvel = -InitialSpeed;
            }
            else if (zvel == 0 && !IsGrounded)
            {
                zvel -= InitialSpeed * 0.02f;
            }
            else if (zvel > -speedlimit + acc && IsGrounded)
            {
                zvel -= acc;
            }
            else if (!IsGrounded)
            {
                zvel -= MidAirDrift * acc;
            }
        }
        else if (zvel < 0)
        {
            zvel = 0;
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, 0);
        }

        if (DDown)
        {
            if (zvel == 0 && IsGrounded)
            {
                zvel = InitialSpeed;
            }
            else if (zvel == 0 && !IsGrounded)
            {
                zvel += InitialSpeed * 0.02f;
            }
            else if (zvel < speedlimit - acc && IsGrounded)
            {
                zvel += acc;
            }
            else if (!IsGrounded)
            {
                zvel += MidAirDrift * acc;
            }
        }
        else if (zvel > 0)
        {
            zvel = 0;
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, 0);
        }

        if (SpaceDown)
        {

            if (!WasGrounded && IsGrounded)
            {
                TimeOfLastLanding = Time.time;
            }

            TimeSinceLastLanding = Time.time - TimeOfLastLanding;
            if (IsGrounded && TimeSinceLastLanding > JumpBufferTime)
            {
                jump = 1.0f;

            }
        }

        rigidBody.AddForce(new Vector3(0, jump * jumpForce, 0), ForceMode.Impulse);

        MoveVector.z = zvel;
        if (MoveVector.z != 0)
        {
            rigidBody.MovePosition(transform.position + MoveVector * Time.deltaTime);
        }
    }
}
