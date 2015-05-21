using UnityEngine;
using System.Collections;

public enum Tools
{
    add = 0, subtract = 1, divide = 2, multiply = 3
}

public class ClickTool : MonoBehaviour
{
    private float range = 50;
    private GameplayUI gameplayUI;
    public int[] toolCharges;
    public bool infiniteCharges = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GetGameplayUI().GetGameMenuMode() == GameMenuModes.gameplay)
        {
            Vector3 pos = transform.position;
            Vector3 forward = Camera.main.transform.forward;
            RaycastHit rayCastHit = new RaycastHit();
            bool isHit = Physics.Linecast(pos, pos + forward * range, out rayCastHit);
            if (isHit)
                Click(rayCastHit.collider.gameObject);
        }
    }


    private GameplayUI GetGameplayUI()
    {
        if (gameplayUI == null)
            gameplayUI = FindObjectOfType<GameplayUI>();
        return gameplayUI;
    }

    private void Click(GameObject clickedObject)
    {
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
            }
            GetGameplayUI().UpdateSelectedToolAppearance();
        }
    }
}
