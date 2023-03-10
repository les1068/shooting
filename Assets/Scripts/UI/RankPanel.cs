using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLines = null;   // UI���� ǥ���ϴ� ��ŷ ���ٵ��� ��Ƶ� �迭 (0��°�� 1��, 4��°�� 5��)
    int[] highScores = null;       // ��ŷ�� �ְ��� (0��°�� 1��, 4��°�� 5��)
    string[] rankerNames = null;   // ��ŷ�� �� ��� �̸�  (0��°�� 1��, 4��°�� 5��)

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();  // ��� ��ŷ ���� �� ��������

        int size = rankLines.Length;         
        highScores = new int[size];     // �迭 Ȯ��
        rankerNames = new string[size];

        LoadRankingData();               
        
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

        string path = $"{Application.dataPath}/Save/";    // ���
        string fullPath = $"{path}Save.json";             // ��ü ���

        result = Directory.Exists(path) && File.Exists(fullPath);  // ������ ������ �ִ��� Ȯ��        

        if (!result)
        {
            // ������ ������ ������ �б�
            string json = File.ReadAllText(fullPath);                 // �ؽ�Ʈ ���� �б�
            SaveData loadData = JsonUtility.FromJson<SaveData>(json); // json���ڿ��� �Ľ��ؼ� SaveData�� �ֱ�
            highScores = loadData.highScores;                         // ������ �ְ� ���� �ֱ�
            rankerNames = loadData.rankerNames;                       // �̸� �ֱ�
        }
        else
        {
            int size = rankLines.Length;
            for (int i = 0; i < size; i++)    // ���Ͽ��� �� �о����� ����Ʈ �� �ֱ�
            {
                int resultScore = 1;
                for (int j = size - i; j > 0; j--)
                {
                    resultScore *= 10;
                }
                highScores[i] = resultScore;  // 10��, ��, õ, ��, ��

                char temp = 'A';
                temp = (char)((byte)temp + i);
                rankerNames[i] = $"{temp}{temp}{temp}";    // AAA,BBB,CCC,DDD,EEE
            }
        }
        RefreshRankLines();    // �ε��� �Ǿ����� RankLines ����
        return result;         
    }
    void RefreshRankLines()
    {
        for (int i = 0; i < rankLines.Length; i++)
        {
            rankLines[i].SetData(rankerNames[i], highScores[i]);
        }
    }
}
