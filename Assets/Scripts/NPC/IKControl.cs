using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))] 

public class IKControl : MonoBehaviour {
    
    protected Animator animator;
    
    public Transform player;

    void Start () 
    {
        animator = GetComponent<Animator>();
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if(animator) {
            // Set the look target position, if one has been assigned
            if(player != null) {
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(player.position);
            }  
        }
    }
}
