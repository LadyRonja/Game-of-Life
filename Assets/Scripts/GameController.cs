using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private bool isRunning = false;
    public bool IsRunning { get => isRunning; }

    public bool playingPvP = false;
    public uint maxItterations = uint.MaxValue;
    public uint curItterations = 0;

    [SerializeField] float itterationSpeed = 0.05f;
    float itterationTimer = 1f;
    private GridContainer container;
    public GridContainer Container { 
        get { 
            if(container == null)
                FindGrid();
            return container; 
        }
        set { 
            container = value; 
        } 
    }

    [SerializeField] GridCreator gridCreatorPrefab;

    private void Awake()
    {
        #region Singleton
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        #endregion
    }

    private void Update()
    {
        if (!isRunning) { return; }
        if (playingPvP && curItterations >= maxItterations) { return; }

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
                    {
                        grid[x, y].shouldBeAlive = false;
                        if (playingPvP /*true*/)
                        {
                            foreach (GridTile neighbour in grid[x, y].neighbours)
                            {
                                if (neighbour.myTeam != PvPController.Teams.None && neighbour.myTeam != grid[x, y].myTeam)
                                {
                                    AudioController.Instance.PlayUnitDeathSound();
                                    break;
                                }
                            }
                        }
                    }
                    else if (aliveNeighbours > 3)
                    {
                        grid[x, y].shouldBeAlive = false;
                    }
                }
                else
                    if (aliveNeighbours == 3)
                        grid[x, y].shouldBeAlive = true;
            }
        }

        if (playingPvP)
        {
            foreach (GridTile tile in grid)
            {
                PvPController.Instance.DetermineTileTeam(tile);
            }
        }

        foreach (GridTile tile in grid)
        {
            tile.UpdateStatus();
        }



        curItterations++;
        if (playingPvP)
        {
            UIController.Instance.UpdateJumbotron();
            PvPController.Instance.CheckGameOver();
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

    public void ToggleIsRunning(out bool running)
    {
        ToggleIsRunning();
        running = isRunning;
    }

    public void NextTick()
    {
        if (container == null) FindGrid();
        UpdateGameState(container.StoredGrid);
    }

    public void KillAll()
    {
        if (container == null) FindGrid();
        foreach (GridTile tile in container.StoredGrid)
        {
            tile.ForceState(false, PvPController.Teams.None);
        }
        curItterations = 0;
    }

    public void SetEmulationSpeed(float speed)
    {
        itterationSpeed = speed;
    }

    public void RandomizeAlive(float percentageAlive)
    {
        KillAll();
        foreach (GridTile tile in container.StoredGrid)
        {
            if (UnityEngine.Random.Range(0, 100) < percentageAlive)
            {
                tile.shouldBeAlive= true;
                tile.UpdateStatus();
            }
        }
    }
}
