using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leopotam.EcsLite;

public class EnemyBehaviourSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();

        var filter = world.Filter<Enemy>().End();

        var enemiessPool = world.GetPool<Enemy>();
        var tanksPool = world.GetPool<Tank>();
        var tanksMoveDataPool = world.GetPool<TankMoveData>();

        foreach(int entity in filter)
        {
            ref var tank = ref tanksPool.Get(entity);
            ref TankMoveData moveData = ref tanksMoveDataPool.Get(entity);

            if (tank.transform == null)
                continue;

            Bullet? dangerestBullet = DetectBullets(systems, tank.transform.gameObject);

            if (dangerestBullet != null)
            {
                Vector3 moveDirection = MoveFromBulletTrace(dangerestBullet.Value, tank.transform);
                moveData.moveDirection = moveDirection.normalized;
            }
        }
    }

    Bullet? DetectBullets(EcsSystems systems, GameObject tankGameObject)
    {

        EcsWorld world = systems.GetWorld ();

        var filter = world.Filter<Bullet>().End();

        List<Bullet> dangerousBullets = new List<Bullet>();

        float minDistance = 100;
        Bullet? dangerestBullet = null;

        var bulletsPool = world.GetPool<Bullet>();

        foreach(var entity in filter)
        {
            ref var bullet = ref bulletsPool.Get(entity);

            Vector2 orig = new Vector2(bullet.transform.position.x, bullet.transform.position.y);

            Vector2 direction = new Vector2(bullet.movement.x, bullet.movement.y);
            
            Vector2 thisPos = new Vector2(tankGameObject.transform.position.x,  tankGameObject.transform.position.y);

            orig = orig + direction;
            
            RaycastHit2D hit = Physics2D.Raycast(orig, direction);

            if (hit.collider != null)
            {
                if (hit.transform.gameObject == tankGameObject)
                {
                    dangerousBullets.Add(bullet);

                    if (hit.distance < minDistance)
                    {
                        minDistance = hit.distance;

                        dangerestBullet = bullet;
                    }
                }
            }
        }

        return dangerestBullet;
    }

    Vector3 MoveFromBulletTrace(Bullet bullet, Transform transform)
    {
        float distance = (transform.position - bullet.transform.position).magnitude;

        float tna = 1f / distance;
        float tnb = (transform.position.y - bullet.transform.position.y)/(transform.position.x - bullet.transform.position.x);

        float l = Mathf.Sqrt(1 + distance * distance);

        float a = Mathf.Atan(tna);
        float b = Mathf.Atan(tnb);

        float x3 = l*Mathf.Cos(a + b);
        float y3 = l*Mathf.Sin(a + b);

        return new Vector3(bullet.transform.position.x + x3, bullet.transform.position.y + y3, 0);
    }
}
