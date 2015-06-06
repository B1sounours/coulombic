using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateStopwatchUI : MonoBehaviour
{
    private Image stopwatchImage;
    public Image[] sliceImages;

    private float sliceTimer = 0;
    private float sliceInterval = 0;
    private int sliceIndex = 0;

    void Start()
    {
        stopwatchImage = GetComponent<Image>();
        sliceInterval = GameManager.GetGameManager().GetVictoryTimerDuration() / sliceImages.Length;
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        bool showStopwatch = !GameManager.GetGameManager().isSandboxMode;
        
        stopwatchImage.enabled = showStopwatch;
        for (int i = 0; i < sliceImages.Length; i++)
            sliceImages[i].enabled = i >= sliceIndex && showStopwatch;

    }

    void Update()
    {
        if (!GameManager.GetGameManager().GetIsPaused())
        {
            sliceTimer += Time.deltaTime;
            if (sliceTimer > sliceInterval)
            {
                sliceIndex++;
                sliceTimer = 0;
            }
            UpdateAppearance();
        }
    }
}
