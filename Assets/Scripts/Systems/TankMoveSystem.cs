using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leopotam.EcsLite;

public class TankMoveSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();

        var filter = world.Filter<Tank>().Inc<TankMoveData>().End();

        var tanksPool = world.GetPool<Tank>();
        var tanksMoveDataPool = world.GetPool<TankMoveData>();

        foreach (var entity in filter)
        {
            ref var tank = ref tanksPool.Get(entity);
            ref var tankMoveData = ref tanksMoveDataPool.Get(entity);

            if (tank.transform == null)
                continue;

            Vector3 newPosition = tank.transform.position + tankMoveData.moveDirection * tank.speed;
            tank.transform.position = Vector3.Slerp(tank.transform.position, newPosition, Time.deltaTime * 1f);

            Vector3 vectorToTarget = tankMoveData.orientation - tank.transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            tank.transform.rotation = Quaternion.Slerp(tank.transform.rotation, q, Time.deltaTime * tank.rotationSpeed);
        }
    }
}
