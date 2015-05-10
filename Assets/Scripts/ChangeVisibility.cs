using UnityEngine;
using System.Collections;

public class ChangeVisibility : MonoBehaviour
{
    public bool isVisible = true;
    void Start()
    {
        SetRenderVisibility(isVisible);
    }

    private void SetRenderVisibility(bool isVisibile)
    {
        foreach (GameObject child in ParentChildFunctions.GetAllChildren(gameObject, true))
        {
            if (child.GetComponent<Renderer>() != null)
                child.GetComponent<Renderer>().enabled = isVisibile;
        }
    }

}
