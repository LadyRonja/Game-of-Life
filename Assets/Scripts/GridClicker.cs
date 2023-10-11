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
    public ShapeFactory.Shapes shapeToDraw = ShapeFactory.Shapes.Glider;
    public ShapeFactory.Rotation shapeRotation = ShapeFactory.Rotation.Deg0;
    public bool flipShapeOnAxisX;
    public bool flipShapeOnAxisY;

    [Header("PVP")]
    public PvPController.Teams teamToDraw = PvPController.Teams.None;
    public int maxUnitsToPlace = 3000;
    public bool allowDrawOnRightSide = true;
    public bool allowDrawOnLeftSide = true;

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

        UIController.Instance.UpdateJumbotron();
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

        UIController.Instance.UpdateJumbotron();
    }

    private void FreeDraw(Vector2Int startPos)
    {
        GridTile[,] grid = GameController.Instance.Container.StoredGrid;
        GridTile tileToEdit = grid[startPos.x, startPos.y];
        if (!allowDrawOnRightSide && tileToEdit.myPos.x > grid.GetLength(0)/2) return;
        if (!allowDrawOnLeftSide && tileToEdit.myPos.x < grid.GetLength(0)/2) return;

        tileToEdit.shouldBeAlive = !tileToEdit.shouldBeAlive;
        tileToEdit.teamToJoin = teamToDraw;
        tileToEdit.UpdateStatus();
    }

    private void DrawShape(Vector2Int startPos)
    {
        GridTile[,] grid = GameController.Instance.Container.StoredGrid;
        int[,] shape = ShapeFactory.GetShape(shapeToDraw, shapeRotation);
        int xMax = grid.GetLength(0);
        int yMax = grid.GetLength(1);

        if (startPos.x + shape.GetLength(0) > xMax - 1 || startPos.y + shape.GetLength(1) > yMax - 1) 
            return;

        
        if (!allowDrawOnRightSide && startPos.x + shape.GetLength(0) > grid.GetLength(0)/2) return;
        if (!allowDrawOnLeftSide && startPos.x < grid.GetLength(0)/2) return;


        if (flipShapeOnAxisX) shape = ShapeFactory.FlipMatrixOnXAxis(shape);
        if (flipShapeOnAxisY) shape = ShapeFactory.FlipMatrixOnYAxis(shape);

        for (int y = 0; y < shape.GetLength(1); y++)
        {
            for (int x = 0; x < shape.GetLength(0); x++)
            {
                // Allow overlapping with existing shapes
                if (shape[x, y] == 1)
                {  
                    GridTile tileToMod = grid[startPos.x + x, startPos.y + y];
                    tileToMod.ForceState(Convert.ToBoolean(shape[x, y]), teamToDraw);
                }
            }
        }
    }

    private void ForceState(Vector2Int startPos, bool forceAlive)
    {

        GridTile[,] grid = GameController.Instance.Container.StoredGrid;
        GridTile tileToEdit = grid[startPos.x, startPos.y];
        if (!allowDrawOnRightSide && tileToEdit.myPos.x > grid.GetLength(0) / 2) return;
        if (!allowDrawOnLeftSide && tileToEdit.myPos.x < grid.GetLength(0) / 2) return;

        if(forceAlive) tileToEdit.ForceState(forceAlive, teamToDraw);
        else tileToEdit.ForceState(forceAlive, PvPController.Teams.None);
    }

}
