using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : PoolObject
{
    [Header("Enemy Base--------")]

    public int MaxHitPoint = 1;         // 적의 최대 HP
    public float moveSpeed = 1.0f;      // 실제 이동 속도(초당 이동 거리)
    public int score = 10;              // 파괴했을 때의 점수
    public PoolObjectType destroyEffect = PoolObjectType.Explosion;  //파괴 이펙트

    int hitPoint = 1;                   // 적의 HP
    bool isCrush = true;                // 파괴되었는지 여부

    Player player = null;  // 플레이어에 대한 참조

    public Player TargetPlayer  // player에 처음 한번만 값을 설정 가능한 프로퍼티. 쓰기 전용.
    {
        set
        {
            if (player == null)     // player가 null일때만 설정
            {
                player = value;
            }
        }
    }
    protected virtual void OnEnable()
    {
        isCrush = true;          //다시 살아난 것 표시
        hitPoint = MaxHitPoint;  // hp 최대치로 채우기
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
        hitPoint--;         // 맞으면 hitPoint 감소
        if (hitPoint < 1)   // hitPoint가 1아래로 내려가면
        {
            OnCrush();      // 파괴
        }
    }
    protected virtual void OnCrush()
    {
        if (isCrush)
        {
            isCrush = false;      // 파괴 되었다고 표시
            player.AddScore(score);  // 점수 추가

            GameObject obj = Factory.Inst.GetObject(destroyEffect);  // 터지는 이펙트 생성
            obj.transform.position = transform.position; // 이펙트 위치 변경

            gameObject.SetActive(false);  // 비활성화
        }
    }
}
