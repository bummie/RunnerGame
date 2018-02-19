using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public GameObject Level;
    public int levelType = 0;
    public GameObject[] levelForrestSegements, levelDesertSegements, levelWinterSegements;
    private Vector3 _levelStartPos;

    private float _updateDelay = 0f;
    private float _segmentMoveSpeed = 15f;
    private bool _shouldMoveSegments = true;
    private float _segmentSize = 50f;
    private float _currentMoveSpeed = 0f;

	private const float _defaultMoveSpeed = 15f;

	private int index;
	private int _lastLoadedSegment;

    private GameObject[] loadedSegments;

    // Timer
    private float _lastTime;

    void Start()
    {
        _levelStartPos = new Vector3(7f, -0.05f, 0f);
        loadedSegments = new GameObject[3];

        loadSegments();

        _lastTime = 0f;
        _currentMoveSpeed = _defaultMoveSpeed;
    }

    void Update()
    {
        _lastTime += Time.deltaTime;
        if (_lastTime >= _updateDelay)
        {
            updateSegments();
            moveSegments();
            _lastTime = 0f;
        }
    }

    // Lastet inn segmentene
    private void loadSegments()
    {
        Vector3 newPos = _levelStartPos;
        for (int i = 0; i < loadedSegments.Length; i++)
        {
            newPos.z = _segmentSize * i;
            if (i == 0)
                loadedSegments[i] = spawnSegment(levelForrestSegements[0]);
            else
                loadedSegments[i] = spawnSegment(newPos);
        }
    }

    // Oppdaterer segmentene, putter inn nye om de er ferdige
    private void updateSegments()
    {
        Vector3 newPos = _levelStartPos;
        newPos.z = 2 * _segmentSize;
        for (int i = 0; i < loadedSegments.Length; i++)
        {
            if (loadedSegments[i] != null)
            {
                if (loadedSegments[i].transform.position.z <= -_segmentSize)
                {
                    Destroy(loadedSegments[i]);
                    loadedSegments[i] = spawnSegment(newPos);
                }
            }
        }
    }

    // Flytter segmentene
    private void moveSegments()
    {
        if (_shouldMoveSegments)
        {
            for (int i = 0; i < loadedSegments.Length; i++)
            {
				loadedSegments[i].transform.Translate(0f, 0f, -_segmentMoveSpeed * Time.deltaTime);
            }
        }
    }

    // Spawner et tilfeldig segment
    private GameObject spawnSegment(Vector3 pos)
    {
        if (Level != null)
        {
            GameObject segment = Instantiate(getRandomSegment(), pos, Quaternion.identity);
            //segment.transform.localPosition = posYo;
            segment.transform.parent = Level.transform;
            segment.name = segment.name;

            return segment;
        }
        else
            return null;
    }

    private GameObject spawnSegment(GameObject startSegment)
    {
        if (Level != null)
        {
            GameObject segment = Instantiate(startSegment);
            segment.transform.position = _levelStartPos;
            segment.transform.parent = Level.transform;
            segment.name = segment.name;
            return segment;
        }
        else
            return null;
    }

	private GameObject getRandomSegment() {
	
		while(index == _lastLoadedSegment) {
			index = Random.Range(1, levelForrestSegements.Length);
		}
		_lastLoadedSegment = index;
		return levelForrestSegements[index];
	}

    public void shouldMoveLevel(bool shouldMove)
    {
        _shouldMoveSegments = shouldMove;
    }

    public void setSegmentMoveSpeed(float speed)
    {
        _segmentMoveSpeed = speed;
        _currentMoveSpeed = speed;
    }

    public void setPlayerMoveSpeed(float speed)
    {
        _segmentMoveSpeed = speed;
    }

    public float getDefaultMoveSpeed()
    {
		return _defaultMoveSpeed;
	}

    public float getCurrentMoveSpeed()
    {
        return _currentMoveSpeed;
    }
}
