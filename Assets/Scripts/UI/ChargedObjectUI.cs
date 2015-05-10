using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChargedObjectUI : MonoBehaviour {
    public GameObject lockContainer;
    public Image fuzzyCircle;
    public Text chargeLabel;

	void Update () {
        transform.LookAt(Camera.main.transform.position);
	}

    public void UpdateAppearance(float charge, bool showLock)
    {
        GetComponent<Canvas>().enabled = true;
        MainMenu.SetUIVisibility(lockContainer, showLock);
        fuzzyCircle.enabled = !showLock;
    
        string text = charge.ToString();
        if (charge > 0)
            text = "+" + text;
        chargeLabel.text = text;

        fuzzyCircle.color = charge > 0 ? Color.cyan : new Color(1f, 0.5f, 0.5f);
    }
}
