using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    static Animator anim;

    private float speed = 5.0f;
    private CharacterController controller;
    private Vector3 moveVector;
    private float gravity = 12.0f;
    private float verticalVelocity = 0.0f;
    private float jumpForce = 4.5f;

    private float animationDuration = 1.0f;
    private float startTime;

    private bool isDead = false;

    // Use this for initialization
    public void Start ()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        startTime = Time.time;
	}
	
	// Update is called once per frame
    public void Update ()
    {
        if (isDead)
            return;

        if (Time.time - startTime < animationDuration)
        {
            controller.Move(Vector3.forward * speed * Time.deltaTime);
            anim.SetBool("isRunning", true);
            return;
        }

        if (controller.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpForce;
            }   
        }
        else
        { 
          {
            verticalVelocity -= gravity * Time.deltaTime;
          }
        }

        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxisRaw("Horizontal") * speed;
        moveVector.y = verticalVelocity;
        moveVector.z = Input.GetAxis("Vertical") * speed;

        controller.Move (moveVector * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("isJumping");
        }

        if (moveVector.z != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    public void SetSpeed(float modifier)
    {
        speed = 5.0f + modifier;
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.point.z > transform.position.z + 0.1f && hit.gameObject.tag == "Enemy")
            Death();
    }

    public void Death()
    {
        isDead = true;
        GetComponent<Score>().OnDeath ();
    }
}
