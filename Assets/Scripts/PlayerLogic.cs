using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerLogic : MonoBehaviour
{
    [Header("Player Setup")]
    public Transform PlayerOrientation;

    [Header("Animation Settings")]
    public Animator animationOpt;

    [Header("Movement Settings")]
    public float speed;
    public float runSpeed;
    public float jumpPower;
    public float fallSpeed;
    public float airMultiplier;

    [Header("Stats Setup")]
    public float hitPoints;
    public float powerPoints;

    [Header("Sound Effects Options")]
    public AudioClip shootAudio;
    public AudioClip stepAudio;
    AudioSource playerAudio;

    [Header("Additional Scripts")]
    public CameraLogic camLogic;
    public UIGamePlayLogic UIGamePlay;

    // Private Variables
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    bool isGrounded = true, aerialBoost = true, TPSMode = true, AimMode = false;
    private float maxHealth;
    private Rigidbody rb;

    private void Movement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = PlayerOrientation.forward * verticalInput + PlayerOrientation.right * horizontalInput;

        bool runKey = Input.GetKey(KeyCode.LeftShift);

        if (isGrounded && moveDirection != Vector3.zero) {
            if (runKey) {
                animationOpt.SetBool("Run", true);
                animationOpt.SetBool("Walk", false);
                rb.AddForce(moveDirection.normalized * runSpeed, ForceMode.Force);
            } else {
                animationOpt.SetBool("Walk", true);
                animationOpt.SetBool("Run", false);
                rb.AddForce(moveDirection.normalized * speed, ForceMode.Force);
            }
        } else {
            animationOpt.SetBool("Walk", false);
            animationOpt.SetBool("Run", false);
        }
        
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
            animationOpt.SetBool("Jump", true);
        } else if (!isGrounded) {
            rb.AddForce(Vector3.down * fallSpeed * rb.mass, ForceMode.Force);
            if (aerialBoost) {
                rb.AddForce(moveDirection.normalized * speed * 1f * airMultiplier, ForceMode.Impulse);
                aerialBoost = false;
            }
        }
    }

    public void groundedChanger()
    {
        isGrounded = true;
        aerialBoost = true;
        animationOpt.SetBool("Jump", false);
    }

    public void AimAdjuster() 
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            if (AimMode) {
                AimMode = false;
                TPSMode = true;
                animationOpt.SetBool("AimMode", false);
            } else if (TPSMode) {
                TPSMode = false;
                AimMode = true;
                animationOpt.SetBool("AimMode", true);
            }
            camLogic.CameraModeChanger(TPSMode, AimMode);
        }
    }

    public void ShootLogic()
    {
        if (Input.GetKey(KeyCode.Mouse0)) {
            playerAudio.clip = shootAudio;
            playerAudio.Play();

            if (moveDirection.normalized != Vector3.zero) {
                animationOpt.SetBool("IdleShoot", false);
                animationOpt.SetBool("WalkShoot", true);
            } else {
                animationOpt.SetBool("IdleShoot", true);
                animationOpt.SetBool("WalkShoot", false);
            }
        } else {
            animationOpt.SetBool("IdleShoot", false);
            animationOpt.SetBool("WalkShoot", false);
        }
    }

    public void PlayerGetHit(float damage)
    {
        hitPoints = hitPoints - damage;
        UIGamePlay.UpdateHealthBar(hitPoints, maxHealth);

        if (hitPoints <= 0f) {
            animationOpt.SetBool("Death", true);
            this.enabled = false;
        }
    }

    public void Step() 
    {
        playerAudio.clip = stepAudio;
        playerAudio.Play();
    }
    
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        playerAudio = this.GetComponent<AudioSource>();
        // PlayerOrientation = this.GetComponent<Transform>();
        maxHealth = hitPoints;
        UIGamePlay.UpdateHealthBar(hitPoints, maxHealth);
        UIGamePlay.UpdatePowerBar(powerPoints, 100);
    }

    void Update()
    {
        Movement();
        Jump();
        AimAdjuster();
        ShootLogic();

        if (Input.GetKey(KeyCode.F)) {
            PlayerGetHit(100f);
        }
    }
}
