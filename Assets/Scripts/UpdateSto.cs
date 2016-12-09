using UnityEngine;
using System;
using System.Collections;

public class UpdateSto : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}

	void OnTriggerEnter(Collider other){
		int ArrayLocationY = (int)(this.gameObject.transform.position.y / Grid.StaticSpacing);
		print(Grid.Sto.GetLength(2));
//		for (int z = 0; z < Grid.Sto.GetLength (2); z++) {
//
//		}
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}

