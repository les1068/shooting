using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public float speed = 1.0f;
    public float amplitude = 1; // ���� ������� ������ų ����(���Ʒ� ���� ����)
    public float frequency = 1; // ���� �׷����� �ѹ� ���µ� �ɸ��� �ð�(���� �� ����)
    [Range(0.1f,3.0f)] //���� ������ (min,max)���̷� �����Ű�� �����̴� �߰�


    public GameObject explosion1prefab; //���� ������ ����Ʈ

    float timeElapsed = 0.0f;  //�����ð� (���ΰ���)
    float baseY; //ó�� ������ ��ġ
    // Update is called once per frame
    private void Start()
    {
        baseY = transform.position.y;
       
    }
    private void Update()
    {
        timeElapsed += Time.deltaTime * frequency;
        float x = transform.position.x - speed * Time.deltaTime; //x�� ������ġ���� �ణ �������� �̵� 
        float y =baseY + Mathf.Sin(timeElapsed)* amplitude; //y�� ������ġ���� sin�������ŭ ���� 
        transform.position= new Vector3(x,y,0); //���� x,y�� �̿��� ���� ���� ����
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))       //Bullet �±׸� ���� ������Ʈ�� �浹 ���� �� �� ����
        {
            GameObject obj = Instantiate(explosion1prefab);  //explosion����Ʈ ����
            obj.transform.position = transform.position;     //�浹 �������� �̵� ��Ű��
            Destroy(gameObject);                             //�ڱ��ڽ� ������� 
        }
    }
}
