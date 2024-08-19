using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimTest : MonoBehaviour
{
    [SerializeField]
    AnimationClip anim;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayAnim();
        }
    }

    void PlayAnim()
    {
        animator.Play(anim.name);
    }
}
