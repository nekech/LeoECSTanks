using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public class PlayerInputSystem : IEcsRunSystem
{
    float MovementSpeed = 1f;
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();
        var filter = world.Filter<Player>().Inc<TankMoveData>().End ();

        var tanksMoveDataPool = world.GetPool<TankMoveData>();

        foreach (int entity in filter)
        {
            ref TankMoveData input = ref tanksMoveDataPool.Get (entity);

            Vector3 movement = new Vector3(0, 0, 0);
            if (Input.GetKey("w"))
            {
                movement += new Vector3(0, 1, 0);
            }
            if (Input.GetKey("s"))
            {
                movement += new Vector3(0, -1, 0);
            }
            if (Input.GetKey("d"))
            {
                movement += new Vector3(1, 0, 0);
            }
            if (Input.GetKey("a"))
            {
                movement += new Vector3(-1, 0, 0);
            }

            input.moveDirection = movement;

            input.orientation = Camera.main.ScreenToWorldPoint(Input.mousePosition); 

            if (Input.GetMouseButtonDown(0))
            {
                var shootsDataPool = world.GetPool<ShootData>();
                shootsDataPool.Add(entity);
            }
        }
    }
}

