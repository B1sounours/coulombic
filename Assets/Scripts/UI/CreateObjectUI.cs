using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class ChargedObjectSettings
{
    //when the system issues a command to create a charged object, either through the sandbox or a sandbox scene reset, this contains all info required to create one object
    public bool showCharge, integerCoords, canMove;
    public float mass, charge;
    public Vector3 startVelocity;
    public SandboxShapes shape;

    public ChargedObjectSettings(bool showCharge, bool integerCoords, bool canMove, float mass, float charge, Vector3 startVelocity,SandboxShapes shape)
    {
        this.showCharge = showCharge;
        this.integerCoords = integerCoords;
        this.canMove = canMove;

        this.mass = mass;
        this.charge = charge;
        this.startVelocity = startVelocity;

        this.shape = shape;
    }
}

public class CreateObjectUI : MonoBehaviour
{
    public Toggle showChargeToggle, integerCoordsToggle, canMoveToggle;
    public Slider massSlider, chargeSlider, xVelocitySlider, yVelocitySlider, zVelocitySlider;

    public Text chargeText, massText, velocityText;
    private SandboxShapes sandboxShape = SandboxShapes.sphere;
    private float sliderExponent;

    private bool showCursor = false;
    private GameObject cursorGameObject;
    private bool shouldRemakeCursor = false;

    void Start()
    {
        UpdateSliderMaximums();
        UpdateCreateObjectUI();
        StartCoroutine(RemakeCursorCycle());
    }

    void Update()
    {
        if (showCursor)
            GetCursorGameObject().transform.position = GetCursorPosition();
    }

    private IEnumerator RemakeCursorCycle()
    {
        //too expensive to remake the gameobject every every time slider values change since sliders are smooth
        //this updates on a timer instead

        while (true)
        {
            if (shouldRemakeCursor)
                RemakeCursorGameObject();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public Vector3 GetCursorPosition()
    {
        Vector3 origin = Camera.main.gameObject.transform.position;
        float range = 5;
        Vector3 position = origin + Camera.main.transform.forward * range;
        if (GetChargedObjectSettingsFromUI().integerCoords)
            position = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));
        return position;
    }

    private GameObject GetCursorGameObject()
    {
        if (cursorGameObject == null)
            RemakeCursorGameObject();
        return cursorGameObject;
    }

    public void SetShowCursor(bool showCursor)
    {
        this.showCursor = showCursor;
        if (!showCursor && cursorGameObject != null)
            Destroy(cursorGameObject);
    }

    private void RemakeCursorGameObject()
    {
        shouldRemakeCursor = false;

        Destroy(cursorGameObject);
        ChargedObjectSettings chargedObjectSettings = GetChargedObjectSettingsFromUI();

        cursorGameObject = Instantiate(SandboxManager.GetSandboxPrefabs() [sandboxShape]);
        ChargedObject co = cursorGameObject.AddComponent<ChargedObject>();
        MovingChargedObject mco = cursorGameObject.AddComponent<MovingChargedObject>();
        co.enabled = false;
        mco.enabled = false;
        co.UpdateValues(chargedObjectSettings);
        mco.UpdateValues(chargedObjectSettings);
        cursorGameObject.transform.position = new Vector3(0, -100000, 0);

        foreach (GameObject child in ParentChildFunctions.GetAllChildren(cursorGameObject, true))
            if (child.GetComponent<Collider>() != null)
                child.GetComponent<Collider>().enabled = false;
    }

    private void UpdateSliderMaximums()
    {
        sliderExponent = 0.5f;

        chargeSlider.maxValue = Mathf.Pow(GameSettings.maximumCharge, sliderExponent);
        chargeSlider.minValue = -Mathf.Pow(GameSettings.maximumCharge, sliderExponent);

        massSlider.maxValue = Mathf.Pow(GameSettings.maximumMass, sliderExponent);
        massSlider.minValue = 1;

        xVelocitySlider.maxValue = Mathf.Pow(GameSettings.maximumVelocity, sliderExponent);
        xVelocitySlider.minValue = -Mathf.Pow(GameSettings.maximumVelocity, sliderExponent);

        yVelocitySlider.maxValue = Mathf.Pow(GameSettings.maximumVelocity, sliderExponent);
        yVelocitySlider.minValue = -Mathf.Pow(GameSettings.maximumVelocity, sliderExponent);

        zVelocitySlider.maxValue = Mathf.Pow(GameSettings.maximumVelocity, sliderExponent);
        zVelocitySlider.minValue = -Mathf.Pow(GameSettings.maximumVelocity, sliderExponent);
    }

    public void UpdateCreateObjectUI()
    {
        chargeText.text = "Charge: " + GetSliderValueString(chargeSlider, 0, true);
        massText.text = "Mass: " + GetSliderValueString(massSlider, 1, false);
        velocityText.text = "(" + GetSliderValueString(xVelocitySlider, 1) + ", " + GetSliderValueString(yVelocitySlider, 1) + ", " + GetSliderValueString(zVelocitySlider, 1) + ")";

        shouldRemakeCursor = true;
    }

    private string GetSliderValueString(Slider slider, int sigFigs)
    {
        return GetSliderValueString(slider, sigFigs, false);
    }

    private string GetSliderValueString(Slider slider, int sigFigs, bool showSign)
    {
        string sign = "";
        if (showSign && slider.value > 0)
            sign = "+";

        string formatString = "0";
        if (sigFigs > 0)
        {
            formatString += ".";
            for (; sigFigs > 0; sigFigs--)
                formatString += "0";
        }
        return sign + GetAdjustedSliderValue(slider).ToString(formatString);
    }

    private float GetAdjustedSliderValue(Slider slider)
    {
        if (slider.value == 0)
            return 0;
        float value = Mathf.Pow(Mathf.Abs(slider.value), 1 / sliderExponent);
        float signedValue = value * Mathf.Abs(slider.value) / slider.value;
        return signedValue;
    }

    public ChargedObjectSettings GetChargedObjectSettingsFromUI()
    {
        Vector3 startVelocity = new Vector3(GetAdjustedSliderValue(xVelocitySlider), GetAdjustedSliderValue(yVelocitySlider), GetAdjustedSliderValue(zVelocitySlider));
        return new ChargedObjectSettings(showChargeToggle.isOn, integerCoordsToggle.isOn, canMoveToggle.isOn, GetAdjustedSliderValue(massSlider), GetAdjustedSliderValue(chargeSlider), startVelocity,sandboxShape);
    }

    public void SelectShape(int shapeCode)
    {
        sandboxShape = shapeCode == 0 ? SandboxShapes.sphere : SandboxShapes.cube;
        UpdateCreateObjectUI();
    }
}
