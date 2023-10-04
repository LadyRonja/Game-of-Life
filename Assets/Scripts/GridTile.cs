using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridTile : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public Vector2Int myPos;

    public GridTile neighbourNorth;
    public GridTile neighbourEast;
    public GridTile neighbourSouth;
    public GridTile neighbourWest;

    public GridTile neighbourNorthEast;
    public GridTile neighbourNorthWest;
    public GridTile neighbourSouthEast;
    public GridTile neighbourSouthWest;

    public List<GridTile> neighbours;

    private bool isAlive = true;
    public bool IsAlive { get => isAlive; }
    public bool shouldBeAlive = true;

    [SerializeField] SpriteRenderer mySpriteRenderer;

    public void UpdateStatus()
    {
        if (shouldBeAlive)
        {
            isAlive = true;
            mySpriteRenderer.color = Color.white;
        }
        else
        {
            isAlive = false;
            mySpriteRenderer.color = Color.black;
        }
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        GridClicker.Instance.OnClickDownDraw(myPos);      
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Input.GetMouseButton((int)MouseButton.Left)) return;

        GridClicker.Instance.OnClickDraw(myPos);
    }

    public void ForceState(bool forceAlive)
    {
        shouldBeAlive = forceAlive;
        isAlive = forceAlive;
        if (forceAlive)
            mySpriteRenderer.color = Color.white;
        else
            mySpriteRenderer.color = Color.black;
    }
}
