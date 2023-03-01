using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMover : MonoBehaviour
{
    // ���ʿ��� ���������� ������
    // ȭ�� ���� ����� ������ ȭ�� ������ �����̱�
    // ������ ȭ�� ������ ���� �����̴� ���� ������ ����
    // ������ ȭ�� ������ ���� �� �Ʒ��ε� �����ϰ� �����̱�
    public float moveSpeed = 10.0f;
    public float minRightEnd = 20.0f;
    public float maxRightEnd = 60.0f;
    public float minHeight = -4.0f;
    public float mixHeight = -1.0f;

    float moveTriggerPosition = -16.0f;

    private void Awake()
    {
        moveTriggerPosition = transform.position.x;   // ó�� ���� �Ǿ��� ���� ��ġ ����ϱ�
        
    }
    private void Update()
    {
        transform.Translate(Time.deltaTime* moveSpeed* -transform.right);
        if(transform.position.x < moveTriggerPosition)
        {
            Vector3 newPos = new Vector3(
                Random.Range(minRightEnd,maxRightEnd),     // x ���ϰ�
                Random.Range(minHeight, mixHeight));       // y ���ϰ�, z�� ��ŵ ���� (��ŵ�ϸ� 0)
            transform.position = newPos;           // �� ��ġ�� �̵���Ű��
        }
    }
}
