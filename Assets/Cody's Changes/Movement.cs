using UnityEngine;
using System.Collections;

/*
 * Writer: Cody - xKriostar
 */

public class Movement : MonoBehaviour {

	public float movementSpeed = 5;
    public float runSpeed = 2;
	public float jumpHeight = 2;

    Camera mainCam;
    float h;//horizontal player input (W,S,up arrow,down arrow)
    float v;//vertical player input (A,D,<-,->)
    Vector3 dir;
    Animator anim;
    Quaternion lastRotation;

    float counter;

	void Start () {
        mainCam = Camera.main;
        anim = GetComponent<Animator>();
	}

	void Update () {

        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        dir = (mainCam.transform.forward * v + h * mainCam.transform.right).normalized;
        dir *= movementSpeed;
        dir.y = 0;
        
        if (v > 0.1f || h > 0.1f || v < -0.1f || h < -0.1f)
        {
            Move();

            //Use this for Quick-attack
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Attack();
            }
        }
        else
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsIdle", true);

            rigidbody.velocity = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.AddForce(Vector3.up * jumpHeight,ForceMode.Impulse);
            
            //Add Jump anim
            //Template:
            anim.SetBool("IsIdle",true);
            anim.SetBool("IsRunning", false);
        }

	}

    void Move()
    {
        rigidbody.AddForce(dir);
        transform.LookAt(transform.position + dir);
        anim.SetBool("IsRunning", true);
        anim.SetBool("IsIdle", false);
        anim.SetBool("Attack1", false);     
    }
    void Attack()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(dir, ForceMode.Impulse);
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("Attack1", true);
    }

    //public void Swim(Transform water)
    //{
    //    Debug.Log("Swimming");
    //    dir = h * mainCam.transform.right + v * mainCam.transform.forward;
    //    dir.y = water.position.y;
    //    rigidbody.AddForce(dir);
    //    transform.LookAt(transform.position + dir);
    //    anim.SetBool("IsRunning", true);
    //    anim.SetBool("IsIdle", false);
    //    anim.SetBool("Attack1", false);
    //}
}
