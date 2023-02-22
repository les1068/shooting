using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    //명중 이펙트
    public GameObject hitprefab;
    public float speed = 10.0f;
    void Start()
    {
        Destroy(gameObject, 5.0f); //Destroy함수는 오브젝트를 파괴하는 함수 (5초뒤에)
        //Debug.Log("Start");
    }
    void Update()
    {
        //초당 speed의 속도로 right방향으로 움직임(로컬 좌표를 기준으로 한 방향) 
        //local좌표와 World좌표
        //local좌표 : "각 오브젝트" 별 기준으로 함 /좌표계 로컬좌표로 보는게 더 편함
        //World좌표 : "맵"을 기준으로 한 좌표계
        //transform.position += Time.deltaTime * speed * Vector3.right;
        //transform.position += Time.deltaTime * speed * transform.right;
        //transform.Translate(Time.deltaTime * speed * transform.right,Space.World);
        //https://nakedgang.tistory.com/52참고
        transform.Translate(Time.deltaTime * speed * Vector2.right); 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy")) //부딪힌 게임오브젝트의 태그가 "Enemy"일때 처리
        {
            
            Debug.Log($"총알이 {collision.gameObject.name}과 충돌");
            
            GameObject obj = Instantiate(hitprefab);                     //hit 이펙트 생성
            obj.transform.position = collision.contacts[0].point;        //충돌 지점으로 이동 시키기
            Destroy(gameObject);                                         //자기자신 사라지기

            //if(collision.gameObject.tag =="Enemy") // 절대로 하지말것 더 느리고 메모리 많이 씀
        }
    }
}
