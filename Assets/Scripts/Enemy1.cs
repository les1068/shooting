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
        baseY = transform.position.y;        //�����Ҷ� ������ ���� 
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * speed * -transform.right);
        //transform.Translate(Time.deltaTime * speed * Vector3.left);

        //�� ���ӿ�����Ʈ�� y��ġ�� �����̻� �ö󰡰ų� �������� ���� ���� 
        transform.Translate(Time.deltaTime * speed * dir * transform.up);
        if ((transform.position.y > baseY + height)|| (transform.position.y < baseY - height))
        {
            dir = dir * -1; // dir *= -1.0f;
        }
        //��������
        // && (and��� ����) : �� ���� ��� true�϶��� true�̴�.
        // true && false = false
        // || (or��� ����) : �� ���� �ϳ��� true�� true�̴�.
        // true || false = true
    }
}
