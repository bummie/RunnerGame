using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    [Header("Player")]
    public float StrafeSpeed = 4f;
    public float JumpStrength = 10f;
	public GameObject Suit;

    [Header("Other")]
    public GameObject LevelHandler;

    [Header("Sounds")]
    public AudioClip Jump;
    public AudioClip DoubleJump;
    public AudioClip Death;
    public AudioClip Hurt;
    public AudioClip Falling;
    public AudioClip Running;
    public AudioClip Health;
    public AudioClip Coin;
    public AudioClip Trampoline;

    private Animator _animator;
    private bool _jumping;
    private bool _canControl = true;
    private Vector3 camPos;
    private IOHandler IO;


    private bool _isDead = false;
    private int _coins = 0, _health = 3;
    private bool _canDoubleJump = true;
    private Color _damageColor = new Color(1f, 0f, 0f);
    private bool _takenDamage = false;
    private float _damageTime = 1.3f;
    private float _damageTimer = 0f;
    private int _lastHurtObjectId = -1;

    private AudioSource _aud;
    private float _fallingTime = 0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        camPos = Camera.main.transform.position;
        _aud = GetComponent<AudioSource>();
        IO = new IOHandler();
    }

    private void Update()
    {
        playSoundFalling();
        playRunningLoop();

        if (_takenDamage)
        {
            _damageTimer += Time.deltaTime;
            if(_damageTimer >= _damageTime)
            {
                _takenDamage = false;
                _damageTimer = 0f;
                Suit.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
            }
        }

        // Get abillity to doublejump when grounded again
        if (!_canDoubleJump)
        {
            if (onGround())
                _canDoubleJump = true;
        }

        // Camera follow Y-coord of player
        Camera.main.transform.position = new Vector3(camPos.x, camPos.y + gameObject.transform.position.y , camPos.z);
        if (_canControl)
        {
            float xMove = Input.GetAxis("Horizontal") * Time.deltaTime * StrafeSpeed;
            float zMove = Input.GetAxis("Vertical") * Time.deltaTime * 200;
            //Debug.Log("zMove: " + zMove)    ;
            LevelHandler.GetComponent<LevelHandler>().setPlayerMoveSpeed(LevelHandler.GetComponent<LevelHandler>().getCurrentMoveSpeed() + zMove+5);
            transform.Translate(xMove, 0f, 0f);

            if (transform.position.x > 5)
            {
                transform.Translate(5f - transform.position.x, 0f, 0f);
            }
            else if (transform.position.x < -4)
            {
                transform.Translate(-4f - transform.position.x, 0f, 0f);
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (!_jumping && onGround())
                {
                    playSound(Jump, false);
                    jump();
                }
                else if (_canDoubleJump && !onGround())
                {
                    playSound(DoubleJump, false);
                    _animator.SetTrigger("Jump");
                    jump();
                    _canDoubleJump = false;
                }
            }

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                _jumping = false;
            }
            else
            {
                _jumping = true;
            }
        }

        // Om spiller faller sett falleanimasjon
        //_animator.SetBool("Falls", (!_jumping && !onGround() ? true : false));
        _animator.SetBool("Falls", (!_jumping && !onGround() && GetComponent<Rigidbody>().velocity.y <= 0 ? true : false));

        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeathZone")
        {
            setDead(true);
        }
        else if (other.gameObject.tag == "HurtZone")
        {
            other.GetComponent<Collider>().enabled = false;
            if (_lastHurtObjectId != other.GetInstanceID())
            {
                    takeDamage();
            }
            _lastHurtObjectId = other.GetInstanceID();
        }
    }

    // Returns true if player on the ground
    private bool onGround()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 1f);
    }

	// Deal damage
	public void takeDamage() {

		if (!GetComponent<PowerupScript>().GetGodMode() && !_takenDamage) {
				
			playSound(Hurt, false);
			_health -= 1;

			GetComponent<PowerupScript>().GodMode(2, false);
			GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);

			_takenDamage = true;
			
			Suit.GetComponent<Renderer>().material.color = _damageColor;

			if (_health <= 0) {
				setDead(true);
			}
		}
	}

    // Getters and setters
    public bool isDead()
    {
        return _isDead;
    }

    public void setDead(bool isDead)
    {
        _isDead = isDead;
        if (_isDead)
        {
            _health = 0;
            _animator.SetTrigger("Dies");
            _canControl = false;
            playSound(Death, false);
        }
    }

    public void coinPickup()
    {
        playSound(Coin, false);
        _coins++;
    }

    public int getCoins()
    {
        return _coins;
    }

    public void setCoins(int coins)
    {
        _coins = coins;
    }

    public void healthPickup()
    {
        playSound(Health, false);
		if (getHealth() <= 2)
			_health += 1;
    }

    public int getHealth()
    {
        return _health;
    }

    public void setHealth(int health)
    {
        _health = health;
    }

    private void jump()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().AddForce(Vector3.up * JumpStrength, ForceMode.Impulse);
    }

    public void playSound(AudioClip clip, bool loop)
    {
        if (IO.getSFX())
        {
            _aud.loop = loop;
            _aud.PlayOneShot(clip);
        }    
    }

    private void playRunningLoop()
    {
        if (onGround() && _aud.isPlaying != Running && !isDead())
        {
            playSound(Running, false);
            _fallingTime = 0f;
        }
    }

    private void playSoundFalling()
    {
        if (!onGround() && GetComponent<Rigidbody>().velocity.y <= 0)
        {
            _fallingTime += Time.deltaTime;
            if (_fallingTime >= 1 && transform.position.y <= -3)
            {
                playSound(Falling, false);
                _fallingTime = 0f;
            }
            
        }
    }

    public AudioSource getAudioSource()
    {
        return _aud;
    }
}
