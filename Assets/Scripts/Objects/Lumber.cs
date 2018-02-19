using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumber : MonoBehaviour
{
    public GameObject lumberObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            if(lumberObj != null)
                lumberObj.GetComponent<Rigidbody>().isKinematic = false;
    }
}
