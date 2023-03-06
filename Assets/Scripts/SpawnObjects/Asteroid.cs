using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : AsteroidBase
{
    [Header("큰 운석 데이터 ---------")]
    public PoolObjectType childType = PoolObjectType.AsteroidSmall; // 파괴 될때 생성할 오브젝트의 종류 
    
    public float minLifeTime = 4.0f; // 최소 수명
    public float maxLifeTime = 7.0f; // 최대 수명

    [Range(0f,1f)]
    public float criticalChance = 0.5f; // 크리티컬이 터질 확률
    public int criticalSplitcount = 20; // 크리티컬이 터졌을 때 나올 작은 운석 갯수

    int splitcount = 3;  // 파괴 될 때 생성할 오브젝트의 갯수

    bool isSelfCrush = false;  // 자폭 여부 표시 true면 자폭 false면 플레이어가 터트린 것
    Animator anim;           // 찾아놓을 컴포넌트
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>(); 
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        isSelfCrush = false;
        float lifeTime = Random.Range(minLifeTime, maxLifeTime);
        //Debug.Log($"{lifeTime}초뒤 자폭");

        StopAllCoroutines();                      // 이전 코루틴 제거
        StartCoroutine(SelfCrush(lifeTime));      // 새 자폭 코루틴 시작
    }
    IEnumerator SelfCrush(float lifeTime)     // 자폭용 코루틴  // lifeTime 자폭시간
    {
        yield return new WaitForSeconds(lifeTime - 1);
        anim.SetTrigger("SelfCrush");

        yield return new WaitForSeconds(1);
        isSelfCrush= true;
        Crush();
    }

    protected override void OnCrush()   // 터질때 일어나는
    {
        if(!isSelfCrush)
        {
            TargetPlayer?.AddScore(score);     // 자폭 상황이 아닐 때만 점수 추가
        }

        float random = Random.Range(0.0f, 1.0f);
        if (random < criticalChance)
        {
            splitcount = criticalSplitcount;
        }
        else
        {
            splitcount = Random.Range(3, 8);   // 3~7개 생성
        }

        float angleGap = 360.0f / splitcount;   // 작은 운석간의 사이각 계산
        float seed = Random.Range(0.0f, 360.0f);  // 처음 적용할 오차 랜덤으로 구하기

        for(int i=0; i<splitcount; i++)           // splitcount만큼 반복
        {
            GameObject obj =  Factory.Inst.GetObject(childType);   // 작은 운석 생성
            obj.transform.position = transform.position;           // 위치는 우선 큰 운석 위치로
            AsteroidBase small = obj.GetComponent<AsteroidBase>();
            small.TargetPlayer = TargetPlayer;                     // 점수 추가를 위해 플레이어 설정

            // UP(0,1,0) 벡터를 일단 z죽을 기준으로 seed만큼 회전시키고 추가로
            // angleGap * i만큼 더 회전 시키고 small방향으로 지정하는 코드
            small.Direction = Quaternion.Euler(0,0, seed + angleGap * i ) * Vector3.up; // 작은 운석 방향 지정하기 ((회전) * 벡터))
        }
    }
}
