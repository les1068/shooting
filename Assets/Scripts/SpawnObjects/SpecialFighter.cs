using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFighter : EnemyBase
{
    protected override void OnEnable()
    {
        base.OnEnable();  // ���� �ʱ�ȭ �۾� ����

        // ó���� ������ �����̰� ��ٸ��ٰ� �ٽ� ������ �����δ�.  
    }
    protected override void OnCrush()
    {
        base.OnCrush(); // ���� ����ó��
        GameObject obj =  Factory.Inst.GetObject(PoolObjectType.PowerUp); // �Ŀ��� ������ ����
        obj.transform.position = transform.position;    // ���� �� ��ġ�� �ű��
    }
}
