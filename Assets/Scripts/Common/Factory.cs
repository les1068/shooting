using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum PoolObjectType  // enum(이넘) 타입
{
    Bullet = 0,
    Hit,
    Enemy,
    Explosion
}
public class Factory : Singleton<Factory> // 오브젝트 생성해주는 클래스
{
    // 생성할 오브젝트의 풀들
    BulletPool bulletPool;
    EnemyPool enemyPool;
    ExplosionEffectPool explosionPool;
    HitEffectPool hitPool;

    protected override void preInitialize()  // 이 싱글톤이 만들어 질 때 처음 한번만 호출될 함수
    {
        // 자식으로 붙어있는 풀들 다 찾아놓기
        bulletPool = GetComponentInChildren<BulletPool>();
        enemyPool= GetComponentInChildren<EnemyPool>();
        explosionPool = GetComponentInChildren<ExplosionEffectPool>();
        hitPool = GetComponentInChildren<HitEffectPool>();
    }
    protected override void Initialize() //씬이 로드될 때마다 호출되는 초기화 함수
    {
        bulletPool?.Initialize();      // ?.은 null이 아니면 실행, null이면 아무것도 하지 않는다.
        enemyPool?.Initialize();
        explosionPool?.Initialize();
        hitPool?.Initialize();
    }
    public GameObject GetObject(PoolObjectType type)  // 지정된 오브젝트를 풀에서 꺼내주는 함수. type = 써낼 오브젝트의 종류
    {
        GameObject result = null;
        switch (type)               // type에 맞게 꺼내서 result에 저장
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

    public Bullet GetBullet() => bulletPool?.GetObject();    // Bullet풀에서 Bullet하나 꺼내는 함수
    public Effect GetHitEffect() => hitPool?.GetObject();   // Effect풀에서 Effect하나 꺼내는 함수
    public Enemy GetEnemy() => enemyPool?.GetObject();    // Enemy풀에서 Enemy하나 꺼내는 함수
    public Effect GetExplosionEffect() => explosionPool?.GetObject();    // ExplosionEffect풀에서 ExplosionEffect하나 꺼내는 함수
}
