using UnityEngine;
using System.Collections;

public class RotateForever : MonoBehaviour {

	public float x,y,z;
	public float speed;

	void Update()
	{
		transform.Rotate(new Vector3(x,y,z) * speed * Time.deltaTime);
	}
}
