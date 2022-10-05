using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    
    public Player TurnPlayer { get; private set; }

    private Board board;

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
        TurnPlayer = null;
        board = FindObjectOfType<Board>();
    }

    private void Start()
    {
        TurnPlayer = GameStateManager.Instance.GetRandomPlayer();
        InitiatePlayerTurn(TurnPlayer);
    }

    public void EndTurn()
    {
        TurnPlayer.ConsumeFood();
        InitiatePlayerTurn(GetNextTurnPlayer());
    }

    private void InitiatePlayerTurn(Player player)
    {
        TurnPlayer = player;
        player.MaxActionPoints = GameStateManager.Instance.RollDice();
        player.ActionPoints = player.MaxActionPoints;
        HUDManager.Instance.OnInitiatePlayerTurn();
    }

    private Player GetNextTurnPlayer()
    {
        var players = GameStateManager.Instance.Players;
        int currentIndex = players.IndexOf(TurnPlayer);
        int newIndex = (currentIndex == players.Count - 1) ? 0 : currentIndex + 1;
        return players[newIndex];
    }
}