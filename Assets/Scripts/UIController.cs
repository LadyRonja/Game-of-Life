using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField] TMP_Text toggleSimulationText;
    [Space]
    [SerializeField] Slider emulationSpeedSlider;
    [SerializeField] TMP_Text emulationSpeedText;
    [Space]
    [SerializeField] Slider randomizeAlivePercantageSlider;
    [SerializeField] TMP_Text randomizeAliveText;
    [Space]
    [SerializeField] RectTransform simulationOptionsPanel;
    [SerializeField] Button displaySimulationOptionsButton;
    [Space]
    [SerializeField] RectTransform drawingOptionsPanel;
    [SerializeField] Button drawingOptionsButton;
    [Space]
    [SerializeField] TMP_Dropdown drawStyleOptionsDropDown;
    [SerializeField] TMP_Dropdown shapesOptionsDropDown;

    private void Start()
    {
        if (GameController.Instance.IsRunning) toggleSimulationText.text = "Pause Simulation";
        else toggleSimulationText.text = "Start Simulation";

        emulationSpeedText.text = Math.Round(emulationSpeedSlider.value, 3).ToString();
        randomizeAliveText.text = Math.Round(randomizeAlivePercantageSlider.value).ToString();

        PopulateDrawStyleDropDown();
        PopulateShapesDropDown();
    }

    public void ToggleRunning()
    {
        GameController.Instance.ToggleIsRunning(out bool isRunningNow);
        if(isRunningNow) toggleSimulationText.text = "Pause Simulation";
        else toggleSimulationText.text = "Start Simulation";
    }

    public void NextTick()
    {
        GameController.Instance.NextTick();
    }

    public void KillAll()
    {
        GameController.Instance.KillAll();
        if(GameController.Instance.IsRunning) ToggleRunning();
    }

    public void ChangeTickSpeed()
    {
        GameController.Instance.SetEmulationSpeed(emulationSpeedSlider.value);
        emulationSpeedText.text = Math.Round(emulationSpeedSlider.value, 3).ToString();
    }

    public void RandomizeNewAlive()
    {
        GameController.Instance.RandomizeAlive(randomizeAlivePercantageSlider.value);
    }

    public void OnRandomizeAliveSliderChange()
    {
        randomizeAliveText.text = Math.Round(randomizeAlivePercantageSlider.value).ToString();
    }

    public void ToggleSimulationOptionPanel()
    {
        displaySimulationOptionsButton.transform.localScale *= -1;
        Vector3 targetPos = simulationOptionsPanel.transform.position;
        targetPos.y *= -1f;
        simulationOptionsPanel.transform.position = targetPos;
    }

    public void ToggleDrawingOptionsPanel()
    {
        drawingOptionsButton.transform.localScale *= -1;
        Vector3 targetPos = drawingOptionsPanel.transform.position;
        targetPos.x *= -1f;
        drawingOptionsPanel.transform.position = targetPos;
    }

    public void SetDrawStyle()
    {
        int modeIndex = drawStyleOptionsDropDown.value;
        GridClicker.DrawMode modeToBecome = (GridClicker.DrawMode)Enum.ToObject(typeof(GridClicker.DrawMode), modeIndex);

        GridClicker.Instance.currentDrawMode = modeToBecome;
    }

    public void SetShapeToDraw()
    {
        int modeIndex = shapesOptionsDropDown.value;
        ShapeStorage.Shapes shapeToDraw = (ShapeStorage.Shapes)Enum.ToObject(typeof(ShapeStorage.Shapes), modeIndex);

        GridClicker.Instance.shapeToDraw = shapeToDraw;
    }

    public void PopulateDrawStyleDropDown()
    {
        List<TMP_Dropdown.OptionData> drawStyleOptions = new();
        string[] drawStyles = Enum.GetNames(typeof(GridClicker.DrawMode));

        for (int i = 0; i < drawStyles.Length; i++) 
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = drawStyles[i];
            drawStyleOptions.Add(option);
        }

        drawStyleOptionsDropDown.options = drawStyleOptions;
    }


    public void PopulateShapesDropDown()
    {
        List<TMP_Dropdown.OptionData> shapeOptions = new();
        string[] shapes = Enum.GetNames(typeof(ShapeStorage.Shapes));

        for (int i = 0; i < shapes.Length; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = shapes[i];
            shapeOptions.Add(option);
        }

        shapesOptionsDropDown.options = shapeOptions;
    }

}
