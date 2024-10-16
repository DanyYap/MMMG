using UnityEngine;

// This class creates the fire particles
public class FireParticleManager : MonoBehaviour
{
    // This method makes fire particles appear on the target
    public GameObject CreateFireParticles(GameObject target, GameObject firePrefab)
    {
        GameObject fireInstance = Instantiate(
            firePrefab, 
            target.transform.position, 
            Quaternion.Euler(-90, 0, 0), 
            target.transform);

        fireInstance.transform.localPosition = Vector3.zero; // Make sure fire is on the object

        return fireInstance;
    }
}
