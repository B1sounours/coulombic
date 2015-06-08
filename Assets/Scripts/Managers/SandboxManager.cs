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
    public static Dictionary<ChargedObject, ChargedObjectSettings> chargedObjects;

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
            MakeChargedObject(cos, false);
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
            ChargedObject newChargedObject = MakeChargedObject(chargedObjectSettings, addToSandboxHistory);
        }
    }

    public static void ClearSandboxHistory()
    {
        sandboxHistory = new List<ChargedObjectSettings>();
        coordinateHistory = new Dictionary<Vector3, ChargedObjectSettings>();
        chargedObjects = new Dictionary<ChargedObject, ChargedObjectSettings>();
    }

    private void AddToHistory(ChargedObject chargedObject, ChargedObjectSettings chargedObjectSettings)
    {
        sandboxHistory.Add(chargedObjectSettings);
        coordinateHistory.Add(chargedObjectSettings.position, chargedObjectSettings);
        GetChargedObjects().Add(chargedObject, chargedObjectSettings);
    }

    public void SendMessageToolChangedCharge(ChargedObject chargedObject)
    {
        //if a charge was changed in sandbox mode before starting the sim, then sandbox history must be updated with the tool use
        if (!GameManager.GetGameManager().HasSimulationBegun())
            UpdateHistory(chargedObject);
    }

    private void UpdateHistory(ChargedObject chargedObject)
    {
        if (GetChargedObjects().ContainsKey(chargedObject))
        {
            //Debug.Log("udpating history " + chargedObject.charge);
            GetChargedObjects()[chargedObject].charge = chargedObject.charge;
        }
        else
        {
            //Debug.Log("UpdateHistory does not contain key");
        }
    }

    private static Dictionary<ChargedObject, ChargedObjectSettings> GetChargedObjects()
    {
        if (chargedObjects == null)
            chargedObjects = new Dictionary<ChargedObject, ChargedObjectSettings>();
        return chargedObjects;
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

    public ChargedObject MakeChargedObject(ChargedObjectSettings chargedObjectSettings, bool addToSandboxHistory = false)
    {
        createCounter++;
        GameObject newObject = Instantiate(SandboxManager.GetSandboxPrefabs()[chargedObjectSettings.shape]);
        newObject.name = "sandbox object " + createCounter;
        newObject.transform.parent = GetRegionManager().gameObject.transform;
        ChargedObject newChargedObject = newObject.AddComponent<ChargedObject>();
        newChargedObject.UpdateValues(chargedObjectSettings);

        if (chargedObjectSettings.canMove)
        {
            MovingChargedObject mco = newObject.AddComponent<MovingChargedObject>();
            mco.UpdateValues(chargedObjectSettings);
            mco.SetFrozenPosition(GameManager.GetGameManager().GetIsPaused());
        }
        newObject.transform.position = chargedObjectSettings.position;

        GetRegionManager().AddChargedObject(newChargedObject);

        if (addToSandboxHistory)
            AddToHistory(newChargedObject, chargedObjectSettings);
        if (!GetChargedObjects().ContainsKey(newChargedObject))
            GetChargedObjects().Add(newChargedObject, chargedObjectSettings);


        return newChargedObject;
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
