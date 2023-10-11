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
    float changeTeamAtNeighbourPercent = 0.1f; // Lowever number makes more volitile, optimally keept between 0.1 and 0.7

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
        ///          Change team if certain % or more of alive neighbours are on another team
        ///          Otherwise stay the same

        if (tile.shouldBeAlive == false)
        {
            tile.teamToJoin = Teams.None;
            return;
        }

        int amountOfNeighbours = tile.neighbours.Count;
        int aliveNeighboursTot = 0;
        List<GridTile> noTeamNeighbours = new();
        List<GridTile> team1Neighbours= new();
        List<GridTile> team2Neighbours = new();

        foreach (GridTile neighbour in tile.neighbours)
        {
            if (neighbour.IsAlive) aliveNeighboursTot++;

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

            if(team2Neighbours.Count/aliveNeighboursTot >= changeTeamAtNeighbourPercent)
            {
                tile.teamToJoin = Teams.P2;
                AudioController.Instance.PlayTeamChangeSound();
            }

            return;
        }

        if (tile.myTeam == Teams.P2)
        {
            if (team1Neighbours.Count / aliveNeighboursTot >= changeTeamAtNeighbourPercent)
            {
                tile.teamToJoin = Teams.P1;
                AudioController.Instance.PlayTeamChangeSound();
            }

            return;
        }

    }

    public Vector2Int CountTeams()
    {
        GridTile[,] tiles = GameController.Instance.Container.StoredGrid;
        Vector2Int output = new();

        foreach (GridTile tile in tiles)
        {
            if (tile.myTeam == Teams.P1) output.x++;
            else if (tile.myTeam == Teams.P2) output.y++;
        }

        return output;
    }

    public void CheckGameOver()
    {
        GridTile[,] tiles = GameController.Instance.Container.StoredGrid;
        Vector2Int teamMembers = new();

        foreach (GridTile tile in tiles)
        {
            if (tile.myTeam == Teams.P1) teamMembers.x++;
            else if (tile.myTeam == Teams.P2) teamMembers.y++;
        }

        if(teamMembers.x == 0 & teamMembers.y == 0)
        {
            GameOver(Teams.None);
        }
        else if(teamMembers.x == 0)
        {
            GameOver(Teams.P2);
        }
        else if (teamMembers.y == 0)
        {
            GameOver(Teams.P1);
        }
        else if(teamMembers.x > teamMembers.y && GameController.Instance.curItterations >= GameController.Instance.maxItterations)
        {
            GameOver(Teams.P1);
        }
        else if (teamMembers.y > teamMembers.x && GameController.Instance.curItterations >= GameController.Instance.maxItterations)
        {
            GameOver(Teams.P2);
        }
        else if (GameController.Instance.curItterations >= GameController.Instance.maxItterations)
        { GameOver(Teams.None);
        }
    }

    public void GameOver(Teams winner)
    {
        UIController.Instance.GameOverDisplay(winner);
    }
}


