using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : PoolObject
{
    [Header("Enemy Base--------")]

    public int MaxHitPoint = 1;         // ���� �ִ� HP
    public float moveSpeed = 1.0f;      // ���� �̵� �ӵ�(�ʴ� �̵� �Ÿ�)
    public int score = 10;              // �ı����� ���� ����
    public PoolObjectType destroyEffect = PoolObjectType.Explosion;  //�ı� ����Ʈ

    int hitPoint = 1;                   // ���� HP
    bool isCrush = true;                // �ı��Ǿ����� ����

    Player player = null;  // �÷��̾ ���� ����

    public Player TargetPlayer  // player�� ó�� �ѹ��� ���� ���� ������ ������Ƽ. ���� ����.
    {
        set
        {
            if (player == null)     // player�� null�϶��� ����
            {
                player = value;
            }
        }
    }
    protected virtual void OnEnable()
    {
        isCrush = true;          //�ٽ� ��Ƴ� �� ǥ��
        hitPoint = MaxHitPoint;  // hp �ִ�ġ�� ä���
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            OnHit();
        }
    }
    protected virtual void OnHit()
    {
        hitPoint--;         // ������ hitPoint ����
        if (hitPoint < 1)   // hitPoint�� 1�Ʒ��� ��������
        {
            OnCrush();      // �ı�
        }
    }
    protected virtual void OnCrush()
    {
        if (isCrush)
        {
            isCrush = false;      // �ı� �Ǿ��ٰ� ǥ��
            player.AddScore(score);  // ���� �߰�

            GameObject obj = Factory.Inst.GetObject(destroyEffect);  // ������ ����Ʈ ����
            obj.transform.position = transform.position; // ����Ʈ ��ġ ����

            gameObject.SetActive(false);  // ��Ȱ��ȭ
        }
    }
}
