using UnityEngine;
using System.Collections;

public class projectileMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void Update()
	{
		if (!renderer.isVisible)
			Destroy (gameObject);
	}
	
	void FixedUpdate () {
	}

	void OnCollisionEnter2D()
	{
		Destroy (gameObject);
	}
}
