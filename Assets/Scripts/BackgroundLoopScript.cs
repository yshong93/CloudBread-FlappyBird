using UnityEngine;
using System.Collections;

public class BackgroundLoopScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localPosition.x < -2.58f)
		{
			transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
		}
		transform.Translate(-Time.deltaTime, 0, 0);
	}
}
