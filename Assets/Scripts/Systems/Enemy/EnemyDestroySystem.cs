using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leopotam.EcsLite;
public class EnemyDestroySystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();

        var filter = world.Filter<Enemy>().End();

        var enemiesPool = world.GetPool<Enemy>();
        var tanksPool = world.GetPool<Tank>();
        var tanksMoveDataPool = world.GetPool<TankMoveData>();

        foreach(int entity in filter)
        {
            ref var tank = ref tanksPool.Get(entity);

            List<Collider2D> activeColliders = new List<Collider2D>();

            if (tank.boxCollider == null)
                continue;
            
            tank.boxCollider.GetContacts(activeColliders);

            foreach(var collider in activeColliders)
            {
                if (collider.tag == "bullet")
                {
                    var tankGO = tank.transform.gameObject;

                    world.DelEntity(entity);
                    UnityEngine.Object.Destroy(tankGO);
                    break;
                }
            }
        }
    }
}
