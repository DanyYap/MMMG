using System;
using UnityEngine;

public class HostageRescue : MonoBehaviour
{
    public ScoreSystem _scoreSystem;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hostage"))
        {
            Debug.Log("Hostage Rescue");
            other.gameObject.SetActive(false);
            _scoreSystem.OnHostageRescue();
        }
    }
}
