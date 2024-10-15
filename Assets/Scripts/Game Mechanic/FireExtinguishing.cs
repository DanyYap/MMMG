using UnityEngine;

public class FireExtinguishing : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide with object");
    }
}
