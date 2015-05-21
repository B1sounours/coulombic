using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateStopwatchUI : MonoBehaviour
{
    private GameManager gameManager;
    private Image stopwatchImage;
    public Image[] sliceImages;

    private float sliceTimer = 0;
    private float sliceInterval = 0;
    private int sliceIndex = 0;

    void Start()
    {
        stopwatchImage = GetComponent<Image>();
        sliceInterval = GetGameManager().GetVictoryTimerDuration() / sliceImages.Length;
    }

    private GameManager GetGameManager()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        return gameManager;
    }

    private void UpdateAppearance()
    {
        stopwatchImage.enabled = true;
        for (int i = 0; i < sliceImages.Length; i++)
            sliceImages[i].enabled = i > sliceIndex;
    }

    void Update()
    {
        if (!GetGameManager().GetIsPaused())
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
