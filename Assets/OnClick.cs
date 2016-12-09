using UnityEngine;
using System.Collections;

public class OnClick : MonoBehaviour {
	//private bool swapped = false;

	// Use this for initialization
	void OnMouseDown(){
			print ("click!");
		if (Grid.swapQue [0] == null) {
			Grid.swapQue [0] = this.gameObject;
		} else if (Grid.swapQue [1] == null) {
			Grid.swapQue [1] = this.gameObject;
		}
	}
}
