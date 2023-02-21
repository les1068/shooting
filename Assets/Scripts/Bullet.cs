using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    void Start()
    {
        //Destroy�Լ��� ������Ʈ�� �����ϴ� �Լ�
        Destroy(gameObject, 5.0f);
        Debug.Log("Start");
    }
    void Update()
    {
        //�ʴ� speed�� �ӵ��� right�������� ������(���� ��ǥ�� �������� �� ����) 
        //local��ǥ�� World��ǥ
        //local��ǥ : "�� ������Ʈ" �� �������� �� ��ǥ��
        //World��ǥ : "��"�� �������� �� ��ǥ��
        //transform.position += Time.deltaTime * speed * Vector3.right;
        //transform.position += Time.deltaTime * speed * transform.right;
       //transform.Translate(Time.deltaTime * speed * transform.right,Space.World);
        transform.Translate(Time.deltaTime * speed * Vector2.right); 
    }
}
