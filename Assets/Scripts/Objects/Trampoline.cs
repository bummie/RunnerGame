using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public int TrampolineStrength = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * TrampolineStrength, ForceMode.Impulse);
            other.gameObject.GetComponent<PlayerScript>().playSound(other.gameObject.GetComponent<PlayerScript>().Trampoline, false);
        }
    }
}
