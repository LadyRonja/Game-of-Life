using Unity.VisualScripting;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public bool generateOnStart = true;
    public bool randomizeAlive = false;
    [Range(0, 100)] public float deadOnSpawn = 20;
    [Space]
    public GameObject tilePrefab;

    [Header("Grid Data")]
    public Vector2Int dimensions;
    public float offSet;
    public float padding;
    public Transform startPosition;
    public bool gridWrap = true;


    private void Start()
    {
        if(generateOnStart)
            GenerateEmptyGrid(dimensions.x, dimensions.y, randomizeAlive);
    }

    private void Update()
    {
        #region ForDebuggin
        if (Input.GetKeyDown(KeyCode.R))
        {
           Destroy(GameObject.Find("Generated Grid"));
           GenerateEmptyGrid(dimensions.x, dimensions.y, randomizeAlive);
        }
        #endregion
    }


    public void GenerateEmptyGrid(int width, int heigth, bool randomize)
    {
        GameObject parent = new GameObject();
        parent.name = "Generated Grid";
        parent.transform.AddComponent<GridContainer>();
        GridContainer container = parent.GetComponent<GridContainer>();
        container.StoredGrid = new GridTile[width, heigth];

        for (int y = 0; y < heigth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 startPos = startPosition.position;
                Vector3 targetPos = new Vector3(startPos.x + offSet * x, startPos.y + offSet * y , 0);
                GameObject tileObj = Instantiate(tilePrefab, targetPos, Quaternion.identity, parent.transform);
                GridTile tileScr = tileObj.GetComponent<GridTile>();

                tileObj.transform.name = $"{x}, {y}";
                tileScr.myPos = new Vector2Int(x, y);
                tileScr.shouldBeAlive = false;
                tileScr.UpdateStatus();
                
                container.StoredGrid[x, y] = tileScr;
            }
        }

        ConnectNeighbour(container.StoredGrid);

        if (randomize)
        {
            RandomizeGrid(container.StoredGrid);
        }

    }

    private void ConnectNeighbour(GridTile[,] grid)
    {
        int yMax = grid.GetLength(1);
        int xMax = grid.GetLength(0);

        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                // South
                if (y != 0)
                {
                    grid[x, y].neighbourSouth = grid[x, y - 1];
                    grid[x, y].neighbours.Add(grid[x, y].neighbourSouth);
                }
                else if(gridWrap)
                {
                    grid[x, y].neighbourSouth = grid[x, yMax - 1];
                    grid[x, y].neighbours.Add(grid[x, y].neighbourSouth);
                }

                // North
                if(y != yMax - 1)
                {
                    grid[x, y].neighbourNorth = grid[x, y + 1];
                    grid[x, y].neighbours.Add(grid[x, y].neighbourNorth);
                }
                else if (gridWrap)
                {
                    grid[x, y].neighbourNorth = grid[x, 0];
                    grid[x, y].neighbours.Add(grid[x, y].neighbourNorth);
                }

                // West
                if (x != 0)
                {
                    grid[x, y].neighbourWest = grid[x - 1, y];
                    grid[x, y].neighbours.Add(grid[x, y].neighbourWest);
                }
                else if (gridWrap)
                {
                    grid[x, y].neighbourWest = grid[xMax - 1, y];
                    grid[x, y].neighbours.Add(grid[x, y].neighbourWest);
                }

                // East
                if (x != xMax - 1)
                {
                    grid[x, y].neighbourEast = grid[x + 1, y];
                    grid[x, y].neighbours.Add(grid[x, y].neighbourEast);
                }
                else if (gridWrap)
                {
                    grid[x, y].neighbourEast = grid[0, y];
                    grid[x, y].neighbours.Add(grid[x, y].neighbourEast);
                }

            }
        }

        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                if(grid[x, y].neighbourNorth != null)
                {
                    grid[x, y].neighbourNorthEast ??= grid[x, y].neighbourNorth.neighbourEast;
                    grid[x, y].neighbours.Add(grid[x, y].neighbourNorthEast);
                    grid[x, y].neighbourNorthWest ??= grid[x, y].neighbourNorth.neighbourWest;
                    grid[x, y].neighbours.Add(grid[x, y].neighbourNorthWest);
                }

                if (grid[x, y].neighbourSouth != null)
                {
                    grid[x, y].neighbourSouthEast ??= grid[x, y].neighbourSouth.neighbourEast;
                    grid[x, y].neighbours.Add(grid[x, y].neighbourSouthEast);
                    grid[x, y].neighbourSouthWest ??= grid[x, y].neighbourSouth.neighbourWest;
                    grid[x, y].neighbours.Add(grid[x, y].neighbourSouthWest);
                }

            }
        }
    }

    private void RandomizeGrid(GridTile[,] grid)
    {
        foreach (GridTile tile in grid)
        {
            int random = Random.Range(0, 100);
            if (random < deadOnSpawn)
            {
                tile.shouldBeAlive = false;
            }
            else
            {
                tile.shouldBeAlive = true;
            }

            tile.UpdateStatus();
        }
    }
}
