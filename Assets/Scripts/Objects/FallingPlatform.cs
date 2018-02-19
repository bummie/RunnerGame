using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

    private float _timeDelay = .005f;
    private float _timer = 0f;
    private bool _shouldFall = false;
    private void Update()
    {
        if (_shouldFall)
        {
            _timer += Time.deltaTime;
            if (_timer >= _timeDelay)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                _shouldFall = false;
            } 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _shouldFall = true;
        }
    }
}
