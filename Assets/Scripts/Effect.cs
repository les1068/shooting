using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))] // �� ��ũ��Ʈ�� ������ ���� ������Ʈ�� �ݵ�� animator�� ������ �Ǿ��ִ�.
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
