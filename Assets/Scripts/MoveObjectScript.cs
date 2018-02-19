
using UnityEngine;

public class MoveObjectScript : MonoBehaviour {

	void Update() {
		float offset = 10f * Time.deltaTime;
		transform.Translate(0, 0, -offset);
	}
}
