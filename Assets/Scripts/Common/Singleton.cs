using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour  //�̱��� : ��ü�� �ϳ��� ������ ������ ����
{
    //static : "����" �̶�� �ܾ�� ���� ������. ���α׷� ���� ��"��"�� �޸� �ּҰ� �����Ǿ��ִ� �Ϳ� ����.
    //         ��� ������ ���̸� Ŭ������ ��� ��ü���� �������� ����� �� �ִ� ������ �ȴ�.
    //dynamic: "����" �̶�� �ܾ�� ���� ������. ���α׷� ���� ��"��"�� �޸� �ּҰ� �����Ǵ� �Ϳ� ����.

    private static Singleton instance;    // �̱���� ��ü. �ٸ� ������ ���� ���ϰ� private���� ����
                                      
    // �̱��� �б����� ������Ƽ. �� ������Ƽ�θ� �̱��濡 ���� ����.
    public static Singleton Inst     
    {
        get
        {
            if(instance == null)      // ������ �������� instance�� �ִ��� ������ Ȯ��
            {
                // ������ ����������� ����.

                Singleton obj = FindObjectOfType<Singleton>();  // �����Ϳ��� ����������� �ִ��� Ȯ��
                if(obj == null)                                 // null�̸� �����Ϳ��� ��������͵� ����.
                {
                    GameObject gameObj = new GameObject();      // �������Ʈ ����
                    gameObj.name = "Singletoin";                // �̸� �����ϰ�
                    obj = gameObj.AddComponent<Singleton>();          // �̱����� ������Ʈ�� �߰�
                }
                instance = obj;                                 // ��� ���� ���� ���̵� �����Ͱ� ����� ���Ҵ� ���̵� instance�� ����
                DontDestroyOnLoad(obj.gameObject);              // ���� �������� ���� ������Ʈ�� �������� �ʰ� ����
            }
            return instance;            // instance ����(������ ���� ������� �־����� �ִ� ��, �׷��� ������ null�� �ƴ� ���� ���ϵȴ�.)
        }
    }
    private void Awake()
    {
        if(instance==null)           
        {
            // instance�� null�̸� ó�� ���� �Ϸ�� �̱��� ���� ������Ʈ�̴�. (���� ��ġ�Ǿ� �ִ� ���� ������Ʈ)
            instance = this;                          // instance�� �� �̱��� ��ü ���
            DontDestroyOnLoad(instance.gameObject);   // ���� �������� ���� ������Ʈ�� �������� �ʰ� ����
        }
        else
        {
            // instance�� null�� �ƴϸ� �̹� ������� �̱��� ���� ������Ʈ�� �ִ� ��Ȳ
            if(instance != this)       
            {
                // Awake�Ǳ� ���� �ٸ� �ڵ忡�� ������Ƽ�� ���ؼ� ���� �߰� �׷��� ������ �� ��Ȳ
                Destroy(this.gameObject);    // ���߿� ������� �ڱ� �ڽ��� �����Ѵ�.
            }
        }
    }
}
public class TestSingleton //�Ϲ� �̱��� ����
{
    private static TestSingleton instance = null; // static������ ���� ��ü�� ������ �ʰ� ����� �� �ְ� �����.

    public static TestSingleton Instance  // �ٸ������� instance�� �������� ���ϵ��� �б� ���� ������Ƽ �����.
    {
        get
        {

            if (instance == null)      //ó�� �������� �� new�ϱ�.
            {
                instance = new TestSingleton();   
            }
            return instance;    // �׻� return�� �� ���� �����Ѵ�.
        }
    }
    private TestSingleton()  // �ߺ����� ���� ����,  private���� �����ڸ� ����� �⺻ pubilc�����ڰ� �������� �ʰ� ����
                             
    {

    }
}
