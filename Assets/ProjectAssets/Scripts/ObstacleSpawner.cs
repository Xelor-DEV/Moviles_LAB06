using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private ObjectPoolStatic pool;
    [SerializeField] private Transform spawnXPosition;
    [SerializeField] private float spawnInterval = 2f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        Spawn();
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnRoutine());
    }

    private void Spawn()
    {
        float yPos = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0, 0.1f, 0)).y,mainCamera.ViewportToWorldPoint(new Vector3(0, 0.9f, 0)).y);

        Vector3 spawnPos = new Vector3(spawnXPosition.position.x, yPos, spawnXPosition.position.z);

        pool.GetObject(spawnPos);
    }
}