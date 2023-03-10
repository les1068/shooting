using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //---------------------------------------------------------------------------------------
    [Header("플레이어 데이터")]

    public float speed = 10.0f;  // 플레이어의 이동 속도

    public float invincibleTime = 2.0f; // 피격 되었을 때의 무적 시간

    private bool isInvincibleMode = false;  // 무적 상태인지 아닌지 표시용

    private float timeElapsed = 0.0f;  // 무적일 때 시간 누적용(cos에서 사용할 용도)

    public int initialLife = 3;  // 시작할 때 생명

    private int life = 3;  // 현재 생명

    private bool isDead = false; // 사망 표시

    public PoolObjectType explosion = PoolObjectType.Explosion;  // 플레이어 비행기 터지는 이팩트
    private int Life  // 수명 처리용 프로퍼티
    {
        get => life;
        set
        {
            if (!isDead)
            {
                if (life > value)
                {
                    // 라이프가 감소한 상황이면
                    OnHit();  // 맞았을 때의 동작이 있는 함수
                }
                life = value;
                Debug.Log($"Life : {life}");
                if ( life <= 0)
                {
                    OnDie();  // 죽었을 때의 동작이 있는 함수 실행
                }
                OnLifeChange?.Invoke(life);  // 델리게이트에 연결된 함수들 실행
            }

        }
    }
    public Action<int> OnLifeChange;  // 수명이 변경되었을 때 실행될 델리게이트

    public Action<Player> onDie;      // 죽었을 때 실행될 델리게이트

    private int score = 0;  // 플레이어의 점수
    int power = 0;  // 현재 플레이어의 파워
    int extraPowerBonus = 300;  // 파워가 최대치일 때 파워업 아이템을 먹으면 얻는 보너스

    private int Power  // 파워를 증감시키기 위한 프로퍼티(설정시 추가처리 있음)
    {
        get => power;
        set
        {
            power = value;
            if (power > 3)                      // 3을 넘어가면 
                AddScore(extraPowerBonus);      // 보너스 점수 추가
            power = Mathf.Clamp(power, 1, 3);   // 1~3사이로 설정되게 Clamp 처리

            RefreshFirePostions(power);         // FireTransforms의 위치와 회전 처리
        }
    }
    // 델리게이트(Delegate) : 신호를 보내는 것. 함수를 등록할 수 있다.
    public Action<int> onScoreChange;  // 점수가 변경되면 실행될 델리게이트. 파라메터가 int 하나이고 리턴타입이 void인 함수를 등록할 수 있다.
    // 프로퍼티(Property) : 값을 넣거나 읽을 때 추가적으로 할일이 많을 때 사용
    
    //---------------------------------------------------------------------------------------
    [Header("총알")]

    public float fireInterval = 0.5f;  // 총알 발사 간격


    public PoolObjectType bulletType;  // 플레이어의 총알 타입


    private Transform[] fireTransforms;  // 발사 위치 표시용 트랜스폼들

    private GameObject fireFlash;  // 총알 발사 이팩트

    IEnumerator fireCoroutine;  // 연사용 코루틴을 저장할 변수

    //---------------------------------------------------------------------------------------
    [Header("컴포넌트")]

    private Animator anim;  // 에니메이터 컴포넌트

    private Rigidbody2D rigid;  // 리지드바디2D 컴포넌트

    private SpriteRenderer spriteRenderer;  // 스프라이트 랜더러 컴포넌트

    //---------------------------------------------------------------------------------------
    [Header("입력값")]

    private PlayerInputActions inputActions;  // 입력처리용 InputAction

    private Vector3 inputDir = Vector3.zero;  // 현재 입력된 입력 방향

    float fireAngle = 30.0f;
    
    public int Score    
    {
        // get : 다른 곳에서 특정 값을 확인할 때 사용됨
        // set : 다른 곳에서 특정 값을 설정할 때 사용됨

        //get     
        //{
        //    return score;
        //}
        get => score;   // 위에 주석으로 처리된 get을 요약한 것

        private set     // 앞에 private를 붙이면 자신만 사용가능
        {
            score = value;
            //if( onScoreChange != null )
            //{
            //    onScoreChange.Invoke(score);
            //}
            onScoreChange?.Invoke(score);   // 위의 4줄을 줄인 것. 점수가 변경되었음을 사방에 알림.

            Debug.Log($"점수 : {score}");
        }
    }

    // 이 게임 오브젝트가 생성완료 되었을 때 실행되는 함수
    private void Awake()
    {
        anim = GetComponent<Animator>();            // GetComponent는 성능 문제가 있기 때문에 한번만 찾도록 코드 작성
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inputActions = new PlayerInputActions();
        Transform fireRoot = transform.GetChild(0);
        fireTransforms = new Transform[fireRoot.childCount];
        for (int i = 0;i < fireRoot.childCount; i++)
        {
            fireTransforms[i] = fireRoot.GetChild(i);
        }

        fireFlash = transform.GetChild(1).gameObject;        
        fireFlash.SetActive(false);

        fireCoroutine = FireCoroutine();            // 코루틴 미리 만들어 놓기
    }

    // 이 게임 오브젝트가 완성된 이후에 활성화 할 때 실행되는 함수
    private void OnEnable()
    {
        inputActions.Player.Enable();
        //inputActions.Player.Fire.started;       // 버튼을 누른 직후
        //inputActions.Player.Fire.performed;     // 버튼을 충분히 눌렀을 때
        //inputActions.Player.Fire.canceled;      // 버튼을 땐 직후

        inputActions.Player.Fire.performed += OnFireStart;
        inputActions.Player.Fire.canceled += OnFireStop;
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    // 이 게임 오브젝트가 비활성화 될 때 실행되는 함수
    private void OnDisable()
    {
        InputDisable();
    }

    // 시작할 때 한번 실행되는 함수
    void Start()
    {
        //Debug.Log("Start");
        //gameObject.SetActive(false);  // 게임 오브젝트 비활성화 시키기
        
        Power = 1;  // power는 1로 시작

        life = initialLife; // 초기 생명 설정
    }

    // 매 프레임마다 계속 실행되는 함수
    //void Update()
    //{
    //    // 인풋 매니저 사용 방식 - 앞으로 사용안함
    //    //Debug.Log("Update");
    //    //if( Input.GetKeyDown(KeyCode.W))
    //    //{
    //    //    Debug.Log("W키가 눌러짐");
    //    //}
    //    //if (Input.GetKeyDown(KeyCode.A))
    //    //{
    //    //    Debug.Log("A키가 눌러짐");
    //    //}
    //    //if (Input.GetKeyDown(KeyCode.S))
    //    //{
    //    //    Debug.Log("S키가 눌러짐");
    //    //}
    //    //if (Input.GetKeyDown(KeyCode.D))
    //    //{
    //    //    Debug.Log("D키가 눌러짐");
    //    //}
    //    //float input = Input.GetAxis("Horizontal");  // 수평 방향 처리
    //    ////Debug.Log(input);
    //    //// 수직 입력 처리하기
    //    //input = Input.GetAxis("Vertical");
    //    //Debug.Log(input);

    //    //Time.deltaTime * speed * inputDir     // 곱하기 총 4번
    //    //inputDir * Time.deltaTime * speed     // 곱하기 총 6번


    //    //transform.position += Time.deltaTime * speed * inputDir;
    //    //transform.Translate(Time.deltaTime * speed * inputDir); // 초당 speed의 속도로 inputDir방향으로 이동
    //    // Time.deltaTime : 이전 프레임에서 현재 프레임까지의 시간

    //    // 30프레임 컴퓨터의 deltaTime = 1/30초 = 0.333333
    //    // 120프레임 컴퓨터의 deltaTime = 1/120초 = 0.0083333
    //    //Debug.Log(Time.deltaTime);
    //}

    private void FixedUpdate()  // 일정한 시간 간격으로 호출되는 업데이트 함수(물리처리용)
    {
        /// 항상 일정한 시간 간격으로 실행되는 업데이트
        /// 물리 연산이 들어가는 것은 이쪽에서 실행
        //Debug.Log(Time.fixedDeltaTime);

        //rigid.MovePosition(); 
        // 특정 위치로 순간이동 시키기.
        // 관성이 없는 움직임을 시킬 때 유용
        // 움직일 때 물리적으로 막히면 거기서부터는 진행을 하지 않는다.

        //rigid.AddForce();
        // 특정 방향으로 힘을 가하는 것.
        // 관성이 있다.
        // 움직일 때 물리적으로 막히면 거기서부터는 진행을 하지 않는다.
        if (isDead)
        {
            rigid.AddForce(Vector2.left * 0.3f, ForceMode2D.Impulse);  // 왼쪽으로 폭팔적으로 힘 추가
            rigid.AddTorque(50.0f);                                    // 반시계 방향으로 회전
        }
        else
        {
            rigid.MovePosition(transform.position + Time.fixedDeltaTime * speed * inputDir);
        }

    }
    private void Update()
    {
        if(isInvincibleMode)
        {
            timeElapsed += Time.deltaTime * 30;   // 1초당으로 처리하면 한번깜빡이는데 3.141592... 초가 필요
            float alpha = (MathF.Cos(timeElapsed) +1.0f) * 0.5f;  // cos 결과를 1~0~1로 변경
            spriteRenderer.color = new Color(1,1,1,alpha);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Life--;
        }
        else if (collision.gameObject.CompareTag("PowerUp"))
        {
            Power++;
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // 충돌 했을 때 실행되는 함수(2D용)
    {
        //Debug.Log($"트리거 안에 들어감 - 대상 트리거 : {collision.gameObject.name}");
    }

    private void OnTriggerExit2D(Collider2D collision)  // 발사 입력 처리용 함수
    {
        //Debug.Log($"트리거에서 나감 - 대상 트리거 : {collision.gameObject.name}");        
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log("트리거 안에서 움직임");        
    //}

    private void OnFireStart(InputAction.CallbackContext _)  // 발사 중지 입력 처리용 함수
    {
        //Debug.Log("Fire");

        StartCoroutine(fireCoroutine);      // 눌렀을 때 코루틴 시작
    }

    private void OnFireStop(InputAction.CallbackContext _)
    {
        StopCoroutine(fireCoroutine);       // 땠을 때 코루틴 정지
    }


    IEnumerator FireCoroutine()  // 연사용 코루틴 함수
    {
        while (true)
        {            
            for(int i=0;i<power;i++)
            {
                GameObject obj = Factory.Inst.GetObject(bulletType);   // bulletType에 맞는 총알 생성
                Transform firePos = fireTransforms[i];
                obj.transform.position = firePos.position;
                obj.transform.rotation = firePos.rotation;
            }
            
            StartCoroutine(FlashEffect());                      // flash 이팩트 깜박이기
            yield return new WaitForSeconds(fireInterval);      // 연사 간격만큼 대기
        }
    }

    IEnumerator FlashEffect()   // flash가 깜빡하는 코루틴
    {
        fireFlash.SetActive(true);              // 켜고
        yield return new WaitForSeconds(0.1f);  // 0.1초 대기하고
        fireFlash.SetActive(false);             // 끄고
    }

    private void OnBomb(InputAction.CallbackContext context)
    {
        Debug.Log("Bomb");
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        anim.SetFloat("InputY", dir.y);         // 에니메이터에 있는 InputY 파라메터에 dir.y값을 준다.
        inputDir = dir;
    }
    private void InputDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Fire.canceled -= OnFireStop;
        inputActions.Player.Fire.performed -= OnFireStart;
        inputActions.Player.Disable();
    }

    private void OnHit()  // 맞았을 때 실행되는 함수
    {
        Power--;                                // 파워 1줄이고
        StartCoroutine(EnterInvincibleMode());  // 무적모드 들어가기
    }
    IEnumerator EnterInvincibleMode()  // 무적상태 진입용 코루틴
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible");  // 레이어를 무적 레이어로 변경
        isInvincibleMode = true;  // 무적 모드로 들어갔다고 표시
        timeElapsed = 0.0f;       // 시간 카운터 초기화

        yield return new WaitForSeconds(invincibleTime);   // invincibleTime만큼 기다리기

        spriteRenderer.color = Color.white;          // 색이 변한 상태에서 무적모드가 끝날 때를 대비해서 색상도 초기화
         
        isInvincibleMode = false;                    // 무적 모드 끝났다고 표시
        gameObject.layer = LayerMask.NameToLayer("Player");  // 레이어 되돌리기
    }
    private void OnDie()
    {
        isDead = true;
        life = 0;

        Collider2D bodyCollider = GetComponent<Collider2D>();
        bodyCollider.enabled = false;                           // 컬라이더 꺼서 양향력 제거

        GameObject effect = Factory.Inst.GetObject(explosion);  // 터지는 이팩트 만들고
        effect.transform.position = transform.position;         // 이팩트 내 위치에 배치

        InputDisable();                 // 입력 막기
        inputDir = Vector3.zero;        // 이동 입력도 초기화

        StopCoroutine(fireCoroutine);   // 총을 쏘던 중이면 더 이상 쏘지 않게 만들기

        // 무적모드 취소
        spriteRenderer.color = Color.white;
        isInvincibleMode = false;
        gameObject.layer = LayerMask.NameToLayer("Player");

        rigid.gravityScale = 1.0f;      // 중력 다시 적용
        rigid.freezeRotation = false;   // 회전도 풀기

        onDie?.Invoke(this);
    }
    public void AddScore(int plus)  // Score에 점수를 추가하는 함수
    {
        Score += plus;
    }

    private void RefreshFirePostions(int power)
    {
        // 기존 fireRoot의 자식 비활성화 하기
        for (int i = 0; i < fireTransforms.Length; i++)
        {
            fireTransforms[i].gameObject.SetActive(false);
        }

        // fireRoot에 power 숫자에 맞게 자식 활성화
        for(int i=0;i<power;i++)
        {
            // 파워1: 0
            // 파워2: -15, 15
            // 파워3: -30, 0, 30
            
            Transform firePos = fireTransforms[i];
            firePos.localPosition = Vector3.zero;
            firePos.rotation = Quaternion.Euler(0, 0, (power - 1) * (fireAngle * 0.5f) + i * -fireAngle);
            firePos.Translate(1, 0, 0);

           /*파워1: (1 - 1) * (30 * 0.5f) + 0 * -30 = 0
             파워2
              i = 0) (2 - 1) * (30 * 0.5f) + 0 * -30 = 15
              i = 1) (2 - 1) * (30 * 0.5f) + 1 * -30 = -15
             파워3
              i = 0) (3 - 1) * (30 * 0.5f) + 0 * -30 = 30
              i = 1) (3 - 1) * (30 * 0.5f) + 1 * -30 = 0
              i = 2) (3 - 1) * (30 * 0.5f) + 2 * -30 = -30*/

            fireTransforms[i].gameObject.SetActive(true);
        }
    }
}
