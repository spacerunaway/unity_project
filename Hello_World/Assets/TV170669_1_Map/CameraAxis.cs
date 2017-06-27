using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAxis : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public float fRotateSpeed = 10.0f; 

	void Update() 
	{ 
		bool isPush = Input.GetMouseButton( 0 ); 

		if( isPush ) 
		{ 
			// 移動量 
			float fValue = fRotateSpeed * Time.deltaTime; 

			// 回転 
			transform.Rotate( 0, fValue, 0, Space.World ); 
		} 
	} 
}
