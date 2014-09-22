using UnityEngine;
using System.Collections;

/*
 * Writer: Cody - xKriostar
 */

public class Movement : MonoBehaviour {

	public float movementSpeed = 5;
	public float jumpHeight = 2;

    Camera mainCam;
    float h;//horizontal player input (W,S,up arrow,down arrow)
    float v;//vertical player input (A,D,<-,->)
    Vector3 dir;
    Animator anim;
    Collider[] hitPoints;
    RaycastHit groundedCheck;
    Vector3 lastPos;
    float unityGravity;
    float groundedGravity;
    float currentGravity;
    bool jump;
    bool move;

	void Start () {
        unityGravity = Physics.gravity.y;
        groundedGravity = 0.1f;
        mainCam = Camera.main;
        anim = GetComponent<Animator>();
	}

    void Update()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        dir = ((v * mainCam.transform.forward + h * mainCam.transform.right)).normalized;
        dir *= movementSpeed;
        dir.y = 0;

        if (v > 0.1f || h > 0.1f || v < -0.1f || h < -0.1f)
        {
            move = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(new Ray(transform.position, -transform.up), out groundedCheck, 0.4f))
            {
                if (groundedCheck.transform.gameObject.layer != LayerMask.NameToLayer("Player")) { jump = true; }
            }
        }
        if(!jump)
        {
            currentGravity = groundedGravity;
        }
        else
        {
            currentGravity = unityGravity;
        }

    }

	void FixedUpdate () 
    {

        if (jump)
        {
            Jump();
            jump = false;
        }

        if(move)
        {
            rigidbody.AddRelativeForce(-transform.up * currentGravity);
           
           Move();
        }
        if(dir.magnitude<=0)
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsIdle", true);

        }

	}

    void Jump()
    {
        Debug.Log("Jumping");
        
        rigidbody.AddForce(transform.up * jumpHeight,ForceMode.Impulse);

        //Add Jump anim
        //Template:
        anim.SetBool("IsIdle", true);
        anim.SetBool("IsRunning", false);
    }

    void Move()
    {
        rigidbody.AddForce(dir);
        transform.LookAt(transform.position +dir);

        anim.SetBool("IsRunning", true);
        anim.SetBool("IsIdle", false);
        anim.SetBool("Attack1", false);     
    }
}
