using UnityEngine;
using System.Collections;

public class FaceMainCamera : MonoBehaviour {
	void Update () {
        transform.LookAt(Camera.main.transform.position);
	}
}
