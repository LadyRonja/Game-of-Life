using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("Simulation Settings")]
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
    [SerializeField] Button killAllButton;
    [SerializeField] Button reRandomizeButton;

    [Header("Drawing Options")]
    [SerializeField] RectTransform drawingOptionsPanel;
    [SerializeField] Button drawingOptionsButton;
    [Space]
    [SerializeField] TMP_Dropdown drawStyleOptionsDropDown;
    [SerializeField] TMP_Dropdown shapesOptionsDropDown;
    [SerializeField] TMP_Dropdown rotationOptionsDropDown;
    [Space]
    [SerializeField] Toggle flipXToggle;
    [SerializeField] Toggle flipYToggle;

    [Header("PvP Settings & Info")]
    [SerializeField] RectTransform jumbotron;
    [SerializeField] TMP_Text playerOneInfoText;
    [SerializeField] TMP_Text playerTwoInfoText;
    [SerializeField] TMP_Text curIttInfoText;
    [Space]
    [SerializeField] Button pvpStartButton;
    [SerializeField] TMP_Text pvpStartButtonText;
    [SerializeField] RectTransform pvpOptionsPanel;
    [SerializeField] Button pvpOptionsButton;
    [Space]
    [SerializeField] Slider maxIttOptions;
    [SerializeField] TMP_Text ittCounter;
    string ittCounterPrereqText = "Max Itterations: ";
    [SerializeField] Slider maxUnitsOptions;
    [SerializeField] TMP_Text maxUitsCounter;
    string maxUnitsCounterPrereqText = "Units Each: ";
    [SerializeField] RectTransform victoryScreen;
    [SerializeField] TMP_Text winnerText;

    private void Awake()
    {
        #region Singleton
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        #endregion
    }

    private void Start()
    {
        if (GameController.Instance.IsRunning) toggleSimulationText.text = "Pause Simulation";
        else toggleSimulationText.text = "Start Simulation";

        emulationSpeedText.text = Math.Round(emulationSpeedSlider.value, 3).ToString();
        randomizeAliveText.text = Math.Round(randomizeAlivePercantageSlider.value).ToString();

        jumbotron.gameObject.SetActive(false);
        ChangeMaxPvPItterations();
        ChangeMaxUnits();
        pvpStartButton.onClick.AddListener(StartWithSettings);

        PopulateDrawStyleDropDown();
        PopulateShapesDropDown();
        PopulateRotationDropDown();
    }

    #region Simulation Settings
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
    #endregion

    #region Drawing Settings
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
        ShapeFactory.Shapes shapeToDraw = (ShapeFactory.Shapes)Enum.ToObject(typeof(ShapeFactory.Shapes), modeIndex);

        GridClicker.Instance.shapeToDraw = shapeToDraw;
    }

    public void SetShapeRotation()
    {
        int modeIndex = rotationOptionsDropDown.value;
        ShapeFactory.Rotation rotationOfShape = (ShapeFactory.Rotation)Enum.ToObject(typeof(ShapeFactory.Rotation), modeIndex);

        GridClicker.Instance.shapeRotation = rotationOfShape;
    }

    public void SetFlipAxis()
    {
        GridClicker.Instance.flipShapeOnAxisX = flipXToggle.isOn;
        GridClicker.Instance.flipShapeOnAxisY = flipYToggle.isOn;
    }

    private void PopulateDrawStyleDropDown()
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

    private void PopulateShapesDropDown()
    {
        List<TMP_Dropdown.OptionData> shapeOptions = new();
        string[] shapes = Enum.GetNames(typeof(ShapeFactory.Shapes));

        for (int i = 0; i < shapes.Length; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = shapes[i];
            shapeOptions.Add(option);
        }

        shapesOptionsDropDown.options = shapeOptions;
    }

    private void PopulateRotationDropDown()
    {
        List<TMP_Dropdown.OptionData> rotationOptions = new();
        string[] rotations = Enum.GetNames(typeof(ShapeFactory.Rotation));

        for (int i = 0; i < rotations.Length; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = rotations[i];
            rotationOptions.Add(option);
        }

        rotationOptionsDropDown.options = rotationOptions;
    }
    #endregion

    #region PvP Settings
    public void TogglePvPOptionsPanel()
    {
        pvpOptionsButton.transform.localScale *= -1;
        Vector3 targetPos = pvpOptionsPanel.transform.position;
        targetPos.x *= -1f;
        pvpOptionsPanel.transform.position = targetPos;
    }

    public void ChangeMaxPvPItterations()
    {
        GameController.Instance.maxItterations = (uint)maxIttOptions.value;
        ittCounter.text = ittCounterPrereqText + maxIttOptions.value;
    }

    public void ChangeMaxUnits()
    {
        GridClicker.Instance.maxUnitsToPlace = (int)maxUnitsOptions.value;
        maxUitsCounter.text = maxUnitsCounterPrereqText + maxUnitsOptions.value;
    }

    public void StartWithSettings()
    {
        KillAll();
        jumbotron.gameObject.SetActive(true);
        UpdateJumbotron();

        ToggleSimulationOptionPanel();
        displaySimulationOptionsButton.enabled = false;

        GridClicker.Instance.teamToDraw = PvPController.Teams.P1;
        GridClicker.Instance.allowDrawOnRightSide = false;
        GameController.Instance.playingPvP = true;

        GridTile[,] grid = GameController.Instance.Container.StoredGrid;
        foreach (GridTile tile in grid)
        {
            if (tile.myPos.x > grid.GetLength(0) / 2) tile.ForceGrayColor();
        }

        pvpStartButton.onClick.RemoveAllListeners();
        pvpStartButton.onClick.AddListener(PlayerOneConfirmReady);
        pvpStartButtonText.text = "Click when Player 1 is ready";
    }

    public void PlayerOneConfirmReady()
    {

        GridClicker.Instance.teamToDraw = PvPController.Teams.P2;
        GridClicker.Instance.allowDrawOnRightSide = true;
        GridClicker.Instance.allowDrawOnLeftSide = false;

        GridTile[,] grid = GameController.Instance.Container.StoredGrid;
        foreach (GridTile tile in grid)
        {
            if (tile.myPos.x < grid.GetLength(0) / 2) tile.ForceGrayColor();
            else tile.UpdateStatus();
        }


        pvpStartButton.onClick.RemoveAllListeners();
        pvpStartButton.onClick.AddListener(PlayerTwoConfirmReady);
        pvpStartButtonText.text = "Click when Player 2 is ready";
    }

    public void PlayerTwoConfirmReady()
    {
        GridTile[,] grid = GameController.Instance.Container.StoredGrid;
        foreach (GridTile tile in grid)
        {
           tile.UpdateStatus();
        }

        pvpStartButton.onClick.RemoveAllListeners();
        pvpStartButton.enabled = false;
        pvpStartButtonText.text = "--";


        GridClicker.Instance.teamToDraw = PvPController.Teams.None;
        GridClicker.Instance.allowDrawOnRightSide = false;
        GridClicker.Instance.allowDrawOnRightSide = false;


        ToggleSimulationOptionPanel();
        displaySimulationOptionsButton.enabled = true;
        killAllButton.enabled = false;
        reRandomizeButton.enabled = false;

        UpdateJumbotron();
    }
    #endregion

    public void UpdateJumbotron()
    {
        if (!jumbotron.gameObject.activeSelf) return;

        Vector2Int aliveUnits = PvPController.Instance.CountTeams();

        playerOneInfoText.text = aliveUnits.x.ToString();
        playerTwoInfoText.text = aliveUnits.y.ToString();

        curIttInfoText.text = GameController.Instance.curItterations.ToString();

    }

    public void GameOverDisplay(PvPController.Teams victor)
    {
        victoryScreen.gameObject.SetActive(true);

        if (victor == PvPController.Teams.None)
        {
            winnerText.text = "TIE";
            return;
        }

        if (victor == PvPController.Teams.P1)
        {
            winnerText.text = "Player 1 Wins!";
            winnerText.color = Color.blue;
            return;
        }

        if (victor == PvPController.Teams.P2)
        {
            winnerText.text = "Player 2 Wins!";
            winnerText.color = Color.red;
            return;
        }

    }

    public void ReloadGame()
    {
        SceneManager.LoadScene("GameOfLife");
    }
}
