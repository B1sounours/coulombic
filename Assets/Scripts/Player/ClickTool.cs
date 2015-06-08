using UnityEngine;
using System.Collections;

public enum Tools
{
    add = 0, subtract = 1, divide = 2, multiply = 3, delete = 4, create = 5
}

public class ClickTool : MonoBehaviour
{
    private float range = 50;
    private GameplayUI gameplayUI;
    public int[] toolCharges;
    public bool infiniteCharges = false;
    private SandboxManager sandboxManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GetGameplayUI().GetGameMenuMode() == GameMenuModes.gameplay)
        {
            Tools tool = GetGameplayUI().GetSelectedTool();
            GameObject clickedObject = GetClickedObject();
            if (tool == Tools.create)
            {
                GetSandboxManager().CreateClick(!GameManager.GetGameManager().HasSimulationBegun());
            }
            else if (clickedObject != null)
            {
                if (tool == Tools.delete)
                    GetSandboxManager().DeleteClick(GetClickedObject());
                else
                {
                    ModifyChargeClick(GetClickedObject());
                }
            }
        }
    }

    private GameObject GetClickedObject()
    {
        Vector3 pos = transform.position;
        Vector3 forward = Camera.main.transform.forward;
        RaycastHit rayCastHit = new RaycastHit();
        bool isHit = Physics.Linecast(pos, pos + forward * range, out rayCastHit);
        if (!isHit)
            return null;
        return rayCastHit.collider.gameObject;
    }

    private SandboxManager GetSandboxManager()
    {
        if (sandboxManager == null)
            sandboxManager = FindObjectOfType<SandboxManager>();
        return sandboxManager;
    }

    public bool HasAtLeastOneToolCharge()
    {
        for (int i = 0; i < toolCharges.Length; i++)
            if (toolCharges[i] > 0)
                return true;
        return false;
    }

    private GameplayUI GetGameplayUI()
    {
        if (gameplayUI == null)
            gameplayUI = FindObjectOfType<GameplayUI>();
        return gameplayUI;
    }

    public bool CanSelectTool(Tools tool)
    {
        if (!GameManager.GetGameManager().isSandboxMode && (tool == Tools.create || tool == Tools.delete))
            return false;
        return infiniteCharges || toolCharges[(int)tool] > 0;
    }

    private void ModifyChargeClick(GameObject clickedObject)
    {
        if (clickedObject == null)
            return;
        ChargedObject co = clickedObject.GetComponent<ChargedObject>();
        Tools tool = GetGameplayUI().GetSelectedTool();
        if (co != null)
        {
            if (co.isLocked)
            {
                Debug.Log("prompt: cannot change locked charged object");
            }
            else if (!infiniteCharges && toolCharges[(int)tool] < 1)
            {
                Debug.Log("prompt: tool is depleted");
                SoundManager.PlaySound(GameSounds.depleted);
            }
            else
            {
                SoundManager.PlaySound(GameSounds.click);
                toolCharges[(int)tool]--;

                if (tool == Tools.add)
                    co.charge += 1;
                if (tool == Tools.subtract)
                    co.charge -= 1;
                if (tool == Tools.divide)
                    co.charge /= 2;
                if (tool == Tools.multiply)
                    co.charge *= 2;

                co.charge = Mathf.Clamp(co.charge, -GameSettings.maximumCharge, GameSettings.maximumCharge);

                co.UpdateAppearance();
                if (GameManager.GetGameManager().isSandboxMode)
                    GetSandboxManager().SendMessageToolChangedCharge(co);
            }
            GetGameplayUI().UpdateSelectedToolAppearance();
        }
    }
}
