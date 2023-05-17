using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Portal : MonoBehaviour
{
    [SerializeField] private PortalState portalState;
    private enum PortalState 
    {
        upSpeed,
        downSpeed,
        updameBullet,
        downdameBullet,
        createNumber,
        removeNumber,
        
    }
}