using Common;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerController : NetworkSingleton<SpawnerController>
{
    [SerializeField] private GameObject ObjectPrefab;

    [SerializeField] private int MaxObjectInstanceCount;

    private void Awake()
    {
        Debug.Log(NetworkManager);
        NetworkManager.Singleton.OnServerStarted += () => { 
            Debug.Log(NetworkObjectPool.Instance.name);
        NetworkObjectPool.Instance.InitializePool(); };
    }

    public void SpawnObjects()
    {
        if (!IsServer) return;

        for (int i = 0; i < MaxObjectInstanceCount; i++)
        {
            GameObject go = NetworkObjectPool.Instance.GetNetworkObject(ObjectPrefab).gameObject;
            go.transform.position = new Vector3(Random.Range(-10f, 10f), 10.0f, Random.Range(-10f, 10f));
            go.GetComponent<NetworkObject>().Spawn();
            // Pooling.
        }
    }
}