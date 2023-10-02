using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private bool isRunning = false;
    public bool IsRunning { get => isRunning; }
    [SerializeField] float itterationSpeed = 0.05f;
    float itterationTimer = 1f;
    [SerializeField] private GridContainer container;


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
        if(container == null) container = GameObject.Find("Generated Grid").GetComponent<GridContainer>();
        foreach (GridTile tile in container.StoredGrid)
        {
            tile.ForceKill();
        }
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
            if (Random.Range(0, 100) < percentageAlive)
            {
                tile.shouldBeAlive= true;
                tile.UpdateStatus();
            }
        }
    }
}
