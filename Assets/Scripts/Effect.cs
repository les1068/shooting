using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))] // 이 스크립트를 가지는 게임 오브젝트는 반드시 animator를 가지게 되어있다.
public class Effect : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>(); 
        
    }
    private void Start()
    {
        Destroy(gameObject, anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);

    }
}
