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
    bool isCrushed = false;                // 파괴되었는지 여부, truie면 파괴된 상황, false면 멀

    Player player = null;  // 플레이어에 대한 참조

    public Player TargetPlayer  // player에 처음 한번만 값을 설정 가능한 프로퍼티. 쓰기 전용. 자신과 상속받은 클래스에서는 읽기도 가능
    {
        protected get => player;
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
        isCrushed = false;          //다시 살아난 것 표시
        hitPoint = MaxHitPoint;  // hp 최대치로 채우기
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Attacked();
        }
    }
    protected void Attacked() // 공격당하면 무조건 실행해야하는 일들
    {
        hitPoint--;         // 맞으면 hitPoint 감소
        OnHit();
        if (hitPoint < 1)   // hitPoint가 1아래로 내려가면
        {
            Crush();      // 파괴
        }
    }
    protected virtual void OnHit()  // 공격 당했을 때 상속받은 클래스별로 해야하는 일들
    {

    }
    protected void Crush()  // 부서지면 무조건 실행해야 할 일들 처리
    {
        if (!isCrushed)
        {
            isCrushed = true;      // 파괴 되었다고 표시

            OnCrush();           // 클래스별 별도 파괴 처리

            GameObject obj = Factory.Inst.GetObject(destroyEffect);  // 터지는 이펙트 생성
            obj.transform.position = transform.position; // 이펙트 위치 변경

            gameObject.SetActive(false);  // 비활성화
        }
    }
    protected virtual void OnCrush()  // 부서질때 상속받은 클래스별로 따로 처리할 일을 override해서 쓰기
    {
            player?.AddScore(score);  // 점수 추가
            
    }
    
}
