using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leopotam.EcsLite;

public class EcsStartup : MonoBehaviour
{
    private EcsWorld ecsWorld;
    private EcsSystems initSystems;
    private EcsSystems updateSystems;
    private EcsSystems fixedUpdateSystems;
    public SceneData sceneData;
 
    private void Start()
    {
        ecsWorld = new EcsWorld();

        initSystems = new EcsSystems(ecsWorld, sceneData); 
        updateSystems = new EcsSystems(ecsWorld, sceneData);
        fixedUpdateSystems = new EcsSystems(ecsWorld, sceneData);
        
    	
        initSystems
            .Add(new PlayerInitSystem())
            .Add(new EnemyInitSystem())
            .Init();

        updateSystems
            .Add(new PlayerInputSystem())
            .Add(new ShootSystem())
            .Add(new BulletDestroySystem())
            .Add(new EnemyDestroySystem())
            .Add(new EnemyBehaviourSystem())
            .Add(new EnemyShutSystem())
            .Init();

        fixedUpdateSystems
            .Add(new TankMoveSystem())
            .Add(new BulletMoveSystem())
            .Init();
    }
 
    private void Update()
    {
        updateSystems?.Run();
    }

    private void FixedUpdate()
    {
        fixedUpdateSystems?.Run();
    }
 
    private void OnDestroy()
    {
        initSystems?.Destroy();
        initSystems = null;
        
        updateSystems?.Destroy();
        updateSystems = null;

        fixedUpdateSystems?.Destroy();
        fixedUpdateSystems = null;

        ecsWorld?.Destroy();
        ecsWorld = null;
    }
}
