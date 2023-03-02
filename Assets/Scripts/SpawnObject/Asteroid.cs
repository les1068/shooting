using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : PoolObject
{
    public float moveSpeed = 1.0f;    // 이동 속도 (초당 이동 거리)
    public float rotateSpeed = 30.0f; // 회전 속도 (초당 회전 각도(도 : degree))

    Vector3 dir = Vector3.left;


    Player player = null;
    public Player TargetPlayer
    {
        set
        {
            if (player == null)     // player가 null일때만 설정
            {
                player = value;
            }
        }
    }

    // 오일러 앵글을 안쓰는이유는 짐벌락때문에 안쓴다
    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir, Space.World);  // 월드 좌표로 초당 moveSpeed속도로 Vector3.left방향으로 움직임
      
        //transform.Rotate(0, 0, Time.deltaTime * -rotateSpeed); // 시계방향으로 초당 rotateSpeed씩 회전
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);    // 반시계방향으로 초당 rotateSpeed씩 회전


        //Debug.Log(transform.rotation);
    }




}
