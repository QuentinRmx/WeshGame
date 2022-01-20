using System.Collections;
using Common;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerController : NetworkSingleton<SpawnerController>
{
    [SerializeField] private GameObject ObjectPrefab;

    [SerializeField] private int MaxObjectInstanceCount;

    [SerializeField, Min(0.005f)]
    private float MinWaitBetweenSpawn = 0.01f;
    
    [SerializeField, Min(0.005f)]
    private float MaxWaitBetweenSpawn = 0.05f;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            NetworkObjectPool.Instance.InitializePool();
        };
    }

    public void SpawnObjects()
    {
        if (!IsServer) return;

        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        for (var i = 0; i < MaxObjectInstanceCount; i++)
        {
            GameObject go = NetworkObjectPool.Instance.GetNetworkObject(ObjectPrefab).gameObject;
            go.transform.position = new Vector3(Random.Range(-10f, 10f), 10.0f, Random.Range(-10f, 10f));
            go.GetComponent<NetworkObject>().Spawn();
            // Random wait between 0.01s and 0.03s to make the spawning of several objects nicer.
            yield return new WaitForSeconds(Random.Range(MinWaitBetweenSpawn, MaxWaitBetweenSpawn));
        }
    }
}