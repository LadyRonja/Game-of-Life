using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PvPController : MonoBehaviour
{
    public static PvPController Instance;

    public enum Teams { None, P1, P2 };
    Teams activePlayer = Teams.None;
    public bool playersCanPlace = false;
    public int maxItterations = 10000;
    float changeTeamAtNeighbourPercent = 0.6f;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        #endregion
    }

    /// <summary>
    /// Run right before updating status, assumes shouldBeAlive status of passed tile and neighbours is accurate.
    /// </summary>
    public void DetermineTileTeam(GridTile tile)
    {
        /// RULES:
        /// If about to be dead, leave team.
        /// If about to be alive:
        ///      If not on a team
        ///          Join the team that most of your neighbours are on.
        ///              If tied, join no team.
        ///      If already on a team
        ///          Change team if 60% or more of neighbours are on another team
        ///          Otherwise stay the same

        if (tile.shouldBeAlive == false)
        {
            tile.teamToJoin = Teams.None;
            return;
        }

        int amountOfNeighbours = tile.neighbours.Count;
        List<GridTile> noTeamNeighbours = new();
        List<GridTile> team1Neighbours= new();
        List<GridTile> team2Neighbours = new();

        foreach (GridTile neighbour in tile.neighbours)
        {
            if (neighbour.myTeam == Teams.P1)
                team1Neighbours.Add(neighbour);
            else if(neighbour.myTeam == Teams.P2)
                team2Neighbours.Add(neighbour);
            else
                noTeamNeighbours.Add(neighbour);
        }

        if (tile.myTeam == Teams.None)
        {
            if (team1Neighbours.Count > team2Neighbours.Count)
                tile.teamToJoin = Teams.P1;
            else if (team1Neighbours.Count < team2Neighbours.Count)
                tile.teamToJoin = Teams.P2;

            return;
        }

        if (tile.myTeam == Teams.P1)
        {
            if(team2Neighbours.Count/amountOfNeighbours >= changeTeamAtNeighbourPercent)
                tile.teamToJoin = Teams.P2;

            return;
        }

        if (tile.myTeam == Teams.P2)
        {
            if (team1Neighbours.Count / amountOfNeighbours >= changeTeamAtNeighbourPercent)
                tile.teamToJoin = Teams.P1;

            return;
        }

    }
}


