using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum SandboxShapes
{
    sphere, cube
}

public class ChargedObjectSettings
{
    //when the system issues a command to create a charged object, either through the sandbox or a sandbox scene reset, this contains all info required to create one object
    public bool showCharge, integerCoords, canMove;
    public float mass, charge;
    public Vector3 startVelocity;

    public ChargedObjectSettings(bool showCharge, bool integerCoords, bool canMove, float mass, float charge, Vector3 startVelocity)
    {
        this.showCharge = showCharge;
        this.integerCoords = integerCoords;
        this.canMove = canMove;

        this.mass = mass;
        this.charge = charge;
        this.startVelocity = startVelocity;
    }
}

public class CreateObjectUI : MonoBehaviour
{
    public Toggle showChargeToggle, integerCoordsToggle, canMoveToggle;
    public Slider massSlider, chargeSlider, xVelocitySlider, yVelocitySlider, zVelocitySlider;

    public Text chargeText, massText, velocityText;
    private SandboxShapes sandboxShape = SandboxShapes.sphere;
    private float sliderExponent;

    void Start()
    {
        UpdateSliderMaximums();
        UpdateCreateObjectUI();
    }

    private void UpdateSliderMaximums()
    {
        sliderExponent = 0.5f;

        chargeSlider.maxValue = Mathf.Pow(GameSettings.maximumCharge, sliderExponent);
        chargeSlider.minValue = -Mathf.Pow(GameSettings.maximumCharge, sliderExponent);

        massSlider.maxValue = Mathf.Pow(GameSettings.maximumMass, sliderExponent);
        massSlider.minValue = -Mathf.Pow(GameSettings.maximumMass, sliderExponent);

        xVelocitySlider.maxValue = Mathf.Pow(GameSettings.maximumVelocity, sliderExponent);
        xVelocitySlider.minValue = -Mathf.Pow(GameSettings.maximumVelocity, sliderExponent);

        yVelocitySlider.maxValue = Mathf.Pow(GameSettings.maximumVelocity, sliderExponent);
        yVelocitySlider.minValue = -Mathf.Pow(GameSettings.maximumVelocity, sliderExponent);

        zVelocitySlider.maxValue = Mathf.Pow(GameSettings.maximumVelocity, sliderExponent);
        zVelocitySlider.minValue = -Mathf.Pow(GameSettings.maximumVelocity, sliderExponent);
    }

    public void UpdateCreateObjectUI()
    {
        chargeText.text = "Charge: " + GetSliderValueString(chargeSlider,0,true);
        massText.text = "Mass: " + GetSliderValueString(massSlider, 1, false);
        velocityText.text = "(" + GetSliderValueString(xVelocitySlider, 1) + ", " + GetSliderValueString(yVelocitySlider, 1) + ", " + GetSliderValueString(zVelocitySlider, 1) + ")";
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
        return sign+GetAdjustedSliderValue(slider).ToString(formatString);
    }

    private float GetAdjustedSliderValue(Slider slider)
    {
        if (slider.value == 0)
            return 0;
        float value= Mathf.Pow(Mathf.Abs( slider.value), 1 / sliderExponent);
        float signedValue = value * Mathf.Abs(slider.value) / slider.value;
        return signedValue;
    }

    public ChargedObjectSettings GetChargedObjectSettingsFromUI()
    {
        Vector3 startVelocity = new Vector3(GetAdjustedSliderValue(xVelocitySlider), GetAdjustedSliderValue(yVelocitySlider), GetAdjustedSliderValue(zVelocitySlider));
        return new ChargedObjectSettings(showChargeToggle.isOn, integerCoordsToggle.isOn, canMoveToggle.isOn, GetAdjustedSliderValue(massSlider), GetAdjustedSliderValue(chargeSlider), startVelocity);
    }

    public void SelectShape(int shapeCode)
    {
        sandboxShape = shapeCode == 0 ? SandboxShapes.sphere : SandboxShapes.cube;
    }
}
