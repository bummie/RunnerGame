using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawnScript : MonoBehaviour {

	public GameObject[] Powerups;

	private GameObject _powerup;

	void Start()
    {
        spawnRandomPowerup();
    }

    private void spawnRandomPowerup()
    {
        _powerup = Instantiate(Powerups[Random.Range(0, Powerups.Length)]);
        _powerup.transform.parent = gameObject.transform;
        _powerup.transform.localPosition = Vector3.zero;
    }

}
