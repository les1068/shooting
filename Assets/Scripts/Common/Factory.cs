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
public class Factory : Singleton<Factory>
{
    BulletPool bulletPool;
    EnemyPool enemyPool;
    ExplosionEffectPool explosionPool;
    HitEffectPool hitPool;

    protected override void preInitialize()
    {
        bulletPool = GetComponentInChildren<BulletPool>();
        enemyPool= GetComponentInChildren<EnemyPool>();
        explosionPool = GetComponentInChildren<ExplosionEffectPool>();
        hitPool = GetComponentInChildren<HitEffectPool>();
    }
    protected override void Initialize()
    {
        bulletPool?.Initialize();      // ?.�� null�� �ƴϸ� ����, null�̸� �ƹ��͵� ���� �ʴ´�.
        enemyPool?.Initialize();
        explosionPool?.Initialize();
        hitPool?.Initialize();
    }
    public GameObject GetObject(PoolObjectType type)
    {
        GameObject result = null;
        switch (type)
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

    public Bullet GetBullet() => bulletPool?.GetObject();
    public Effect GetHitEffect() => hitPool?.GetObject();
    public Enemy GetEnemy() => enemyPool?.GetObject();
    public Effect GetExplosionEffect() => explosionPool?.GetObject();
}
