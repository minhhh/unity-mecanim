using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent (typeof(Animator))]
public class Movement : MonoBehaviour
{
    public Animator animator;

    void Awake ()
    {
        animator = GetComponent <Animator> ();
         
    }

    void Update ()
    {
        Move ();
    }

    void Move ()
    {
        animator.SetFloat ("Forward", Input.GetAxis ("Vertical"));
        animator.SetFloat ("Turn", Input.GetAxis ("Horizontal"));
    }
}
