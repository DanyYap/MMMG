using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

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
    private Dictionary<string, List<GameObject>> activeParticles; // Track active particles in the scene

    private void OnEnable()
    {
        // Initialize dictionaries for prefabs, pools, and active particles.
        prefabDictionary = new Dictionary<string, GameObject>();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        activeParticles = new Dictionary<string, List<GameObject>>();

        // Load existing particles from the current scene (if any).
        foreach (var entry in particlePrefabs)
        {
            prefabDictionary.Add(entry.key, entry.prefab);

            // Initialize pool and create initial instances.
            var pool = new Queue<GameObject>();
            poolDictionary.Add(entry.key, pool);
            activeParticles.Add(entry.key, new List<GameObject>());

            for (int i = 0; i < entry.initialPoolSize; i++)
            {
                var instance = Instantiate(entry.prefab);
                instance.SetActive(false); // Keep inactive until needed
                pool.Enqueue(instance); // Add to the pool
            }
        }

        // Register to listen for scene changes to clean up old effects and initialize new ones.
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneUnloaded(Scene currentScene)
    {
        // Cleanup all active particles when the scene is unloaded.
        foreach (var particles in activeParticles.Values)
        {
            foreach (var particle in particles)
            {
                if (particle != null)
                {
                    Destroy(particle); // Destroy any active particles that were in the previous scene.
                }
            }
        }
        activeParticles.Clear(); // Clear the list of active particles for the old scene.
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reinitialize particle pools and reattach any particles if necessary.
        // This could be used for ensuring that certain particles are ready or reactivate any needed instances.

        // For example, we could load particles that are needed for the current scene:
        foreach (var entry in particlePrefabs)
        {
            // Check if there's any particle system we want to initialize for the new scene.
            if (!activeParticles.ContainsKey(entry.key))
            {
                activeParticles.Add(entry.key, new List<GameObject>());
            }

            // Re-add to the pool if there were any leftover particles.
            var pool = poolDictionary[entry.key];
            for (int i = pool.Count; i < entry.initialPoolSize; i++)
            {
                var instance = Instantiate(entry.prefab);
                instance.SetActive(false); // Keep inactive until needed.
                pool.Enqueue(instance); // Add to the pool.
            }
        }

        // Optionally, you could reactivate specific particles if needed here.
    }

    public override GameObject CreateInstance(string key, Vector3? position = null, Quaternion? rotation = null)
    {
        // Default position and rotation if not provided.
        Vector3 spawnPosition = position ?? Vector3.zero;  // Default to (0,0,0) if null
        Quaternion spawnRotation = rotation ?? Quaternion.identity;  // Default to no rotation if null

        if (prefabDictionary.TryGetValue(key, out var prefab))
        {
            var pool = poolDictionary[key];

            // Reuse a GameObject from the pool if available.
            if (pool.Count > 0)
            {
                var particleObject = pool.Dequeue();

                // Check if the particleObject has been destroyed or is inactive
                if (particleObject == null)
                {
                    Debug.LogWarning($"Particle object for key '{key}' has been destroyed or is inactive, creating a new one.");
                    // Renamed variable to avoid conflict
                    var newParticleObjectFromPool = Instantiate(prefab, spawnPosition, spawnRotation);
                    activeParticles[key].Add(newParticleObjectFromPool); // Track new particle object in the scene
                    return newParticleObjectFromPool;
                }

                particleObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
                particleObject.SetActive(true);

                // Track active particle in the current scene
                activeParticles[key].Add(particleObject);
                return particleObject;
            }

            // If the pool is empty, create a new GameObject and check for the component.
            var newParticleObject = Instantiate(prefab, spawnPosition, spawnRotation);
            activeParticles[key].Add(newParticleObject); // Track new particle object in the scene
            return newParticleObject;
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
        activeParticles[key].Remove(particleObject); // Remove from active particles list
    }

    private void OnDisable()
    {
        // Unregister from scene change events.
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
