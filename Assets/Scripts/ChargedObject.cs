using UnityEngine;
using System.Collections;

public class ChargedObject : MonoBehaviour {
    public float charge = 1;

    public void UpdateAppearance()
    {
        GetComponent<Renderer>().material.color = charge > 0 ? Color.red : Color.green;
    }
}
