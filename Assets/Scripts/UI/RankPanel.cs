using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLines = null;    // UI에서 표시하는 랭킹 한줄들을 모아둔 배열 (0번째가 1등, 4번째가 5등)
    int[] highScores = null;        // 랭킹별 최고점 (0번째가 1등, 4번째가 5등)
    string[] rankerNames = null;    // 랭킹에 들어간 사람 이름  (0번째가 1등, 4번째가 5등)
    int rankCount = 5;              // 최대 랭킹 표시수
    TMP_InputField inputField;      // inputfield 컴포넌트
    const int NotUpdated = -1;      // 랭킹이 업데이트 되지 않았음을 표시하는 상수
    int updatedIndex = NotUpdated;  // 현재 업데이트 된 랭킹의 인덱스
    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();  // 컴포넌트 찾고
        inputField.onEndEdit.AddListener(OnNameInputEnd);       // 입력이 끝났을 때 실행될 함수 등록


        //inputField.onEndEdit;       // 편집이 끝난거 
        //inputField.onSubmit;        // 입력 완료가 되었을때
        //inputField.onValueChanged;  // 내가 누르면 실행되는


        rankLines = GetComponentsInChildren<RankLine>();        // 모든 랭킹 라인 다 가져오기
        rankCount = rankLines.Length;           // UI 갯수에 맞게 최대 랭킹 갯수 설정
        highScores = new int[rankCount];        // 배열 확보
        rankerNames = new string[rankCount];

        LoadRankingData();

    }

    private void Start()
    {
        inputField.gameObject.SetActive(false);     // 시작할 때 인풋 필드 안보이게 만들기
        Player player = FindObjectOfType<Player>();
        player.onDie += RankUpdate;                  // 플레이어가 죽었을 때 랭크 업데이트 시도
    }
    private void OnNameInputEnd(string text)  // 이름 입력이 완료되었을 때 실행되는 함수
    {
        rankerNames[updatedIndex] = text;            // 입력받은 텍스트를 해당 랭커의 이름으로 지정
        inputField.gameObject.SetActive(false);      // 입력 완료되었으니 다시 안보이게 만들기
        SaveRankingData();       // 새로 저장하고
        RefreshRankLines();      // UI 갱신
    }
    private void RankUpdate(Player player)  // 랭킹 업데이트 하는 함수
    { 
        int newScore = player.Score;           // 새 점수
        for (int i = 0; i < rankCount; i++)    // 랭킹 1등부터 5등까지 확인
        {
            if (highScores[i] < newScore)      // 새 점수가 현재 랭킹보다 높으면
            {
                for(int j = rankCount-1; j>i; j--)  // 현재 랭킹부터 한칸씩 아래로 밀기
                {
                    highScores[j] = highScores[j-1];
                    rankerNames[j] = rankerNames[j-1];
                }
                highScores[i] = newScore;        // 새 점수를 현재 랭킹에 끼워 넣기
                rankerNames[i] = ""; 
                updatedIndex = i;                // 랭킹 업데이트된 인덱스 기억

                Vector3 newPos = inputField.transform.position;
                newPos.y = rankLines[i].transform.position.y;
                inputField.transform.position = newPos;     // 인풋 필드의 위치를 랭크라인과 같게 만들기
                //inputField.ActivateInputField();
                inputField.gameObject.SetActive(true); // 인풋 필드 활성화
                break;   // 랭킹 삽입 될 곳을 찾았으니 중지
            }
        } 
    }

    void SaveRankingData()
    {
        //PlayerPrefs.SetInt("Score", 10);    // 컴퓨터에 Score라는 이름으로 10을 저장

        //SaveData saveData = new SaveData();            
        SaveData saveData = new(); // 윗줄과 같은 코드(타입을 알 수 있기 때문에 생략한것)

        saveData.rankerNames = rankerNames;  // 생성한 인스턴스에 데이터 기록
        saveData.highScores = highScores;    
        
        string json = JsonUtility.ToJson(saveData);   // saveData에 있는 내용을 json 양식으로 설정된 string으로 변경
        //Debug.Log(json);

        string path = $"{Application.dataPath}/Save/";  // 저장될 경로 구하기(에디터에서는 Assets폴더)
        if(!Directory.Exists(path))                     // path에 저장된 폴더가 있는지 확인
        {
            Directory.CreateDirectory(path);            // 폴더가 없으면 그 폴더를 만든다.
        }
        string fullPath = $"{path}Save.json";           // 전체 경로 = 폴더 + (Save)파일이름 + (.json)파일확장자
        File.WriteAllText(fullPath, json);              // fullPath에 json내용 파일로 기록하기

    }
    bool LoadRankingData()
    {
        bool result = false;

        string path = $"{Application.dataPath}/Save/";              // 경로
        string fullPath = $"{path}Save.json";                       // 전체 경로

        result = Directory.Exists(path) && File.Exists(fullPath);   // 폴더와 파일이 있는지 확인

        if (result)
        {
            // 폴더와 파일이 있으면 읽기
            string json = File.ReadAllText(fullPath);                   // 텍스트 파일 읽기
            SaveData loadData = JsonUtility.FromJson<SaveData>(json);   // json문자열을 파싱해서 SaveData에 넣기
            highScores = loadData.highScores;            // 실제로 최고 점수 넣기
            rankerNames = loadData.rankerNames;          // 이름 넣기
        }
        else
        {
            // 파일에서 못읽었으면 디폴트 값 주기
            int size = rankLines.Length;
            for (int i = 0; i < rankCount; i++)
            {
                int resultScore = 1;
                for (int j = rankCount - i; j > 0; j--)
                {
                    resultScore *= 10;
                }
                highScores[i] = resultScore; // 10만, 만, 천, 백, 십

                char temp = 'A';
                temp = (char)((byte)temp + i);
                rankerNames[i] = $"{temp}{temp}{temp}";     // AAA,BBB,CCC,DDD,EEE
            }
        }
        RefreshRankLines();     // 로딩이 되었으니 RankLines 갱신
        return result;

    }
    void RefreshRankLines()    // 랭크 라인 화면 업데이트
    {
        for (int i = 0; i < rankLines.Length; i++)
        {
            rankLines[i].SetData(rankerNames[i], highScores[i]);
        }
    }
}
