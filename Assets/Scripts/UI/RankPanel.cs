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
    RankLine[] rankLines = null;    // UI���� ǥ���ϴ� ��ŷ ���ٵ��� ��Ƶ� �迭 (0��°�� 1��, 4��°�� 5��)
    int[] highScores = null;        // ��ŷ�� �ְ��� (0��°�� 1��, 4��°�� 5��)
    string[] rankerNames = null;    // ��ŷ�� �� ��� �̸�  (0��°�� 1��, 4��°�� 5��)
    int rankCount = 5;              // �ִ� ��ŷ ǥ�ü�
    TMP_InputField inputField;      // inputfield ������Ʈ
    const int NotUpdated = -1;      // ��ŷ�� ������Ʈ ���� �ʾ����� ǥ���ϴ� ���
    int updatedIndex = NotUpdated;  // ���� ������Ʈ �� ��ŷ�� �ε���
    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();  // ������Ʈ ã��
        inputField.onEndEdit.AddListener(OnNameInputEnd);       // �Է��� ������ �� ����� �Լ� ���


        //inputField.onEndEdit;       // ������ ������ 
        //inputField.onSubmit;        // �Է� �Ϸᰡ �Ǿ�����
        //inputField.onValueChanged;  // ���� ������ ����Ǵ�


        rankLines = GetComponentsInChildren<RankLine>();        // ��� ��ŷ ���� �� ��������
        rankCount = rankLines.Length;           // UI ������ �°� �ִ� ��ŷ ���� ����
        highScores = new int[rankCount];        // �迭 Ȯ��
        rankerNames = new string[rankCount];

        LoadRankingData();

    }

    private void Start()
    {
        inputField.gameObject.SetActive(false);     // ������ �� ��ǲ �ʵ� �Ⱥ��̰� �����
        Player player = FindObjectOfType<Player>();
        player.onDie += RankUpdate;                  // �÷��̾ �׾��� �� ��ũ ������Ʈ �õ�
    }
    private void OnNameInputEnd(string text)  // �̸� �Է��� �Ϸ�Ǿ��� �� ����Ǵ� �Լ�
    {
        rankerNames[updatedIndex] = text;            // �Է¹��� �ؽ�Ʈ�� �ش� ��Ŀ�� �̸����� ����
        inputField.gameObject.SetActive(false);      // �Է� �Ϸ�Ǿ����� �ٽ� �Ⱥ��̰� �����
        SaveRankingData();       // ���� �����ϰ�
        RefreshRankLines();      // UI ����
    }
    private void RankUpdate(Player player)  // ��ŷ ������Ʈ �ϴ� �Լ�
    { 
        int newScore = player.Score;           // �� ����
        for (int i = 0; i < rankCount; i++)    // ��ŷ 1����� 5����� Ȯ��
        {
            if (highScores[i] < newScore)      // �� ������ ���� ��ŷ���� ������
            {
                for(int j = rankCount-1; j>i; j--)  // ���� ��ŷ���� ��ĭ�� �Ʒ��� �б�
                {
                    highScores[j] = highScores[j-1];
                    rankerNames[j] = rankerNames[j-1];
                }
                highScores[i] = newScore;        // �� ������ ���� ��ŷ�� ���� �ֱ�
                rankerNames[i] = ""; 
                updatedIndex = i;                // ��ŷ ������Ʈ�� �ε��� ���

                Vector3 newPos = inputField.transform.position;
                newPos.y = rankLines[i].transform.position.y;
                inputField.transform.position = newPos;     // ��ǲ �ʵ��� ��ġ�� ��ũ���ΰ� ���� �����
                //inputField.ActivateInputField();
                inputField.gameObject.SetActive(true); // ��ǲ �ʵ� Ȱ��ȭ
                break;   // ��ŷ ���� �� ���� ã������ ����
            }
        } 
    }

    void SaveRankingData()
    {
        //PlayerPrefs.SetInt("Score", 10);    // ��ǻ�Ϳ� Score��� �̸����� 10�� ����

        //SaveData saveData = new SaveData();            
        SaveData saveData = new(); // ���ٰ� ���� �ڵ�(Ÿ���� �� �� �ֱ� ������ �����Ѱ�)

        saveData.rankerNames = rankerNames;  // ������ �ν��Ͻ��� ������ ���
        saveData.highScores = highScores;    
        
        string json = JsonUtility.ToJson(saveData);   // saveData�� �ִ� ������ json ������� ������ string���� ����
        //Debug.Log(json);

        string path = $"{Application.dataPath}/Save/";  // ����� ��� ���ϱ�(�����Ϳ����� Assets����)
        if(!Directory.Exists(path))                     // path�� ����� ������ �ִ��� Ȯ��
        {
            Directory.CreateDirectory(path);            // ������ ������ �� ������ �����.
        }
        string fullPath = $"{path}Save.json";           // ��ü ��� = ���� + (Save)�����̸� + (.json)����Ȯ����
        File.WriteAllText(fullPath, json);              // fullPath�� json���� ���Ϸ� ����ϱ�

    }
    bool LoadRankingData()
    {
        bool result = false;

        string path = $"{Application.dataPath}/Save/";              // ���
        string fullPath = $"{path}Save.json";                       // ��ü ���

        result = Directory.Exists(path) && File.Exists(fullPath);   // ������ ������ �ִ��� Ȯ��

        if (result)
        {
            // ������ ������ ������ �б�
            string json = File.ReadAllText(fullPath);                   // �ؽ�Ʈ ���� �б�
            SaveData loadData = JsonUtility.FromJson<SaveData>(json);   // json���ڿ��� �Ľ��ؼ� SaveData�� �ֱ�
            highScores = loadData.highScores;            // ������ �ְ� ���� �ֱ�
            rankerNames = loadData.rankerNames;          // �̸� �ֱ�
        }
        else
        {
            // ���Ͽ��� ���о����� ����Ʈ �� �ֱ�
            int size = rankLines.Length;
            for (int i = 0; i < rankCount; i++)
            {
                int resultScore = 1;
                for (int j = rankCount - i; j > 0; j--)
                {
                    resultScore *= 10;
                }
                highScores[i] = resultScore; // 10��, ��, õ, ��, ��

                char temp = 'A';
                temp = (char)((byte)temp + i);
                rankerNames[i] = $"{temp}{temp}{temp}";     // AAA,BBB,CCC,DDD,EEE
            }
        }
        RefreshRankLines();     // �ε��� �Ǿ����� RankLines ����
        return result;

    }
    void RefreshRankLines()    // ��ũ ���� ȭ�� ������Ʈ
    {
        for (int i = 0; i < rankLines.Length; i++)
        {
            rankLines[i].SetData(rankerNames[i], highScores[i]);
        }
    }
}
