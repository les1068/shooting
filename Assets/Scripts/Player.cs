using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 10.0f;
    PlayerInputActions inputActions;
    Vector3 inputDir = Vector3.zero;

    // 이 게임 오브젝트가 생성완료 되었을 때 실행되는 함수
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    // 이 게임 오브젝트가 완성된 이후에 활성화 할 때 실행되는 함수
    private void OnEnable()
    {
        inputActions.Player.Enable();
        //inputActions.Player.Fire.started;   // 버튼을 누른 직후
        //inputActions.Player.Fire.performed; // 버튼을 충분히 눌렀을 때
        //inputActions.Player.Fire.canceled;  // 버튼을 땐 직후
        inputActions.Player.Fire.performed += OnFire;
        inputActions.Player.Bomb.performed += OnBomb;
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    // 이 게임 오브젝트가 비활성화 될 때 실행되는 함수
    private void OnDisable()
    {
        inputActions.Player.Fire.performed -= OnFire;
        inputActions.Player.Bomb.performed -= OnBomb;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Disable();
    }

    // 시작할 때 한번 실행되는 함수
    void Start()
    {
        Debug.Log("Start");
    }

    // 매 프레임마다 계속 실행되는 함수
    void Update()
    {
        transform.position += Time.deltaTime * speed * inputDir;
        //transform.Translate(inputDir);
        //Time.deltaTime = 이전 프레임에서 현재 프레임까지의 시간
    }
    private void OnBomb(InputAction.CallbackContext context)
    {
        Debug.Log("Bomb");
    }
    private void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire");
    }
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        inputDir = dir;
    }
}