using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : Spawner
{
    override protected IEnumerator Spawn()
    {
        while (true)     // ���� �ݺ�(���ѷ���)
        {
            // �����ϰ� ������ ������Ʈ�� �������� �ڽ����� �����
            GameObject obj = Factory.Inst.GetObject(PoolObjectType.Enemy);

            Enemy enemy = obj.GetComponent<Enemy>();    // ������ ���ӿ�����Ʈ���� Enemy ������Ʈ ��������
            enemy.TargetPlayer = player;                // Enemy�� �÷��̾� ����

            enemy.transform.position = transform.position;  // ������ ��ġ�� �̵�
            float r = Random.Range(minY, maxY);             // �����ϰ� ������ ���� ���� ���ϰ�
            enemy.BaseY = transform.position.y + r;         // ���� ���� ����

            //yield return wait;
            yield return new WaitForSeconds(interval);  // ���͹���ŭ ���
        }
    }
}