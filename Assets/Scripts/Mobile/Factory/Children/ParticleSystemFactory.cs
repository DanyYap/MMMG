using System.Collections.Generic;
using UnityEngine;

// Factory for creating and reusing particle systems by key, with configurable initial pool sizes.
[CreateAssetMenu(fileName = "ParticleSystemFactory", menuName = "Scriptable Objects/ParticleSystemFactory")]
public class ParticleSystemFactory : ScriptableFactoryBase<ParticleSystem>
{
    [System.Serializable]
    public class ParticleEntry
    {
        public string key; // Name to identify the particle (e.g., "Explosion").
        public GameObject prefab; // The prefab (GameObject) to create.
        public int initialPoolSize = 5; // Number of prefabs to pre-create for this key.
    }

    [SerializeField] private List<ParticleEntry> particlePrefabs; // List of all particles.
    private Dictionary<string, GameObject> prefabDictionary; // Quick lookup for prefabs.
    private Dictionary<string, Queue<GameObject>> poolDictionary; // Pools for each key.

    private void OnEnable()
    {
        // Initialize dictionaries for prefabs and pools.
        prefabDictionary = new Dictionary<string, GameObject>();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var entry in particlePrefabs)
        {
            if (!prefabDictionary.ContainsKey(entry.key))
            {
                prefabDictionary.Add(entry.key, entry.prefab);

                // Create a pool and populate it with the initial size.
                var pool = new Queue<GameObject>();
                poolDictionary.Add(entry.key, pool);

                for (int i = 0; i < entry.initialPoolSize; i++)
                {
                    var instance = Instantiate(entry.prefab);
                    instance.SetActive(false); // Keep inactive until needed.
                    pool.Enqueue(instance); // Add to the pool.
                }
            }
        }
    }

    public override GameObject CreateInstance(string key, Vector3 position, Quaternion rotation)
    {
        if (prefabDictionary.TryGetValue(key, out var prefab))
        {
            var pool = poolDictionary[key];

            // Reuse a GameObject from the pool if available.
            if (pool.Count > 0)
            {
                var particleObject = pool.Dequeue();
                particleObject.transform.SetPositionAndRotation(position, rotation);
                particleObject.SetActive(true);

                // Ensure the GameObject has the ParticleSystem component.
                if (particleObject.GetComponent<ParticleSystem>() == null)
                {
                    Debug.LogError($"The prefab for key '{key}' does not contain a ParticleSystem component!");
                    return null;
                }

                return particleObject; // Return the GameObject with ParticleSystem attached.
            }

            // If the pool is empty, create a new GameObject and check for the component.
            var newParticleObject = Instantiate(prefab, position, rotation);
            if (newParticleObject.GetComponent<ParticleSystem>() == null)
            {
                Debug.LogError($"The prefab for key '{key}' does not contain a ParticleSystem component!");
                return null;
            }

            return newParticleObject; // Return the GameObject with ParticleSystem attached.
        }

        Debug.LogError($"Particle prefab with key '{key}' not found!");
        return null;
    }

    public void ReturnToPool(string key, GameObject particleObject)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogError($"No pool exists for key '{key}'!");
            return;
        }

        particleObject.SetActive(false); // Hide the particle system.
        poolDictionary[key].Enqueue(particleObject); // Add it back to the pool.
    }
}
