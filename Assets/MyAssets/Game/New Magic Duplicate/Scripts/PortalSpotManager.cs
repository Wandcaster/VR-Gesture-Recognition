using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpotManager : MonoBehaviour
{
    List<PortalSpotController> portalSpotControllers;
    public bool portalIsActive;
    private void Start()
    {
        portalSpotControllers= new List<PortalSpotController>(FindObjectsOfType<PortalSpotController>());
        foreach (var portalSpot in portalSpotControllers)
        {
            portalSpot.statusChanged.AddListener(TryActivePortal);
        }
    }
    /// <summary>
    /// Check all portalSpot power status
    /// </summary>
    /// <returns>True if all potal spots are powered</returns>
    public bool CheckPortalStatus()
    {
        bool result = true;
        foreach (PortalSpotController portalSpot in portalSpotControllers)
        {
            if(!portalSpot.IsPowered)result= false;
        }
        return result;
    }
    public void TryActivePortal()
    {
        if (CheckPortalStatus())portalIsActive= true;
        else portalIsActive= false;
    }
}
