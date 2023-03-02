using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAsteroid : Spawner
{
    Transform destination;

    private void Awake()
    {
        destination = transform.GetChild(0);
    }

    protected override IEnumerator Spawn()
    {
        while (true)     // ���� �ݺ�(���ѷ���)
        {
            // �����ϰ� ������ ������Ʈ�� �������� �ڽ����� �����
            GameObject obj = Factory.Inst.GetObject(PoolObjectType.Asteroid);

            Asteroid asteroid = obj.GetComponent<Asteroid>();   // ������ ���ӿ�����Ʈ���� asteroid ������Ʈ ��������
            asteroid.TargetPlayer = player;                     // asteroid�� �÷��̾� ����

            asteroid.transform.position = transform.position;   // ������ ��ġ�� �̵�
            float r = Random.Range(minY, maxY);                 // �����ϰ� ������ ���� ���� ���ϰ�
            asteroid.transform.Translate(r * Vector3.up);

            yield return new WaitForSeconds(interval);  // ���͹���ŭ ���
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // ���� ������ ť��� �׸���
        Gizmos.color = Color.blue;
        if (destination == null)
        {
            destination = transform.GetChild(0);
        }
        Gizmos.DrawWireCube(destination.position,
            new Vector3(1, Mathf.Abs(maxY) + Mathf.Abs(minY) + 2, 1));
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        // ���� ������ ������ �߱�
        Gizmos.color = Color.red;
        if (destination == null)
        {
            destination = transform.GetChild(0);
        }
        Vector3 from = destination.position + Vector3.up * minY;
        Vector3 to = destination.position + Vector3.up * maxY;
        Gizmos.DrawLine(from, to);
    }
}