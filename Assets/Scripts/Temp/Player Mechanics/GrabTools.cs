using UnityEngine;

public class GrabTools : MonoBehaviour
{
    public Transform handPosition;
    public ParticleSystem grabParticleSystem;
    private GameObject objectInRange;
    [SerializeField] bool isHoldingObject = false;

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("grabObject"))
        {
            objectInRange = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("grabObject"))
        {
            objectInRange = null;
        }
    }

    public void GrabObject()
    {
        if (objectInRange != null)
        {
            objectInRange.transform.SetParent(handPosition); 
            objectInRange.transform.localPosition = Vector3.zero; 
            objectInRange.transform.localRotation = Quaternion.identity; 
            isHoldingObject = true;
        }
    }

    void ReleaseObject()
    {
        if (isHoldingObject)
        {
            objectInRange.transform.SetParent(null);
            objectInRange = null;
            isHoldingObject = false;
        }
    }
    
    void UseWaterHose()
    {
        if (grabParticleSystem != null)
        {
            grabParticleSystem.Play();
        }
        else
        {
            grabParticleSystem.Stop();
        }
    }
    
}
