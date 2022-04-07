using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leopotam.EcsLite;
public class EnemyShutSystem : IEcsRunSystem
{
    EcsWorld world;
    public void Run(EcsSystems systems)
    {
        world = systems.GetWorld ();

        var filter = world.Filter<Enemy>().End();

        var enemiessPool = world.GetPool<Enemy>();
        var tanksPool = world.GetPool<Tank>();
        var tanksMoveDataPool = world.GetPool<TankMoveData>();

        var playerFilter = world.Filter<Player>().End();
        int playerEntity = 0;

        foreach(int entity in filter)
        {
            playerEntity = entity;
        }

        ref Tank playerTank = ref tanksPool.Get(playerEntity);

        foreach(int entity in filter)
        {
            ref var tank = ref tanksPool.Get(entity);
            ref TankMoveData moveData = ref tanksMoveDataPool.Get(entity);

            if (tank.transform == null)
                continue;

            Vector3 diffRotation = GetAngleToTarget(playerTank.transform, tank.transform);

            moveData.orientation = playerTank.transform.position;
        }
    }

    Vector3 GetAngleToTarget(Transform target, Transform tank)
    {
        Vector3 vectorToTarget = target.position - tank.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector3 diffRotation = tank.rotation.eulerAngles - q.eulerAngles;

        return diffRotation;
    }
}
