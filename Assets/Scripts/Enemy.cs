using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public float speed = 1.0f;
    public float amplitude = 1; // 사인 결과값을 증폭시킬 변수(위아래 차이 결정)
    public float frequency = 1; // 사인 그래프가 한번 도는데 걸리는 시간(가로 폭 결정)
    [Range(0.1f,3.0f)] //변수 범위를 (min,max)사이로 변경시키는 슬라이더 추가


    public GameObject explosion1prefab; //적이 터지는 이펙트

    float timeElapsed = 0.0f;  //누적시간 (사인계산용)
    float baseY; //처음 등장한 위치
    // Update is called once per frame
    private void Start()
    {
        baseY = transform.position.y;
       
    }
    private void Update()
    {
        timeElapsed += Time.deltaTime * frequency;
        float x = transform.position.x - speed * Time.deltaTime; //x는 현재위치에서 약간 왼쪽으로 이동 
        float y =baseY + Mathf.Sin(timeElapsed)* amplitude; //y는 시작위치에서 sin결과값만큼 변경 
        transform.position= new Vector3(x,y,0); //구한 x,y를 이용해 높이 새로 지정
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))       //Bullet 태그를 가진 오브젝트와 충돌 했을 때 만 실행
        {
            GameObject obj = Instantiate(explosion1prefab);  //explosion이펙트 생성
            obj.transform.position = transform.position;     //충돌 지점으로 이동 시키기
            Destroy(gameObject);                             //자기자신 사라지기 
        }
    }
}
