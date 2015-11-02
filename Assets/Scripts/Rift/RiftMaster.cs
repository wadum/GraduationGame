using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RiftMaster : MonoBehaviour {

    public GameObject gb1, gb2;
    private List<Transform> Portals;
    private RiftScript rs1, rs2;

    // Use this for initialization
    void Start () {
        // Sidekick needs to know location of all rifts.
        Portals = new List<Transform>();
        GameObject[] TmpPortals = GameObject.FindGameObjectsWithTag("PortalRift");
        foreach (GameObject portal in TmpPortals)
        {
            if (portal.GetComponent<RiftScript>().active)
                TagRift(portal);
            Portals.Add(portal.transform);
        }
    }

    public void TagRift(GameObject obj)
    {
        // No other rifts are active, so activate this rift
        if (!gb1)
        {
            gb1 = obj;
            rs1 = gb1.GetComponent<RiftScript>();
            rs1.active = true;
        }
        else
        {
            // Rift is open, and needs to close.
            if (obj == gb1)
            {
                Clear(gb1);
                return;
            }
            if (obj == gb2)
            {
                Clear(gb2);
                return;
            }
            if (gb2)
            {
                Clear(gb1);
            }
            gb2 = obj;
            rs2 = gb2.GetComponent<RiftScript>();
            rs2.active = true;
            rs1.AssignPartner(rs2);
            rs2.AssignPartner(rs1);
        }
    }

    // Closes the GameObject
    void Clear(GameObject obj)
    {
        // If first rift is closed, second rift needs to become the new first.
        if (obj == gb1)
        {
            rs1.active = false;
            rs1.ClearPartner();
            if (gb2)
            {
                gb1 = gb2;
                rs1 = rs2;
                rs2.ClearPartner();
                gb2 = null;
            }
            else
            {
                rs1.ClearPartner();
                gb1 = null;
            }
        }
        if (obj == gb2)
        {
            rs1.ClearPartner();
            rs2.ClearPartner();
            rs2.active = false;
            gb2 = null;
        }
    }
}