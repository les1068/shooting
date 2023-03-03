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
    bool isCrushed = false;                // �ı��Ǿ����� ����, truie�� �ı��� ��Ȳ, false�� ��

    Player player = null;  // �÷��̾ ���� ����

    public Player TargetPlayer  // player�� ó�� �ѹ��� ���� ���� ������ ������Ƽ. ���� ����. �ڽŰ� ��ӹ��� Ŭ���������� �б⵵ ����
    {
        protected get => player;
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
        isCrushed = false;          //�ٽ� ��Ƴ� �� ǥ��
        hitPoint = MaxHitPoint;  // hp �ִ�ġ�� ä���
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Attacked();
        }
    }
    protected void Attacked() // ���ݴ��ϸ� ������ �����ؾ��ϴ� �ϵ�
    {
        hitPoint--;         // ������ hitPoint ����
        OnHit();
        if (hitPoint < 1)   // hitPoint�� 1�Ʒ��� ��������
        {
            Crush();      // �ı�
        }
    }
    protected virtual void OnHit()  // ���� ������ �� ��ӹ��� Ŭ�������� �ؾ��ϴ� �ϵ�
    {

    }
    protected void Crush()  // �μ����� ������ �����ؾ� �� �ϵ� ó��
    {
        if (!isCrushed)
        {
            isCrushed = true;      // �ı� �Ǿ��ٰ� ǥ��

            OnCrush();           // Ŭ������ ���� �ı� ó��

            GameObject obj = Factory.Inst.GetObject(destroyEffect);  // ������ ����Ʈ ����
            obj.transform.position = transform.position; // ����Ʈ ��ġ ����

            gameObject.SetActive(false);  // ��Ȱ��ȭ
        }
    }
    protected virtual void OnCrush()  // �μ����� ��ӹ��� Ŭ�������� ���� ó���� ���� override�ؼ� ����
    {
            player?.AddScore(score);  // ���� �߰�
            
    }
    
}
