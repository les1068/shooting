using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ������Ʈ�� �ֱ������� ������ Ŭ����
public class Spawner : MonoBehaviour
{
    //������ ���� ������Ʈ
    public GameObject spawnprefab;
    
    public float minY = -4; //������ ��ġ(�ּҰ�)
    public float maxY = 4;  //������ ��ġ(�ִ�)
    //�ð� ����
    public float interval = 5.0f;

    //WaitForSeconds wait;
    private void Start()
    {
       // Wait =new WaitForSeconds(interval); //���ӵ��߿� interval�� ������ �ʴ´ٸ� �̸� ����� �δ�
        
        //������ �� Spawn �ڷ�ƾ ����
        StartCoroutine(Spawn());
    }

   
    IEnumerator Spawn()  //"�ڡڿ�����Ʈ�� �ֱ������� �����ϴ� �ڷ�ƾ"
    {
        while(true)    //���� ����
        {
            //�����ϰ� ������ ������Ʈ�� �������� �ڽ����� �����
            GameObject obj = Instantiate(spawnprefab, transform); 
            //obj.transform.position = transform.position;
            
            float r = Random.Range(minY, maxY); //�����ϰ� ������ ���� ����
            obj.transform.Translate(Vector3.up *r); //���� ����

            //yield return wait;
            yield return new WaitForSeconds(interval); //interval ��ŭ ���

        }
    }

    private void OnDrawGizmos() //��â�� ���߿� ������ �׸��� �Լ�
    {
        Gizmos.color = Color.green; //���� ��ü������ ���� ������
        //Gizmos.color = new Color(1,0,0); //RGB ������ ���������
        
        //���� ������ ������ �߱�
        Vector3 from = transform.position + Vector3.up * minY;
        Vector3 to = transform.position + Vector3.up * maxY;
        Gizmos.DrawLine(from, to);

        Gizmos.DrawWireCube(transform.position,new Vector3(1, Mathf.Abs(maxY)+Mathf.Abs(minY)+2 , 1));

    }

}
