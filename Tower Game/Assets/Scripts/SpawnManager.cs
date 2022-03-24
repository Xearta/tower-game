using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private const float ZONE_ANGLE = 72.0f;
    private const float SPAWN_DISTANCE = 20.0f;
    private const int AMN_ZONES = 5;

    public static SpawnManager Instance{ set; get; }
    public GameObject[] enemyPrefabs;

    private int zoneIndex;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            SpawnEnemy(0, zoneIndex);
    }

    public void SpawnEnemy(int prefabIndex, int zoneId)
    {
        float zonePosition = Random.Range(0, ZONE_ANGLE);
        zonePosition += zoneId * ZONE_ANGLE;

        Vector3 direction = Vector3.forward * SPAWN_DISTANCE;
        Quaternion angle = Quaternion.Euler(0, zonePosition, 0);

        Enemy e = (Instantiate(enemyPrefabs[prefabIndex], angle * direction, Quaternion.identity) as GameObject).GetComponent<Enemy>();
        e.LaunchEnemy();

        zoneIndex++;

        if (zoneIndex >= AMN_ZONES)
            zoneIndex = 0;
    }
}
