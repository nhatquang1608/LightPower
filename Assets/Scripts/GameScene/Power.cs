using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Animator animator;

    public void SetLineRenderer(GameObject begin, GameObject end, bool set)
    {
        if(set)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, begin.transform.position);
            lineRenderer.SetPosition(1, end.transform.position);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    public void PowerOff()
    {
        animator.SetTrigger("off");
    }
}
