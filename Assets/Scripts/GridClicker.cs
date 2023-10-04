using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridClicker : MonoBehaviour
{
    public static GridClicker Instance;
    public enum DrawMode { Free, Force_Alive, Force_Dead, Shape }

    public DrawMode currentDrawMode = DrawMode.Free;

    [Header("Shapes")]
    public ShapeStorage.Shapes shapeToDraw = ShapeStorage.Shapes.Glider;
    public ShapeStorage.Rotation shapeRotation = ShapeStorage.Rotation.Deg0;
    public bool flipShapeOnAxisX;
    public bool flipShapeOnAxisY;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        #endregion
    }

    public void OnClickDownDraw(Vector2Int startPos)
    {
        switch (currentDrawMode)
        {
            case DrawMode.Free:
                FreeDraw(startPos);
                break;
            case DrawMode.Shape:
                DrawShape(startPos);
                break;
            case DrawMode.Force_Alive:
                ForceState(startPos, true);
                break;
            case DrawMode.Force_Dead:
                ForceState(startPos, false);
                break;
            default:
                Debug.LogError("Reached end of Switch-state machine, new states not added?");
                currentDrawMode = DrawMode.Free;
                break;
        }
    }

    public void OnClickDraw(Vector2Int startPos)
    {
        switch (currentDrawMode)
        {
            case DrawMode.Free:
                FreeDraw(startPos);
                break;
            case DrawMode.Shape:
                // Do nothing
                break;
            case DrawMode.Force_Alive:
                ForceState(startPos, true);
                break;
            case DrawMode.Force_Dead:
                ForceState(startPos, false);
                break;
            default:
                Debug.LogError("Reached end of Switch-state machine, new states not added?");
                currentDrawMode = DrawMode.Free;
                break;
        }
    }

    private void FreeDraw(Vector2Int startPos)
    {
        GridTile tileToEdit = GameController.Instance.Container.StoredGrid[startPos.x, startPos.y];
        tileToEdit.shouldBeAlive = !tileToEdit.shouldBeAlive;
        tileToEdit.UpdateStatus();
    }

    private void DrawShape(Vector2Int startPos)
    {
        GridTile[,] grid = GameController.Instance.Container.StoredGrid;
        int[,] shape = ShapeStorage.GetShape(shapeToDraw, shapeRotation);
        int xMax = grid.GetLength(0);
        int yMax = grid.GetLength(1);

        if (startPos.x + shape.GetLength(0) > xMax - 1 || startPos.y + shape.GetLength(1) > yMax - 1) 
            return;

        for (int y = 0; y < shape.GetLength(1); y++)
        {
            for (int x = 0; x < shape.GetLength(0); x++)
            {
                grid[startPos.x + x, startPos.y + y].ForceState(Convert.ToBoolean(shape[x, y]));
            }
        }

    }

    private void ForceState(Vector2Int startPos, bool forceAlive)
    {
        GameController.Instance.Container.StoredGrid[startPos.x, startPos.y].ForceState(forceAlive);
    }

}
