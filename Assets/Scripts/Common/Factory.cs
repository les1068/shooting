using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum PoolObjectType  // enum(�̳�) Ÿ��
{
    Bullet = 0,
    Hit,
    Enemy,
    Explosion
}
public class Factory : Singleton<Factory> // ������Ʈ �������ִ� Ŭ����
{
    // ������ ������Ʈ�� Ǯ��
    BulletPool bulletPool;
    EnemyPool enemyPool;
    ExplosionEffectPool explosionPool;
    HitEffectPool hitPool;

    protected override void preInitialize()  // �� �̱����� ����� �� �� ó�� �ѹ��� ȣ��� �Լ�
    {
        // �ڽ����� �پ��ִ� Ǯ�� �� ã�Ƴ���
        bulletPool = GetComponentInChildren<BulletPool>();
        enemyPool= GetComponentInChildren<EnemyPool>();
        explosionPool = GetComponentInChildren<ExplosionEffectPool>();
        hitPool = GetComponentInChildren<HitEffectPool>();
    }
    protected override void Initialize() //���� �ε�� ������ ȣ��Ǵ� �ʱ�ȭ �Լ�
    {
        bulletPool?.Initialize();      // ?.�� null�� �ƴϸ� ����, null�̸� �ƹ��͵� ���� �ʴ´�.
        enemyPool?.Initialize();
        explosionPool?.Initialize();
        hitPool?.Initialize();
    }
    public GameObject GetObject(PoolObjectType type)  // ������ ������Ʈ�� Ǯ���� �����ִ� �Լ�. type = �᳾ ������Ʈ�� ����
    {
        GameObject result = null;
        switch (type)               // type�� �°� ������ result�� ����
        {
            case PoolObjectType.Bullet:
                result = GetBullet().gameObject;
                break;
            case PoolObjectType.Hit:
                result = GetHitEffect().gameObject;
                break;
            case PoolObjectType.Enemy:
                result = GetEnemy().gameObject;
                break;
            case PoolObjectType.Explosion:
                result = GetExplosionEffect().gameObject;
                break;
        }
        return result;
    }

    public Bullet GetBullet() => bulletPool?.GetObject();    // BulletǮ���� Bullet�ϳ� ������ �Լ�
    public Effect GetHitEffect() => hitPool?.GetObject();   // EffectǮ���� Effect�ϳ� ������ �Լ�
    public Enemy GetEnemy() => enemyPool?.GetObject();    // EnemyǮ���� Enemy�ϳ� ������ �Լ�
    public Effect GetExplosionEffect() => explosionPool?.GetObject();    // ExplosionEffectǮ���� ExplosionEffect�ϳ� ������ �Լ�
}
