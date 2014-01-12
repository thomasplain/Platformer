using UnityEngine;
using System.Collections;

public class cameraMovement : MonoBehaviour {

	private Transform player;
	private float nominalDistanceToPlayer;
	private Vector3 relativeCameraPosition;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		relativeCameraPosition = transform.position - player.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 newPosition = Vector3.Lerp (transform.position, player.position + relativeCameraPosition, 0.8f);
		transform.position = newPosition;
	}
}
