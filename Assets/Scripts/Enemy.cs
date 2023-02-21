using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed= 5.0f;
    public float amplitude = 1;//���ΰ������ ������ų ���� (���Ʒ� ���� ����)
    public float frequency = 1; //���α׷����� �ѹ� ���µ� �ɸ��� �ð�(���� �� ����)
    float timeElapsed = 0.0f;
    float baseY;


    // Start is called before the first frame update
    private void Start()
    {
        baseY = transform.position.y;
    }

    // Update is called once per frame
    private void Update()
    {
        timeElapsed += Time.deltaTime * frequency;
        float x = transform.position.x - speed * Time.deltaTime; //x�� ������ġ���� �ణ �������� �̵� 
        float y =baseY + Mathf.Sin(timeElapsed)* amplitude; //y�� ������ġ���� sin�������ŭ ���� 
        transform.position= new Vector3(x,y,0); //���� x,y�� �̿��� ���� ���� ����
    }
}
