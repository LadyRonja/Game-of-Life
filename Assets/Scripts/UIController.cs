using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] TMP_Text toggleSimulationText;
    [Space]
    [SerializeField] Slider emulationSpeedSlider;
    [SerializeField] TMP_Text emulationSpeedText;
    [SerializeField] Slider randomizeAlivePercantageSlider;
    [SerializeField] TMP_Text randomizeAliveText;

    private void Start()
    {
        if (GameController.Instance.IsRunning) toggleSimulationText.text = "Pause Simulation";
        else toggleSimulationText.text = "Start Simulation";


        emulationSpeedText.text = Math.Round(emulationSpeedSlider.value, 3).ToString();
        randomizeAliveText.text = Math.Round(randomizeAlivePercantageSlider.value).ToString();
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

}
