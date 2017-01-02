using UnityEngine;
using System.Collections;

public class MotionCurveController : MonoBehaviour
{
    private Animator animator;
    private Transform playerTransform;
    private Vector3 tempVec;
    public float yFactor = 0.0f;

    void Awake ()
    {
        animator = GetComponent<Animator> ();
        playerTransform = transform;
    }

    void OnAnimatorMove ()
    {
        tempVec = animator.deltaPosition;

        tempVec.y *= yFactor;

        playerTransform.position += tempVec;
    }
}
