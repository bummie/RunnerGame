using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Move")]
    public float MoveMax = 1f;
    public float MoveMin = 0f;
    public float IncrementSpeed = 0.2f;

    [Header("Axis X:0, Y:1, Z:2")]
    public int Axis = 0; // X, Y, Z

    private float _time = 0.0f;
	
	void Update ()
    {
        // Velge axsen plattformen skal bevege seg på
        switch (Axis)
        {
            case 0:
                transform.localPosition = new Vector3(Mathf.Lerp(MoveMin, MoveMax, _time), transform.localPosition.y, transform.localPosition.z);
                break;
            case 1:
                transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(MoveMin, MoveMax, _time), transform.localPosition.z);
                break;
            case 2:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(MoveMin, MoveMax, _time));
                break;
        }

        _time += IncrementSpeed * Time.deltaTime;

        if (_time > 1.0f)
        {
            float t = MoveMax;
            MoveMax = MoveMin;
            MoveMin = t;
            _time = 0.0f;
        }
    }
}
