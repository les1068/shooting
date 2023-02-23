using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 오브젝트를 주기적으로 생성할 클래스
public class Spawner : MonoBehaviour
{
    public GameObject spawnPrefab;    // 생성할 게임 오브젝트

    public float minY = -4;    // 생성할 위치(최소값)

    public float maxY = 4;  // 생성할 위치(최대값)

    public float interval = 1.0f;   // 생성 시간 간격
    //WaitForSeconds wait;

    Player player = null;  // 게임 내의 플레이어에 대한 참조

    private void Start()
    {
        //wait = new WaitForSeconds(interval);  // 게임 실행 도중에 interval이 변하지 않는다면 미리 만들어 두는 것이 좋다.

        player = FindObjectOfType<Player>();    // 플레이어를 미리 찾아 놓기
                
        StartCoroutine(Spawn());    // 시작할 때 Spawn 코루틴 시작
    }


    IEnumerator Spawn()  // 오브젝트를 주기적으로 생성하는 코루틴
    {
        while(true)     // 무한 반복(무한루프)
        {
            // 생성하고 생성한 오브젝트를 스포너의 자식으로 만들기
            GameObject obj = Instantiate(spawnPrefab, transform);
            float r = Random.Range(minY, maxY);         // 랜덤하게 적용할 높이 구하고
            obj.transform.Translate(Vector3.up * r);    // 높이 적용
            
            Enemy enemy = obj.GetComponent<Enemy>();    // 생성한 게임오브젝트에서 Enemy 컴포넌트 가져오기
            enemy.TargetPlayer = player;                // Enemy에 플레이어 설정

            //yield return wait;
            yield return new WaitForSeconds(interval);  // 인터벌만큼 대기
        }
    }

    private void OnDrawGizmos()  // 씬창에 개발용 정보를 그리는 함수
    {
        Gizmos.color = Color.green;             // 게임 전체적으로 색상 지정됨
        // Gizmos.color = new Color(0, 1, 0);   // rgb값으로 색상을 만들 수도 있다.
        
        // 스폰 영역을 큐브로 그리기
        Gizmos.DrawWireCube(transform.position, 
            new Vector3(1, Mathf.Abs(maxY) + Mathf.Abs(minY) + 2, 1));
    }

    private void OnDrawGizmosSelected()  // 씬창에 개발용 정보를 그리는 함수(선택된 게임 오브젝트만 그릴때)
    {
        Gizmos.color = Color.red;

        // 스폰 지점을 선으로 긋기
        Vector3 from = transform.position + Vector3.up * minY;
        Vector3 to = transform.position + Vector3.up * maxY;
        Gizmos.DrawLine(from, to);
    }
}
