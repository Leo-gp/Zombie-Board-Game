using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Vector2 size;
    public Vector2 pieceOffset = new Vector2(1.5f, 1.5f);
    public Piece piecePrefab;

    private List<Piece> pieces;

    public void CreateBoard()
    {
        pieces = new List<Piece>();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var piece = CreatePiece(new Vector2(x * pieceOffset.x, y * pieceOffset.y), new Vector2(x, y));
                piece.name = "Piece (" + x + "," + y + ")";
                pieces.Add(piece);
            }
        }
    }

    public Piece FindPieceByBoardPosition(Vector2 boardPosition)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].BoardPosition.x == boardPosition.x && pieces[i].BoardPosition.y == boardPosition.y)
            {
                return pieces[i];
            }
        }
        return null;
    }

    public Vector2 GetRandomBoardPosition(bool shouldBeEmpty)
    {
        if (GameStateManager.Instance.initialNumberOfPlayers >= pieces.Count)
        {
            shouldBeEmpty = false;
        }
        var position = new Vector2();
        var validPosition = false;
        do
        {
            int posX = Random.Range(0, (int) size.x);
            int posY = Random.Range(0, (int) size.y);
            position.x = posX;
            position.y = posY;
            validPosition = true;
            if (shouldBeEmpty && PlayersAtPiece(GetPieceAtBoardPosition(new Vector2(posX, posY))).Count > 0)
            {
                validPosition = false;
            }
        }
        while(!validPosition);
        return position;
    }

    public List<Player> PlayersAtPiece(Piece piece)
    {
        var players = GameStateManager.Instance.Players;
        var playersAtPiece = new List<Player>();
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Piece == piece)
            {
                playersAtPiece.Add(players[i]);
            }
        }
        return playersAtPiece;
    }

    public int GetDistanceBetweenPieces(Piece piece1, Piece piece2)
    {
        return (int) Vector2.Distance(piece1.BoardPosition, piece2.BoardPosition);
    }

    /*public int GetDistanceBetweenPieces(Piece piece1, Piece piece2)
    {
        Vector2 diff = piece1.BoardPosition - piece2.BoardPosition;
        return (int) (Mathf.Abs(diff.x) + Mathf.Abs(diff.y));
    }*/

    private Piece CreatePiece(Vector2 position, Vector2 boardPosition)
    {
        var piece = Instantiate<Piece>(piecePrefab, this.transform);
        piece.transform.localPosition = new Vector3(position.x, position.y, 0);
        piece.transform.localRotation = Quaternion.identity;
        piece.SetType(GetRandomPieceType());
        piece.SetBoardPosition(boardPosition);
        piece.SetResources(GetResourcesForPiece(piece));
        return piece;
    }

    private PieceTypes GetRandomPieceType()
    {
        var piecesConfigs = GameStateManager.Instance.piecesConfigs;
        var pieceConfig = piecesConfigs[Random.Range(0, piecesConfigs.Count)];
        return pieceConfig.pieceType;
    }

    private List<Resource> GetResourcesForPiece(Piece piece)
    {
        var piecesConfigs = GameStateManager.Instance.piecesConfigs;
        for (int i = 0; i < piecesConfigs.Count; i++)
        {
            if (piecesConfigs[i].pieceType == piece.Type)
            {
                return piecesConfigs[i].resources;
            }
        }
        return null;
    }

    private Piece GetPieceAtBoardPosition(Vector2 boardPosition)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].BoardPosition.x == boardPosition.x && pieces[i].BoardPosition.y == boardPosition.y)
            {
                return pieces[i];
            }
        }
        return null;
    }
}