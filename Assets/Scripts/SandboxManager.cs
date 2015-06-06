using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SandboxShapes
{
    sphere, cube
}

public class SandboxManager : MonoBehaviour
{

    public List<ChargedObjectSettings> sandboxObjects;
    private static Dictionary<SandboxShapes, GameObject> sandboxPrefabs;
    private CreateObjectUI createObjectUI;
    private RegionManager regionManager;

    public static Dictionary<SandboxShapes, GameObject> GetSandboxPrefabs()
    {
        if (sandboxPrefabs == null)
        {
            sandboxPrefabs = new Dictionary<SandboxShapes, GameObject>();
            sandboxPrefabs.Add(SandboxShapes.cube, Resources.Load<GameObject>("prefabs/cube"));
            sandboxPrefabs.Add(SandboxShapes.sphere, Resources.Load<GameObject>("prefabs/sphere"));
        }
        return sandboxPrefabs;
    }

    private RegionManager GetRegionManager()
    {
        if (regionManager == null)
            regionManager = FindObjectOfType<RegionManager>();
        return regionManager;
    }

    private CreateObjectUI GetCreateObjectUI()
    {
        if (createObjectUI == null)
            createObjectUI = FindObjectOfType<CreateObjectUI>();
        return createObjectUI;
    }

    public void CreateClick()
    {
        if (sandboxObjects == null)
            sandboxObjects = new List<ChargedObjectSettings>();

        SoundManager.PlaySound(GameSounds.click);
        GameObject newObject= MakeChargedObject();
        GetRegionManager().AddChargedObject(newObject.GetComponent<ChargedObject>());
    }

    public GameObject MakeChargedObject()
    {
        ChargedObjectSettings cos = GetCreateObjectUI().GetChargedObjectSettingsFromUI();

        GameObject newObject = Instantiate(SandboxManager.GetSandboxPrefabs()[cos.shape]);
        ChargedObject co = newObject.AddComponent<ChargedObject>();
        co.UpdateValues(cos);

        if (cos.canMove)
        {
            MovingChargedObject mco = newObject.AddComponent<MovingChargedObject>();
            mco.UpdateValues(cos);
        }
        newObject.transform.position = GetCreateObjectUI().GetCursorPosition();
        return newObject;
    }

    public void DeleteClick(GameObject clickObject)
    {
        ChargedObject co = clickObject.GetComponent<ChargedObject>();
        if (co != null)
        {
            GetRegionManager().DeleteChargedObject(co);
            SoundManager.PlaySound(GameSounds.click);
        }
    }
}
