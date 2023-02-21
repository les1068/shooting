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
        //Destroy함수는 오브젝트를 파괴하는 함수
        Destroy(gameObject, 5.0f);
        Debug.Log("Start");
    }
    void Update()
    {
        //초당 speed의 속도로 right방향으로 움직임(로컬 좌표를 기준으로 한 방향) 
        //local좌표와 World좌표
        //local좌표 : "각 오브젝트" 별 기준으로 한 좌표계
        //World좌표 : "맵"을 기준으로 한 좌표계
        //transform.position += Time.deltaTime * speed * Vector3.right;
        //transform.position += Time.deltaTime * speed * transform.right;
        //transform.Translate(Time.deltaTime * speed * transform.right,Space.World);
        //https://nakedgang.tistory.com/52참고
        transform.Translate(Time.deltaTime * speed * Vector2.right); 
    }
}
