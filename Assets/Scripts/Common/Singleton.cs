using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component //싱글톤 : 객체를 하나만 가지는 디자인 패턴
{
    //static : "정적" 이라는 단어로 많이 번역됨. 프로그램 실행 ★"전"에 메모리 주소가 결정되어있는 것에 붙임.
    //         멤버 변수에 붙이면 클래스의 모든 객체에서 공용으로 사용할 수 있는 변수가 된다.
    //dynamic: "동적" 이라는 단어로 많이 번역됨. 프로그램 실행 ★"중"에 메모리 주소가 결정되는 것에 붙임.

    private bool initalized = false;              // 초기화를 진행한 표시를 나타내는 플래그

    private const int NOT_SET = -1;               // 설정 안되었다는 것을 표시하기 위한 상수
    private int mainSceneIndex = NOT_SET;         // 게임의 메인씬의 인덱스

    private static bool isShutDown = false;       // 이미 종료처리에 들어갔는지 표시하기 위한 용도

    private static T instance;    // 싱글톤용 객체. 다른 곳에서 접근 못하게 private으로 설정

    // 싱글톤 읽기전용 프로퍼티. 이 프로퍼티로만 싱글톤에 접근 가능.
    public static T Inst
    {
        get
        {
            if (isShutDown)       // 종료처리에 들어간 상황이면
            {
                Debug.LogWarning($"{typeof(T)} 싱글톤은 이미 삭제되었다.");  // 이 코드는 수행되지 않아야 한다.(이게 뜨면 사용한 곳에서 코드를 잘못 만든 것)
                return null;      // null 리턴
            }
            if (instance == null)      // 접근한 시점에서 instance가 있는지 없는지 확인
            {
                // 없으면 만들어진적이 없다.

                T obj = FindObjectOfType<T>();  // 에디터에서 만들어진것이 있는지 확인
                if (obj == null)                 // null이면 에디터에서 만들어진것도 없다.
                {
                    GameObject gameObj = new GameObject();      // 빈오브젝트 생성
                    gameObj.name = typeof(T).Name;              // 이름 변경하고
                    obj = gameObj.AddComponent<T>();            // 싱글톤을 컴포넌트로 추가
                }
                instance = obj;                                 // 없어서 새로 만든 것이든 에디터가 만들어 놓았던 것이든 instance에 저장
                DontDestroyOnLoad(obj.gameObject);              // 씬이 닫히더라도 게임 오브젝트가 삭제되지 않게 설정
            }
            return instance;            // instance 리턴(없으면 새로 만들었고 있었으면 있던 것, 그래서 무조건 null이 아닌 값이 리턴된다.)
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            // instance가 null이면 처음 생성 완료된 싱글톤 게임 오브젝트이다. (씬에 배치되어 있는 게임 오브젝트)
            instance = this as T;                     // instance에 이 싱글톤 객체 기록
            DontDestroyOnLoad(instance.gameObject);   // 씬이 닫히더라도 게임 오브젝트가 삭제되지 않게 설정
        }
        else
        {
            // instance가 null이 아니면 이미 만들어진 싱글톤 게임 오브젝트가 있는 상황
            if (instance != this)
            {
                // Awake되기 전에 다른 코드에서 프로퍼티를 통해서 접근 했고 그래서 생성이 된 상황
                Destroy(this.gameObject);    // 나중에 만들어진 자기 자신을 삭제한다.
            }
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnApplicationQuit()
    {
        isShutDown = true;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)  // 씬이 로드되며 호출이 되는 함수  //scene = 로드된 씬, mode = 로드 모드
    {
        preInitialize();
        Initialize();
    }
    protected virtual void preInitialize() // 이 싱글톤이 처음 만들어졌을 때 단 한번만 실행될 초기화 함수(Awake 제일 마지막에 호출)
    {
        if (!initalized)                                    //초기화 되지 않았을 때만 실행
        {
            initalized = true;                              // 초기화 되었다고 표시해서 두번 실행되지 않게 하기
            Scene active = SceneManager.GetActiveScene();   // 현재 씬 가져와서
            mainSceneIndex = active.buildIndex;             // 인덱스 저장해 놓기
        }
    }
    protected virtual void Initialize() // 이 싱글톤이 만들어지고 씬이 로드 될 때 마다 실행될 초기화 함수
    {

    }
    public class TestSingleton //일반 싱글톤 예제
    {
        private static TestSingleton instance = null; // static변수를 만들어서 객체를 만들지 않고 사용할 수 있게 만들기.

        public static TestSingleton Instance  // 다른곳에서 instance를 수정하지 못하도록 읽기 전용 프로퍼티 만들기.
        {
            get
            {

                if (instance == null)      //처음 접근했을 때 new하기.
                {
                    instance = new TestSingleton();
                }
                return instance;    // 항상 return될 때 값은 존재한다.
            }
        }
        private TestSingleton()  // 중복생성 방지 목적,  private으로 생성자를 만들어 기본 pubilc생성자가 생성되지 않게 막기

        {

        }
    }


}