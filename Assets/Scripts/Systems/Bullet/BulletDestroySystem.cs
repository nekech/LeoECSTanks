using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leopotam.EcsLite;
public class BulletDestroySystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();

        var filter = world.Filter<Bullet>().End();

        var bulletsPool = world.GetPool<Bullet>();

        foreach(int entity in filter)
        {
            ref var bullet = ref bulletsPool.Get(entity);

            List<Collider2D> activeColliders = new List<Collider2D>();
            
            bullet.rigidBody.GetContacts(activeColliders);

            foreach(var collider in activeColliders)
            {
                if (collider.tag == "bullet")
                {
                    UnityEngine.Object.Destroy(bullet.transform.gameObject);
                    bulletsPool.Del(entity);
                }
            }
        }
    }
}
