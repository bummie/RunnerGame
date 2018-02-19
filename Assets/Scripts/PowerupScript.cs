using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour {

	public float BigSize = 2f;
	public float SmallSize = 0.5f;
	public float Slow = 10f;
	public float Speedup = 20f;

	public GameObject Level;
	public GameObject Suit;
    private IOHandler IO;

    [Header("Sounds")]
    public AudioClip Powerup;
    public AudioClip Powerdown;

    private float _sizePowerupTime;
	private float _speedPowerupTime;
	private float _godModeTime;
	private float _pickedupTime;
	private int _godModeDuration;

	private Vector3 _bigSize;
	private Vector3 _smallSize;

    private AudioSource _aud;

	private bool _pickedUp;
    private bool _godMode = false;

	void Start() {
		_bigSize = new Vector3(BigSize, BigSize, BigSize);
		_smallSize = new Vector3(SmallSize, SmallSize, SmallSize);
        _aud = GetComponent<AudioSource>();
        IO = new IOHandler();
	}

	void OnTriggerEnter(Collider other)
    {
		if (!_pickedUp) {
			switch (other.gameObject.tag) {
				case "Big":
					SizePowerup(_bigSize);
					Destroy(other.gameObject);
					playPowerupSound();
					_pickedUp = true;
					break;
				case "Small":
					SizePowerup(_smallSize);
					Destroy(other.gameObject);
					playPowerupSound();
					_pickedUp = true;
					break;
				case "Slow":
					SpeedPowerup(Slow);
					Destroy(other.gameObject);
					playPowerupSound();
					_pickedUp = true;
					break;
				case "Fast":
					SpeedPowerup(Speedup);
					Destroy(other.gameObject);
					playPowerupSound();
					_pickedUp = true;
					break;
				case "God":
					GodMode(5, true);
					Destroy(other.gameObject);
					playPowerupSound();
					_pickedUp = true;
					break;
				case "Health":
					gameObject.GetComponent<PlayerScript>().healthPickup();
					Destroy(other.gameObject);
					playPowerupSound();
					_pickedUp = true;
					break;
			}
			_pickedupTime = Time.time;
		}
	}

	void SizePowerup(Vector3 size) {
		_sizePowerupTime = Time.time;
		StartCoroutine(ScaleObject(size));
		
	}

	void SpeedPowerup(float speed) {
		_speedPowerupTime = Time.time;
		Level.GetComponent<LevelHandler>().setSegmentMoveSpeed(speed);
	}

	public void GodMode(int duration, bool godModeColor) {
		_godModeDuration = duration;
		_godModeTime = Time.time;
		_godMode = true;
		if (godModeColor) {
			Suit.GetComponent<Light>().enabled = true;
			GetComponent<ParticleSystem>().Play();
		}
	}


	void Update() {
		if(Time.time - _sizePowerupTime > 5) {
			SizePowerup(new Vector3(1f,1f,1f));
		}

		if(Time.time - _speedPowerupTime > 5) {
			SpeedPowerup(Level.GetComponent<LevelHandler>().getDefaultMoveSpeed());
		}
		if(Time.time - _godModeTime > _godModeDuration) {
			_godMode = false;
			Suit.GetComponent<Light>().enabled = false;
			GetComponent<ParticleSystem>().Stop();
		}
		if (Time.time - _godModeTime > _godModeDuration - 0.75) {
			GetComponent<ParticleSystem>().Stop();
		}


		if (Time.time - _pickedupTime < 0.1) {
			_pickedUp = false;
		}
	}

	public bool GetGodMode() {
		return _godMode;
	}

	IEnumerator ScaleObject(Vector3 targetScale) {
		float scaleDuration = 0.7f;                               
		Vector3 actualScale = transform.localScale;             

		for (float t = 0; t < 1; t += Time.deltaTime / scaleDuration) {
			transform.localScale = Vector3.Lerp(actualScale, targetScale, t);
			yield return null;
		}
	}

    private void playPowerupSound()
    {
        if (IO.getSFX())
        {
            _aud.PlayOneShot(Powerup);
        }
    }
}


