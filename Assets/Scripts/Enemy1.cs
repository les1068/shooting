using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{

    public float speed = 5.0f;

    float baseY;
    float dir = 1.0f;
    public float height = 3.0f;

    // Start is called before the first frame update
    private void Start()
    {
        baseY = transform.position.y;        //시작할때 시작한 높이 
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * speed * -transform.right);
        //transform.Translate(Time.deltaTime * speed * Vector3.left);

        //이 게임오브젝트의 y위치가 일정이상 올라가거나 내려가면 방향 변경 
        transform.Translate(Time.deltaTime * speed * dir * transform.up);
        if ((transform.position.y > baseY + height)|| (transform.position.y < baseY - height))
        {
            dir = dir * -1; // dir *= -1.0f;
        }
        //논리연산자
        // && (and라고 읽음) : 양 변이 모두 true일때만 true이다.
        // true && false = false
        // || (or라고 읽음) : 양 변중 하나만 true면 true이다.
        // true || false = true
    }
}
