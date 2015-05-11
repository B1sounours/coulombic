using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToolSelectUI : MonoBehaviour
{
    public GameObject[] toolContainers;
    private Text[] counterTexts;
    public void UpdateAppearance()
    {
        ClickTool clickTool = FindObjectOfType<ClickTool>();
        for (int i = 0; i < toolContainers.Length; i++)
        {
            int charges = clickTool.toolCharges[i];
            GetCountTexts()[i].text = GetCountText(charges,clickTool);
            if (charges < 1 && !clickTool.infiniteCharges)
                MainMenu.SetUIVisibility(toolContainers[i], false);
        }
    }

    public static string GetCountText(int count,ClickTool clickTool)
    {
		if (clickTool.infiniteCharges)
			return "";
        return count > 0 ? count + "\nleft" : "0";
    }

    private Text[] GetCountTexts()
    {
        if (counterTexts == null)
        {
            counterTexts = new Text[toolContainers.Length];
            for (int i = 0; i < toolContainers.Length; i++)
            {
                foreach (GameObject child in ParentChildFunctions.GetAllChildren(toolContainers[i]))
                    if (child.name.Equals("count"))
                    {
                        counterTexts[i] = child.GetComponent<Text>();
                        break;
                    }
            }
        }
        return counterTexts;
    }
}
