using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PuzzleTitleUpdate : MonoBehaviour {
	void Start () {
        int index=0;
        foreach (GameObject child in ParentChildFunctions.GetAllChildren(gameObject, false))
        {
            if (child.GetComponent<Text>() != null)
            {
                child.GetComponent<Text>().text = LevelManager.GetLevelTitle(index);
                index++;
            }
        }

	}
}
