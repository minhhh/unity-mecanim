using UnityEngine;
using System.Collections;

public enum Weapon
{
    None,
    Wand,
    Sword
}

[DisallowMultipleComponent]
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public readonly Vector3 GroundDistanceCheckVector = new Vector3 (0, -0.2f, 0);
    public readonly Vector3 GroundCheckErrorVector = new Vector3 (0, 0.1f, 0);

    Animator animator;
    Rigidbody myRigidBody;
    bool grounded = false;

    void Awake ()
    {
        animator = GetComponent <Animator> ();
        myRigidBody = GetComponent <Rigidbody> ();
    }

    void Update ()
    {
        CheckGround ();
        Move ();
        Jump ();

        SyncAnimParameters ();
    }

    void CheckGround ()
    {
        RaycastHit hitInfo;;

        if (CheckApproximateHit(transform.position, out hitInfo)) {
            if (hitInfo.collider.gameObject.CompareTag ("Ground")) {
                grounded = true;
            }
        } else {
            grounded = false;
        }
    }

    bool CheckApproximateHit (Vector3 position, out RaycastHit hitInfo)
    {
        if (Physics.Raycast(position + GroundCheckErrorVector, GroundDistanceCheckVector, out hitInfo)) {
            return true;
        }
        return false;
    }

    void SyncAnimParameters ()
    {
        if (animator.GetFloat ("gravityWeight") < 0.1) {
            if (myRigidBody.useGravity) {
                myRigidBody.useGravity = false;
            }
        } else {
            if (!myRigidBody.useGravity) {
                myRigidBody.useGravity = true;
            }
        }

        if (animator.GetBool ("grounded") != grounded) {
            animator.SetBool ("grounded", grounded);
        }
    }

    void Move ()
    {
        animator.SetFloat ("speed", Input.GetAxis ("Vertical"));
    }

    void Jump ()
    {
        if (Input.GetButtonDown ("Jump")) {
            if (!animator.GetBool ("jump") && grounded) {
                animator.SetTrigger ("jump");
            }
        }
    }
}
