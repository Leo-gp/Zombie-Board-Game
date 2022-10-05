using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Piece : MonoBehaviour
{
    public TextMeshProUGUI typeText;
    
    public PieceTypes Type { get; private set; }
    public Vector2 BoardPosition { get; private set; }
    public List<Resource> Resources { get; private set; }

    private Board board;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        board = FindObjectOfType<Board>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetTypeText();
    }

    public void SetType(PieceTypes pieceType)
    {
        Type = pieceType;
    }

    public void SetBoardPosition(Vector2 position)
    {
        BoardPosition = position;
    }

    public void SetResources(List<Resource> resources)
    {
        Resources = resources;
    }

    private void SetTypeText()
    {
        switch (Type)
        {
            case PieceTypes.House:
                typeText.text = "House";
                break;
            case PieceTypes.Warehouse:
                typeText.text = "Warehouse";
                break;
            case PieceTypes.GunShop:
                typeText.text = "Gun Shop";
                break;
            case PieceTypes.Hospital:
                typeText.text = "Hospital";
                break;
            case PieceTypes.Market:
                typeText.text = "Market";
                break;
            case PieceTypes.Laboratory:
                typeText.text = "Laboratory";
                break;
            case PieceTypes.Infestation:
                typeText.text = "Infestation";
                break;
        }
    }

    private void Highlight()
    {
        spriteRenderer.color = GameStateManager.Instance.pieceHighlightColor;
    }

    private void Unhighlight()
    {
        spriteRenderer.color = GameStateManager.Instance.pieceDefaultColor;
    }

    private void OnMouseDown() 
    {
        var turnPlayer = TurnManager.Instance.TurnPlayer;
        if (turnPlayer.Piece != this)
        {
            turnPlayer.MoveToPiece(this);
        }
        else
        {
            var playersAtPiece = board.PlayersAtPiece(this);
            for (int i = 0; i < playersAtPiece.Count; i++)
            {
                if (playersAtPiece[i] != turnPlayer)
                {
                    turnPlayer.AttackPlayer(playersAtPiece[i]);
                    break;
                }
            }
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TurnManager.Instance.TurnPlayer.InteractWithPiece(this);
        }
    }

    private void OnMouseEnter()
    {
        Highlight();
    }

    private void OnMouseExit()
    {
        Unhighlight();
    }
}