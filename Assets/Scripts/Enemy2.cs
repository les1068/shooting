using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public float speed = 5.0f;
    public float bounceTime = 1.0f;
    float currentTime = 0.0f;
    Vector2 dir;
    private void Start()
    {
        dir = new Vector2(-1, 1);
        dir.Normalize();                //���⸸ �����.(=���̰� 1�̴�.)
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime; //�ð� �����ϱ�
        if(currentTime > bounceTime)   //������ �ð� ������
        {
            currentTime= 0.0f;         //�ð� �ʱ�ȭ�ϰ�
            dir.y = -dir.y;            //���������
        }
        transform.Translate(Time.deltaTime* speed*dir);
        
    }
}
