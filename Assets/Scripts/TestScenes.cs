using UnityEngine;
using System.Collections;

public class TestScenes : MonoBehaviour {
	void Update () {
		for (int i=0; i<10; i++) {
			if (Input.GetKeyDown((KeyCode)(48+i)))
				Application.LoadLevel("test"+i);
		}
	}
}
