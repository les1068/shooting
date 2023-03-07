using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : PoolObject
{
    public float moveSpeed = 2.0f;  // 이동 속도

    public float dirChangeInterval = 5.0f; //방향 전환하는 시간 간격

    Transform playerTransform;  // 플레이어의 트랜스폼

    Vector2 dir;  // 이동 방향

    WaitForSeconds changeInterval;  // 코루틴용으로 미리 계산해 놓은 변수
    
    const int DirChangeCountMax = 5;       // 최대 튕기는 횟수를 기록한 상수
    int dirChangeCount = DirChangeCountMax;  // 현재 남아있는 튕길 횟수를 저장하는 변수

    int DirChangeCount
    {
        get => dirChangeCount;          // 읽는 건 마음대로
        set
        {
            dirChangeCount = value;     // 쓸때는 0 이하가 되면 특정 행동을 처리
            if (dirChangeCount <= 0)
            {
                StopAllCoroutines();    // 모든 코루틴을 정지시켜서 일정 시간간격으로 튕기는 것을 방지
            }
        }
    }
    private void Awake()
    {
        changeInterval = new WaitForSeconds(dirChangeInterval);
    }

    private void OnEnable()
    {
        if (playerTransform == null) // 없을 때만 찾기
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player"); // 태그로 찾기
            playerTransform = player.transform;
            //playerTransform = FindObjectOfType<Player>().transform;       // 타입으로 찾기
        }
        SetRandomDirection(true);       // 시작할 때 랜덤 방향 설정하기

        DirChangeCount = DirChangeCountMax;    // 튕기는 횟수 초기화

        StopAllCoroutines();            // 이전 코루틴 모두 제거
        StartCoroutine(DirChange());    // 다시 시작할 때 랜덤 방향으로
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir);  // 방향대로 이동시키기
    }

    void SetRandomDirection(bool allRandom = false)
    {
            if (!allRandom && Random.value < 0.4f)
            {
                // 완전 랜덤이 아니고 40%의 확률에 당첨이 되면 플레이어의 반대 방향으로 이동시키기
                Vector2 playerToPowerUp = transform.position - playerTransform.position;
                // 플레이어에서 파워업으로 가는 방향 벡터를 z축 기준으로 +-90도를 랜덤으로 회전
                dir = Quaternion.Euler(0, 0, Random.Range(-90.0f, 90.0f)) * playerToPowerUp;
                //Debug.Log("도망");
            }
            else
            {
                // 완전 랜덤이거나 40% 확률에 당첨되지 않았을 때
                dir = Random.insideUnitCircle;  // 반지름이 1인 원 안의 랜덤한 위치 가져오기
                                                //Debug.Log("랜덤");
            }

        dir = dir.normalized;   // 길이를 1로 만들어서 항상 동일한 속도가 되게 만들기
        DirChangeCount--;    // 튕길 대 마다 dirChangeCount 감소

    }

    IEnumerator DirChange()
    {
        while (true)
        {
            yield return changeInterval;
            SetRandomDirection();           // 랜덤하게 방향 변경하기
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (DirChangeCount > 0 && collision.gameObject.CompareTag("Border"))   // 튕길 횟수가 남아 있고 Border와 부딪쳤으면 
        {
            dir = Vector2.Reflect(dir, collision.contacts[0].normal);   // 방향 전환하기
            DirChangeCount--;                                           // 충돌할 때도  dirChangeCount 감소 
        }
    }
}