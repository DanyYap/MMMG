using UnityEngine;

public class GrabDrop : MonoBehaviour
{
    public Transform handPosition; // The empty GameObject position at player's hand
    private GameObject waterHose; // The object within grab range
    private bool isHoldingObject = false;
    
    

    // Update is called once per frame
    void Update()
    {
        if (waterHose != null && Input.GetKeyDown(KeyCode.E) && !isHoldingObject)
        {
            GrabObject();
        }
        else if (isHoldingObject && Input.GetKeyDown(KeyCode.E))
        {
            ReleaseObject();
        }

    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("grabObject"))
        {
            waterHose = other.gameObject;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("grabObject"))
        {
            waterHose = null;
        }
    }
    
    void GrabObject()
    {
        if (waterHose != null)
        {
            waterHose.transform.SetParent(handPosition); // Attach object to hand
            waterHose.transform.localPosition = Vector3.zero; // Position it at the hand position
            waterHose.transform.localRotation = Quaternion.identity; // Reset rotation
            isHoldingObject = true;
        }
    }
    
    void ReleaseObject()
    {
        if (isHoldingObject)
        {
            waterHose.transform.SetParent(null); // Detach from the hand
            waterHose = null;
            isHoldingObject = false;
        }
    }
    
}
