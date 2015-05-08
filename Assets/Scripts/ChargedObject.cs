using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChargedObject : MonoBehaviour
{
    public float charge = 1;
    private GameObject canvasGameObject;
    private static GameObject canvasPrefab;

    public void UpdateAppearance()
    {
        GetComponent<Renderer>().material.color = charge > 0 ? Color.red : Color.green;
        if (canvasGameObject == null)
        {
            canvasGameObject = Instantiate(GetCanvasPrefab(), transform.position, transform.rotation) as GameObject;
            canvasGameObject.transform.SetParent(transform);
        }
        UpdateCanvasAppearance();
    }

    private void UpdateCanvasAppearance()
    {
        if (charge == 0)
        {
            canvasGameObject.GetComponent<Canvas>().enabled = false;
            return;
        }
        canvasGameObject.GetComponent<Canvas>().enabled = true;

        foreach (GameObject go in ParentChildFunctions.GetAllChildren(gameObject, false))
        {
            Text text = go.GetComponent<Text>();
            Image image = go.GetComponent<Image>();
            if (text != null)
            {
                string label=charge.ToString();
                if (charge > 0)
                    label = "+" + label;
                text.text = label;
            }
            if (image != null)
                image.color = charge > 0 ? Color.cyan : new Color(1f, 0.5f, 0.5f);
        }
    }

    private GameObject GetCanvasPrefab()
    {
        if (canvasPrefab == null)
            canvasPrefab = Resources.Load("prefabs/charge canvas") as GameObject;
        return canvasPrefab;
    }
}
