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
    public static Dictionary<Vector3, ChargedObjectSettings> coordinateHistory;
    private static Dictionary<SandboxShapes, GameObject> sandboxPrefabs;
    private CreateObjectUI createObjectUI;
    private RegionManager regionManager;
    private int createCounter = 0;

    void Start()
    {
        if (sandboxHistory == null)
            sandboxHistory = new List<ChargedObjectSettings>();
        if (coordinateHistory == null)
            coordinateHistory = new Dictionary<Vector3, ChargedObjectSettings>();
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

        if (OverlapsWithCoordHistory(chargedObjectSettings) && GameManager.GetGameManager().GetIsPaused())
        {
            SoundManager.PlaySound(GameSounds.refuse);
        }
        else
        {
            MakeChargedObject(chargedObjectSettings);
            if (addToSandboxHistory)
                AddToHistory(chargedObjectSettings);
        }
    }

    private void AddToHistory(ChargedObjectSettings cos)
    {
        sandboxHistory.Add(cos);
        coordinateHistory.Add(cos.position, cos);
    }

    private void RemoveFromHistory(ChargedObjectSettings cos)
    {
        sandboxHistory.Remove(cos);
        coordinateHistory.Remove(cos.position);
    }

    private bool OverlapsWithCoordHistory(ChargedObjectSettings cos)
    {
        return coordinateHistory.ContainsKey(cos.position);
    }

    public GameObject MakeChargedObject(ChargedObjectSettings chargedObjectSettings)
    {
        createCounter++;
        GameObject newObject = Instantiate(SandboxManager.GetSandboxPrefabs()[chargedObjectSettings.shape]);
        newObject.name = "sandbox object " + createCounter;
        newObject.transform.parent = GetRegionManager().gameObject.transform;
        ChargedObject co = newObject.AddComponent<ChargedObject>();
        co.UpdateValues(chargedObjectSettings);

        if (chargedObjectSettings.canMove)
        {
            MovingChargedObject mco = newObject.AddComponent<MovingChargedObject>();
            mco.UpdateValues(chargedObjectSettings);
            mco.SetFrozenPosition(GameManager.GetGameManager().GetIsPaused());
        }
        newObject.transform.position = chargedObjectSettings.position;

        GetRegionManager().AddChargedObject(co);
        return newObject;
    }

    public void DeleteClick(GameObject clickObject)
    {
        ChargedObject co = clickObject.GetComponent<ChargedObject>();
        if (co != null)
        {
            ChargedObjectSettings cos = co.GetChargedObjectSettings();
            if (cos != null && sandboxHistory.Contains(cos))
                RemoveFromHistory(cos);

            GetRegionManager().DestroyChargedObject(co);
            SoundManager.PlaySound(GameSounds.click);
        }
    }
}
