using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SandboxShapes
{
    sphere, cube
}

public class SandboxManager : MonoBehaviour
{

    public static List<ChargedObjectSettings> sandboxHistory;
    private static Dictionary<SandboxShapes, GameObject> sandboxPrefabs;
    private CreateObjectUI createObjectUI;
    private RegionManager regionManager;

    void Start()
    {
        if (sandboxHistory == null)
            sandboxHistory = new List<ChargedObjectSettings>();
        if (sandboxHistory.Count > 0)
            RemakeAllFromSandboxHistory();
    }

    private void RemakeAllFromSandboxHistory()
    {
        foreach (ChargedObjectSettings cos in sandboxHistory)
            MakeChargedObject(cos);
    }

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

    public void CreateClick(bool addToSandboxHistory)
    {
        SoundManager.PlaySound(GameSounds.click);

        ChargedObjectSettings chargedObjectSettings = GetCreateObjectUI().GetChargedObjectSettingsFromUI();
        MakeChargedObject(chargedObjectSettings);
        if (addToSandboxHistory)
            sandboxHistory.Add(chargedObjectSettings);
    }

    public GameObject MakeChargedObject(ChargedObjectSettings chargedObjectSettings)
    {
        GameObject newObject = Instantiate(SandboxManager.GetSandboxPrefabs()[chargedObjectSettings.shape]);
        newObject.transform.parent = GetRegionManager().gameObject.transform;
        ChargedObject co = newObject.AddComponent<ChargedObject>();
        co.UpdateValues(chargedObjectSettings);

        if (chargedObjectSettings.canMove)
        {
            MovingChargedObject mco = newObject.AddComponent<MovingChargedObject>();
            mco.UpdateValues(chargedObjectSettings);
        }
        newObject.transform.position = chargedObjectSettings.position ;

        GetRegionManager().AddChargedObject(co);
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
