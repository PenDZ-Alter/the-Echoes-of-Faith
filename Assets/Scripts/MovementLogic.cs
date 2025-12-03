using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLogic : MonoBehaviour
{
    [Header("Character Setup")]
    public Transform PlayerOrientation;
    public Animator anim;

    [Header("Movement Settings")]
    public float walkSpeed;
    public float runSpeed;
    public float jumpPower;
    public float fallSpeed;
    public float airMultiplier;

    private Rigidbody rb;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    bool grounded = true, aerialboost = true;
    bool TPSMode = true;
    bool AimMode = false;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        PlayerOrientation = this.GetComponent<Transform>();

    }

    void Update()
    {
        Movement();
        Jump();
        ShootLogic();

    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            grounded = false;
            anim.SetBool("Jump", true);
        }
        else if (!grounded)
        {
            rb.AddForce(Vector3.down * fallSpeed * rb.mass, ForceMode.Force);
            if (aerialboost)
            {
                rb.AddForce(moveDirection.normalized * walkSpeed * airMultiplier, ForceMode.Impulse);
                aerialboost = false;
            }
        }
    }
    public void groundedchanger()
    {
        grounded = true;
        aerialboost = true;
        anim.SetBool("Jump", false);
    }

    private void Movement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = PlayerOrientation.forward * verticalInput + PlayerOrientation.right * horizontalInput;

        if (grounded && moveDirection != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
                rb.AddForce(moveDirection.normalized * runSpeed * 10f, ForceMode.Force);

            }
            else
            {
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                rb.AddForce(moveDirection.normalized * walkSpeed * 10f, ForceMode.Force);

            }
        }
        else
        {
            anim.SetBool("Run", false);
            anim.SetBool("Walk", false);
        }
    }

    private void ShootLogic()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (moveDirection.normalized != Vector3.zero)
            {
                anim.SetBool("WalkShoot", true);
                anim.SetBool("IdleShoot", false);
            }
            else
            {
                anim.SetBool("IdleShoot", true);
                anim.SetBool("WalkShoot", false);
            }
        }
        else
        {
            anim.SetBool("WalkShoot", false);
            anim.SetBool("IdleShoot", false);
        }
    }
    public void AimModeAdjuster()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (AimMode)
            {
                TPSMode = true;
                AimMode = false;
                anim.SetBool("AimMode", false);
            }
            else if (TPSMode)
            {
                TPSMode = false;
                AimMode = true;
                anim.SetBool("AimMode", true);
            }
        }
    }



}

