using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

public class ScoreBoard : MonoBehaviour
{
    public float scoreUpSpeed = 50.0f;      //점수가 올라가는 속도
    float currentScore = 0;                 //UI에서 보이는 점수
    int tagetScore = 0;                     //실제 플레이어가 가지는 점수
    TextMeshProUGUI score;                 //점수를 출력할 UI text

    private void Awake()
    {
        Transform child = transform.GetChild(1);        // 두번째 자식 가져오기
        score = child.GetComponent<TextMeshProUGUI>();  // 두번째 자식의 컴포넌트 가져오기
        
    }

    private void Start()
    {
        score.text = "0";
        Player player = FindObjectOfType<Player>();
        player.onScoreChange += ScoreUpdate;           // player의 onScoreChange 델리게이트가 실행될 때 ScoreUpdate를 실행해라

        score.text = currentScore.ToString();          // 초기값 설정
    }
    private void Update()
    {
        if(currentScore< tagetScore)                       // CurrentScore가 Targetscore보다 작으면
        {  
            currentScore += Time.deltaTime * scoreUpSpeed; // CurrentScore를 초당 scoreUpSpeed씩 증가시킨다.
            score.text = $"{currentScore:f0}";             // UI에 출력할 때 소수점은 0개씩 출력한다.(소수점 출력안함)
        }
    }
    void ScoreUpdate(int newScore)                         // 목표점수를 새점수로 설정
    {
        tagetScore = newScore;
    }
}
