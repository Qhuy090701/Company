using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Portal : MonoBehaviour
{
    [SerializeField] private PortalState portalState;
    private enum PortalState 
    {
        LevelUp,
        LevelDown,
    }

    private void Update()
    {
        switch (portalState)
        {
            case PortalState.LevelUp:
                UpdateLevelUp();
                break;
            case PortalState.LevelDown:
                UpdateLevelDown();
                break;
            default:
                break;
        }
    }


    private void UpdateLevelUp()
    {

    }

    private void UpdateLevelDown()
    {

    }

}