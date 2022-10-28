using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonShooter : MonoBehaviour
{
    [SerializeField] CanonInputManager canonInputManager;

    private int shootMouseButton = 0;

    private void Start()
    {
        canonInputManager.OnMouseButtonRelease += CanonInputManager_OnMouseButtonRelease;
    }
    private void CanonInputManager_OnMouseButtonRelease(int mouseButton)
    {
        if (mouseButton == shootMouseButton) Shoot();
    }
    private void Shoot()
    {
        Debug.Log("Shoot!");
    }  
}
