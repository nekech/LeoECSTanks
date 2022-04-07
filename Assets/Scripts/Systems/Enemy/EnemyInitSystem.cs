using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leopotam.EcsLite;

public class EnemyInitSystem : IEcsInitSystem
{
    private SceneData sceneData;

    public void Init(EcsSystems systems)
  	{
        EcsWorld world = systems.GetWorld ();
        sceneData = systems.GetShared<SceneData>();

        for (int i = 0; i < sceneData.enemiesCount; ++i)
        {
            int enemyEntity = world.NewEntity();

            var enemiesPool = world.GetPool<Enemy>();
            enemiesPool.Add(enemyEntity);

            var tanksMoveDataPool = world.GetPool<TankMoveData>();
            tanksMoveDataPool.Add(enemyEntity);

            var tanksPool = world.GetPool<Tank>();
            tanksPool.Add(enemyEntity);

            float randX = Random.RandomRange(-5, 5);
            float randY = Random.RandomRange(-5, 5);
            
            GameObject enemyGO = Object.Instantiate(sceneData.enemyPrefab, new Vector3(randX, randY, 0), Quaternion.identity);
            var filter = world.Filter<Enemy> ().Inc<Tank>().End ();

            foreach (int entity in filter)
            {
                ref Enemy enemy = ref enemiesPool.Get (entity);
                ref Tank tank = ref tanksPool.Get (entity);

                tank.transform = enemyGO.transform;
                tank.speed = sceneData.enemySpeed;
                tank.rotationSpeed = sceneData.enemyRotationSpeed;
                tank.boxCollider = enemyGO.GetComponent<BoxCollider2D>();
            }
        }
  	}
}
