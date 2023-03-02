using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : PoolObject
{
    public float moveSpeed = 1.0f;    // �̵� �ӵ� (�ʴ� �̵� �Ÿ�)
    public float rotateSpeed = 30.0f; // ȸ�� �ӵ� (�ʴ� ȸ�� ����(�� : degree))

    Vector3 dir = Vector3.left;


    Player player = null;
    public Player TargetPlayer
    {
        set
        {
            if (player == null)     // player�� null�϶��� ����
            {
                player = value;
            }
        }
    }

    // ���Ϸ� �ޱ��� �Ⱦ��������� ������������ �Ⱦ���
    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir, Space.World);  // ���� ��ǥ�� �ʴ� moveSpeed�ӵ��� Vector3.left�������� ������
      
        //transform.Rotate(0, 0, Time.deltaTime * -rotateSpeed); // �ð�������� �ʴ� rotateSpeed�� ȸ��
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);    // �ݽð�������� �ʴ� rotateSpeed�� ȸ��


        //Debug.Log(transform.rotation);
    }




}
