using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public Player playerPrefab;
    public int initialNumberOfPlayers;
    public Color[] playerColors;
    public List<PieceConfig> piecesConfigs;
    public Color pieceDefaultColor;
    public Color pieceHighlightColor;
    public int diceSides;

    public static GameStateManager Instance { get; private set; }
    
    public List<Player> Players { get; private set; }

    private Board board;
    private Dice dice;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        board = FindObjectOfType<Board>();
        board.CreateBoard();
        InstantiatePlayers();
        dice = new Dice();
        dice.Sides = diceSides;
    }

    private void Start()
    {
        SetPlayersStartingPositions();
    }

    public int RollDice()
    {
        return dice.Roll();
    }

    public Player GetRandomPlayer()
    {
        return Players[Random.Range(0, Players.Count)];
    }

    public void OnPlayerDeath(Player player)
    {
        Players.Remove(player);
        Destroy(player.gameObject);
        if (Players.Count == 1)
        {
            GameOver();
        }
    }

    public void Rematch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void InstantiatePlayers()
    {
        Players = new List<Player>();
        for (int i = 0; i < initialNumberOfPlayers; i++)
        {
            var player = Instantiate(playerPrefab);
            player.name = "Player " + (i + 1);
            player.GetComponent<SpriteRenderer>().color = playerColors[i];
            Players.Add(player);
        }
    }

    private void SetPlayersStartingPositions()
    {
        foreach (var player in Players)
        {
            var piece = board.FindPieceByBoardPosition(board.GetRandomBoardPosition(true));
            player.transform.position = new Vector3(piece.transform.position.x, piece.transform.position.y, 0);
            player.Piece = piece;
        }
    }

    private void GameOver()
    {
        HUDManager.Instance.OnGameOver();
    }

    /*private Color GetRandomUniquePlayerColor()
    {
        if (numberOfPlayers > playerColors.Length)
        {
            throw new System.Exception("Unable to get unique color: there are more players than colors.");
        }
        var color = new Color();
        var uniqueColor = false;
        while (!uniqueColor)
        {
            color = playerColors[Random.Range(0, playerColors.Length)];
            uniqueColor = true;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].GetComponent<SpriteRenderer>().color == color)
                {
                    uniqueColor = false;
                    break;
                }
            }
        }
        return color;
    }*/
}