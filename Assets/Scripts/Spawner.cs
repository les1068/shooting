using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 오브젝트를 주기적으로 생성할 클래스
public class Spawner : MonoBehaviour
{
    //생성할 게임 오브젝트
    public GameObject spawnprefab;
    
    public float minY = -4; //생성할 위치(최소값)
    public float maxY = 4;  //생성할 위치(최댓값)
    //시간 간격
    public float interval = 5.0f;

    //WaitForSeconds wait;
    private void Start()
    {
       // Wait =new WaitForSeconds(interval); //게임도중에 interval이 변하지 않는다면 미리 만들어 두는
        
        //시작할 때 Spawn 코루틴 시작
        StartCoroutine(Spawn());
    }

   
    IEnumerator Spawn()  //"★★오브젝트를 주기적으로 생성하는 코루틴"
    {
        while(true)    //무한 루프
        {
            //생성하고 생성한 오브젝트를 스포너의 자식으로 만들기
            GameObject obj = Instantiate(spawnprefab, transform); 
            //obj.transform.position = transform.position;
            
            float r = Random.Range(minY, maxY); //랜덤하게 적용할 높이 구함
            obj.transform.Translate(Vector3.up *r); //높이 적용

            //yield return wait;
            yield return new WaitForSeconds(interval); //interval 만큼 대기

        }
    }

    private void OnDrawGizmos() //씬창에 개발용 정보를 그리는 함수
    {
        Gizmos.color = Color.green; //게임 전체적으로 색상 지정됨
        //Gizmos.color = new Color(1,0,0); //RGB 값으로 색상넣을때
        
        //스폰 지점을 선으로 긋기
        Vector3 from = transform.position + Vector3.up * minY;
        Vector3 to = transform.position + Vector3.up * maxY;
        Gizmos.DrawLine(from, to);

        Gizmos.DrawWireCube(transform.position,new Vector3(1, Mathf.Abs(maxY)+Mathf.Abs(minY)+2 , 1));

    }

}
