using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    public Transform playerSpawnPoint;
    public float playerSpeed = 1f;
    public float playerRotationSpeed = 1f;

    public GameObject playerPrefab;

    public int enemiesCount = 1;
    public GameObject enemyPrefab;

    public float enemySpeed = 1f;
    public float enemyRotationSpeed = 1f;

    public GameObject bulletPrefab;
    public float bulletSpeed = 1f;
}
