using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    public enum Rotation {Deg0, Deg90, Deg180, Deg270};
    public enum Shapes { Glider, Line, Acorn }

    public static int[,] GetShape(Shapes shape, Rotation rotation)
    {
        switch (shape)
        {
            case Shapes.Glider:
                return Glider(rotation);
            case Shapes.Line:
                return Line(rotation);
            case Shapes.Acorn:
                return Acorn(rotation);
            default:
                Debug.LogError("Reached default of from statemachine, missing cases?");
                Debug.LogError("Returning Glider with 0deg rotation.");
                return Glider(Rotation.Deg0);
        }
    }

    #region Shapes
    public static int[,] Glider(Rotation rotation)
    {
        // - X -
        // - - X
        // X X X

        int[,] output = {
            {0, 1, 0 },
            { 0, 0, 1 },
            { 1, 1, 1 } 
        };


        output = RotateMatrix90Deg(output);

        int rotate90degAmount = 0;

        if (rotation == Rotation.Deg90) rotate90degAmount = 1;
        else if(rotation == Rotation.Deg180) rotate90degAmount = 2;
        else if (rotation == Rotation.Deg270) rotate90degAmount = 3;

        for (int i = 0; i < rotate90degAmount; i++)
        {
            output = RotateMatrix90Deg(output);
        }

        return output;
    }

    public static int[,] Line(Rotation rotation)
    {
        // X X X X X X

        int[,] output = {
            {1, 1, 1, 1 , 1, 1 } 
        };


        output = RotateMatrix90Deg(output);

        int rotate90degAmount = 0;

        if (rotation == Rotation.Deg90) rotate90degAmount = 1;
        else if (rotation == Rotation.Deg180) rotate90degAmount = 2;
        else if (rotation == Rotation.Deg270) rotate90degAmount = 3;

        for (int i = 0; i < rotate90degAmount; i++)
        {
            output = RotateMatrix90Deg(output);
        }

        return output;
    }

    public static int[,] Acorn(Rotation rotation)
    {
        // - X - - - - -
        // - - - X - - -
        // X X - - X X X

        int[,] output = {
            { 0, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0 },
            { 1, 1, 0, 0, 1, 1, 1 },
        };

        output = RotateMatrix90Deg(output);

        int rotate90degAmount = 0;

        if (rotation == Rotation.Deg90) rotate90degAmount = 1;
        else if (rotation == Rotation.Deg180) rotate90degAmount = 2;
        else if (rotation == Rotation.Deg270) rotate90degAmount = 3;

        for (int i = 0; i < rotate90degAmount; i++)
        {
            output = RotateMatrix90Deg(output);
        }

        return output;
    }

    #endregion

    private static int[,] RotateMatrix90Deg(int[,] matrixToRotate)
    {
        int[,] output = new int[matrixToRotate.GetLength(1), matrixToRotate.GetLength(0)];

        for (int oldY = 0; oldY < matrixToRotate.GetLength(1); oldY++)
        {
            int newY = 0;
            for (int oldX = matrixToRotate.GetLength(0) - 1; oldX >= 0; oldX--)
            {
                output[oldY, newY] = matrixToRotate[oldX, oldY];
                newY++;
            }
        }

        return output;
    }

    public static int[,] FlipMatrixOnXAxis(int[,] matrixToRotate)
    {
        int[,] output = new int[matrixToRotate.GetLength(0), matrixToRotate.GetLength(1)];

        for (int y = 0; y < matrixToRotate.GetLength(1); y++)
        {
            for (int x = 0; x < matrixToRotate.GetLength(0); x++)
            {
                output[x, y] = matrixToRotate[matrixToRotate.GetLength(0) - 1 - x, y];
            }
        }

        return output;
    }

    public static int[,] FlipMatrixOnYAxis(int[,] matrixToRotate)
    {
        int[,] output = new int[matrixToRotate.GetLength(0), matrixToRotate.GetLength(1)];

        for (int y = 0; y < matrixToRotate.GetLength(1); y++)
        {
            for (int x = 0; x < matrixToRotate.GetLength(0); x++)
            {
                output[x, y] = matrixToRotate[x, matrixToRotate.GetLength(1) - 1 - y];
            }
        }

        return output;
    }
}
