using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Transform FireTransform;
    public float speed = 10.0f;
    public GameObject bullet;

    Animator anim;
    PlayerInputActions inputActions;
    Vector3 inputDir = Vector3.zero;

    // 이 게임 오브젝트가 생성완료 되었을 때 실행되는 함수
    private void Awake()
    {
        transform.Find("FireTransform");
        FireTransform = transform.GetChild(0);
        anim = GetComponent<Animator>();               //Getcomponent는 성능에 문제가 있기 때문에 한번만 찾도록 코딩
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
        //https://inyongs.tistory.com/18참고
        //Time.deltaTime = 이전 프레임에서 현재 프레임까지의 시간
        //inputDir = 입력받은값
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"충돌영역에 들어감 - 충돌 대상 : {collision.gameObject.name}");
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log($"충돌영역에 나감 - 충돌 대상 : {collision.gameObject.name}");
    }
   /* private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("충돌영역에 접촉해 있으면서 움직이는 중");
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"트리거 안에 들어감 - 대상 트리거 : {collision.gameObject.name}");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log($"트리거 안에 나감 - 대상 트리거 : {collision.gameObject.name}");
    }
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("트리거 안에서 움직임");
    }*/
    private void OnBomb(InputAction.CallbackContext context)
    {
        Debug.Log("Bomb");
    }
    
    private void OnFire(InputAction.CallbackContext context)
    {
        //Debug.Log("Fire");
        //Instantiate함수는 오브젝트를 추가하는 함수
        //https://velog.io/@ko0930/C-Unity-GetComponentInstantiateDestroyprefabInvoke참고
        GameObject obj = Instantiate(bullet);
        obj.transform.position = FireTransform.position;
    }
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        anim.SetFloat("Input Y",dir.y); //애니메이터에 있는 input y값에 파라메터에 dir.y값을 준다.
        inputDir = dir;
    }
}