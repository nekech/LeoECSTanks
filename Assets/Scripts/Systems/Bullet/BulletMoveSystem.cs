using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leopotam.EcsLite;
public class BulletMoveSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();

        var filter = world.Filter<Bullet>().End();

        var bulletsPool = world.GetPool<Bullet>();

        foreach(int entity in filter)
        {
            ref var bullet = ref bulletsPool.Get(entity);

            Vector2 offset2d = new Vector2(bullet.movement.x, bullet.movement.y) * bullet.speed;

            Vector2 newPosition = bullet.rigidBody.position + offset2d * Time.deltaTime;

            bullet.rigidBody.MovePosition(newPosition);

            if (newPosition.x > 10 || newPosition.x < -10 || newPosition.y > 10 || newPosition.y < -10)
            {
                UnityEngine.Object.Destroy(bullet.transform.gameObject);
                bulletsPool.Del(entity);
            }

        }
    }
}
