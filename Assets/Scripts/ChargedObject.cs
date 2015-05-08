using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChargedObject : MonoBehaviour
{
    public float charge = 1;
    public float reverseChargeTimer = 0;
    private float reverseChargeTimeCount = 0;

    private GameObject canvasGameObject;
    private static GameObject canvasPrefab;

    private Text canvasText;
    private Image canvasImage;

    void Update()
    {
        if (reverseChargeTimer > 0)
        {
            reverseChargeTimeCount += Time.deltaTime;
            if (reverseChargeTimeCount > reverseChargeTimer)
            {
                reverseChargeTimeCount = 0;
                charge *= -1;
                UpdateAppearance();
            }
        }
    }

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

        string label = charge.ToString();
        if (charge > 0)
            label = "+" + label;
        GetCanvasText().text = label;

        GetCanvasImage().color = charge > 0 ? Color.cyan : new Color(1f, 0.5f, 0.5f);
    }

    private Text GetCanvasText()
    {
        if (canvasText == null)
        {
            foreach (GameObject go in ParentChildFunctions.GetAllChildren(gameObject, false))
            {
                Text text = go.GetComponent<Text>();
                if (text != null)
                    canvasText = text;
            }
        }
        return canvasText;
    }

    private Image GetCanvasImage()
    {
        if (canvasImage == null)
        {
            foreach (GameObject go in ParentChildFunctions.GetAllChildren(gameObject, false))
            {
                Image image = go.GetComponent<Image>();
                if (image != null)
                    canvasImage = image;
            }
        }
        return canvasImage;
    }

    private GameObject GetCanvasPrefab()
    {
        if (canvasPrefab == null)
            canvasPrefab = Resources.Load("prefabs/charge canvas") as GameObject;
        return canvasPrefab;
    }
}
