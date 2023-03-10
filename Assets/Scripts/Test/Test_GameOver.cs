using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class Test_GameOver : Test_Base
{
    // 1���� ������ "/Assets/Save/TestSave.json"�� ���� ���� �����ϱ�
    // 2���� ������ "/Assets/Save/TestSave.json"�� �о Debug�� ����ϱ�

    Player player;
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    protected override void Test1(InputAction.CallbackContext _)
    {
        player.AddScore(700);
    }
    protected override void Test4(InputAction.CallbackContext _)
    {
        string path = $"{Application.dataPath}/Save/";              // ���
        string fullPath = $"{path}TestSave.json";                   // ��ü ���

        TestSaveData data = new TestSaveData();
        data.score = player.Score;
        string json = JsonUtility.ToJson(data);
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(fullPath , json );
        Debug.Log("���̺� �Ϸ�");
    }
    protected override void Test5(InputAction.CallbackContext _)
    {
        string path = $"{Application.dataPath}/Save/";              // ���
        string fullPath = $"{path}TestSave.json";                   // ��ü ���

        if(Directory.Exists(path)&& File.Exists(fullPath))
        {
            string dataStr = File.ReadAllText(fullPath);

            TestSaveData data = JsonUtility.FromJson<TestSaveData>(dataStr);
            Debug.Log($"���� ������ : {data.score}");
            player.AddScore(data.score);
        }
    }

}
