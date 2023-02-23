using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    Transform FireTransform;
    public float speed = 10.0f;              //플레이어의 이동 속도

    public GameObject bullet;                //총알 이펙트
    private GameObject fireflash;            //총알 발사 이벤트

    public float fireInterval = 0.5f;        //총알 발사 간격

    private int score = 0;

    IEnumerator fireCoroutine;               //연사용 코루틴을 저장할 변수
    
    //델리게이트(delegate): 신호를 보내는 것
    public Action<int> onScoreChange;        //점수가 변경되면 실행될 델리게이트. 파라미터가 int 하나이고 리턴타입이 void인 함수를 등록할 수 있다.
    Animator anim;
    PlayerInputActions inputActions;
    Vector3 inputDir = Vector3.zero;

    // 이 게임 오브젝트가 생성완료 되었을 때 실행되는 함수
    private void Awake()
    {
        transform.Find("FireTransform");
        FireTransform = transform.GetChild(0);
        fireflash = transform.GetChild(1).gameObject; 
        fireflash.SetActive(false);

        anim = GetComponent<Animator>();               //Getcomponent는 성능에 문제가 있기 때문에 한번만 찾도록 코딩
        inputActions = new PlayerInputActions();

        fireCoroutine = FireCoroutine();
    }

    // 이 게임 오브젝트가 완성된 이후에 활성화 할 때 실행되는 함수
    private void OnEnable()
    {
        inputActions.Player.Enable();
        //inputActions.Player.Fire.started;   // 버튼을 누른 직후
        //inputActions.Player.Fire.performed; // 버튼을 충분히 눌렀을 때
        //inputActions.Player.Fire.canceled;  // 버튼을 땐 직후
        inputActions.Player.Fire.performed += OnFireStart;
        inputActions.Player.Fire.canceled += OnFireStop;
        inputActions.Player.Bomb.performed += OnBomb;
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }
    // 이 게임 오브젝트가 비활성화 될 때 실행되는 함수
    private void OnDisable()
    {
        inputActions.Player.Fire.performed -= OnFireStart;
        inputActions.Player.Fire.canceled -= OnFireStop;
        inputActions.Player.Bomb.performed -= OnBomb;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Disable();
    }
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
            /*if(onScoreChange != null)
            {
                onScoreChange.Invoke(score);
            }*/
            onScoreChange?.Invoke(Score); //위에 4줄을 줄인것. 점수가 변경되었음을 사방에 알림.
            Debug.Log($"점수 : {score}");
        }
    }
    // 시작할 때 한번 실행되는 함수
    void Start()
    {
        /*Debug.Log("Start");*/
        //gameObject.SetActive(false);  // 게임 오브젝트 비활성화 시키기
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
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"충돌영역에 들어감 - 충돌 대상 : {collision.gameObject.name}");

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log($"충돌영역에 나감 - 충돌 대상 : {collision.gameObject.name}");
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("충돌영역에 접촉해 있으면서 움직이는 중");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"트리거 안에 들어감 - 대상 트리거 : {collision.gameObject.name}");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log($"트리거 안에 나감 - 대상 트리거 : {collision.gameObject.name}");
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("트리거 안에서 움직임");
    }*/
    private void OnBomb(InputAction.CallbackContext context)
    {
        Debug.Log("Bomb");
    }

    private void OnFireStart(InputAction.CallbackContext _)
    {
        //Debug.Log("Fire");
        StartCoroutine(fireCoroutine);              //눌렀을때 코루틴 시작
    }
    private void OnFireStop(InputAction.CallbackContext _)
    {
        StopCoroutine(fireCoroutine);                 //땠을때 코루틴 정지
    }
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            //Instantiate함수는 오브젝트를 추가하는 함수
            //https://velog.io/@ko0930/C-Unity-GetComponentInstantiateDestroyprefabInvoke참고
            GameObject obj = Instantiate(bullet);               // 총알 생성
            obj.transform.position = FireTransform.position;    // 위치 변경
            StartCoroutine(FlashEffect());
            yield return new WaitForSeconds(fireInterval);      // 연사 간격만큼 대기
        }
    }
    IEnumerator FlashEffect()
    {
        fireflash.SetActive(true);              // 켜고
        yield return new WaitForSeconds(0.1f);  // 0.1초 대기하고
        fireflash.SetActive(false);             // 끄고
    }
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        anim.SetFloat("InputY", dir.y); //애니메이터에 있는 input y값에 파라메터에 dir.y값을 준다.
        inputDir = dir;
    }
    public void AddScore(int plus)
    {
        Score += plus;
    }
}
