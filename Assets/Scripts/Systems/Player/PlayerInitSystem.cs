using Leopotam.EcsLite;
using UnityEngine;

public class PlayerInitSystem : IEcsInitSystem
{
	private SceneData sceneData;

  	public void Init(EcsSystems systems)
  	{
        EcsWorld world = systems.GetWorld ();
        int playerEntity = world.NewEntity();

        var playerPool = world.GetPool<Player>();
        playerPool.Add(playerEntity);

        var tanksMoveDataPool = world.GetPool<TankMoveData>();
        tanksMoveDataPool.Add(playerEntity);

        var tanksPool = world.GetPool<Tank>();
        tanksPool.Add(playerEntity);

        sceneData = systems.GetShared<SceneData>(); 
        
        GameObject playerGO = Object.Instantiate(sceneData.playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        var filter = world.Filter<Player> ().Inc<Tank>().End ();

        foreach (int entity in filter)
        {
            ref Player player = ref playerPool.Get (entity);
            ref Tank tank = ref tanksPool.Get (entity);

            tank.transform = playerGO.transform;
            tank.speed = sceneData.playerSpeed;
            tank.rotationSpeed = sceneData.playerRotationSpeed;

            tank.boxCollider = playerGO.GetComponent<BoxCollider2D>();
        }
  	}
}
