using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    Animator anim;
    Button button;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(OnRestart);   // ��ư Ŭ�� �̺�Ʈ�� �Լ� ���

    }
    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onDie += (_) => ShowPanel();  // �÷��̾��� onDie ��������Ʈ�� �Լ� ���
        //gameObject.SetActive(false);    // ���� ������Ʈ ��Ȱ��ȭ
    }
    private void OnRestart()
    {
        SceneManager.LoadScene(0);  // 0���� �ҷ�����
    }
    private void ShowPanel()
    {
        //transform.GetChild(0).gameObject.SetActive(true);   // ���ڰ� ��Ȱ��ȭ �Ǿ��ִ� ���� Ȱ��ȭ
        //gameObject.SetActive(true);     // ���� ���� �г� ��ü ���̱�

        anim.SetTrigger("GameOverStart");
    }
    
}