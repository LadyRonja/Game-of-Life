using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class GameController : MonoBehaviour
{
    public bool isRunning = false;
    [SerializeField] float itterationSpeed = 0.05f;
    float itterationTimer = 1f;
    [SerializeField] private GridContainer container;
    [SerializeField] private Slider emulationSpeedSlider;

    private void Update()
    {
        if (!isRunning) { return; }
        if (emulationSpeedSlider != null) { itterationSpeed = emulationSpeedSlider.value; }

        itterationTimer -= Time.deltaTime;

        if (itterationTimer <= 0)
        {
            itterationTimer = itterationSpeed;
            UpdateGameState(container.StoredGrid);
        }
    }

    private void UpdateGameState(GridTile[,] grid)
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                int aliveNeighbours = 0;
                foreach (GridTile neighbour in grid[x,y].neighbours)
                {
                    if (neighbour == null) continue;
                    if (neighbour.IsAlive)
                        aliveNeighbours++;
                }

                if (grid[x, y].IsAlive)
                {
                    if (aliveNeighbours < 2)
                        grid[x, y].shouldBeAlive = false;
                    else if (aliveNeighbours > 3)
                        grid[x, y].shouldBeAlive = false;
                }
                else
                    if (aliveNeighbours == 3)
                        grid[x, y].shouldBeAlive = true;
            }
        }

        foreach (GridTile tile in grid)
        {
            tile.UpdateStatus();
        }

    }

    public void FindGrid()
    {
        container = GameObject.Find("Generated Grid").GetComponent<GridContainer>();
    }

    public void ToggleIsRunning()
    {
        if(container == null) FindGrid();
        isRunning= !isRunning;
    }

    public void NextTick()
    {
        if (container == null) FindGrid();
        UpdateGameState(container.StoredGrid);
    }

}
