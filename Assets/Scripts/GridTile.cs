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

    public PvPController.Teams myTeam = PvPController.Teams.None;
    public PvPController.Teams teamToJoin = PvPController.Teams.None;

    Dictionary<PvPController.Teams, Color> teamColors = new()
    {
        { PvPController.Teams.None, Color.white},
        { PvPController.Teams.P1, Color.blue},
        { PvPController.Teams.P2, Color.red},
    };

    [SerializeField] SpriteRenderer mySpriteRenderer;

    public void UpdateStatus()
    {
        myTeam = teamToJoin;

        if (shouldBeAlive)
        {
            isAlive = true;
            mySpriteRenderer.color = GetTeamColor();
        }
        else
        {
            isAlive = false;
            mySpriteRenderer.color = GetTeamColor();
        }

    }

    private Color GetTeamColor()
    {
        if (isAlive)
        {
            if (teamColors.TryGetValue(myTeam, out Color colorToBe))
                return colorToBe;
            else
            {
                Debug.LogError("Failed to get teamcolor from dictionary, teams not added?");
                Debug.Log("Setting Color to white");
                return Color.white;
            }
        }
        else
        {
            return Color.black;
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
        mySpriteRenderer.color = GetTeamColor();
    }

    public void ForceState(bool forceAlive, PvPController.Teams forceTeam)
    {
        myTeam = forceTeam;
        teamToJoin = forceTeam;
        ForceState(forceAlive);
    }
}
