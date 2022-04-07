using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leopotam.EcsLite;
public class ShootSystem : IEcsRunSystem
{
    private SceneData sceneData;
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();

        var bulletsPool = world.GetPool<Bullet>();
        var tanksPool = world.GetPool<Tank>();
        var shootsDataPool = world.GetPool<ShootData>();

        var filter = world.Filter<Tank>().Inc<ShootData>().End();

        sceneData = systems.GetShared<SceneData>(); 

        foreach (int entity in filter)
        {
            GameObject bulletGO = Object.Instantiate(sceneData.bulletPrefab);

            ref Tank tank = ref tanksPool.Get(entity);

            int bulletEntity = world.NewEntity();
            bulletsPool.Add(bulletEntity);

            ref Bullet bullet = ref bulletsPool.Get(bulletEntity);

            bullet.speed = sceneData.bulletSpeed;
            bullet.rigidBody = bulletGO.GetComponent<Rigidbody2D>();
            bullet.transform = bulletGO.transform;
            bullet.transform.position = tank.transform.localToWorldMatrix * new Vector4(1, 0, 0, 1f);

            bullet.movement = tank.transform.right;

            bullet.destroy = false;

            shootsDataPool.Del(entity);
        }
    }
}
