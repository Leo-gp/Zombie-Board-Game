using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public List<Resource> Resources { get; private set; }
    [field: SerializeField] public int MoveCost { get; private set; }
    [field: SerializeField] public int InteractionCost { get; private set; }
    [field: SerializeField] public int AttackCost { get; private set; }
    [field: SerializeField] public int FoodNeeds { get; private set; }
    
    public int MaxActionPoints { get; set; }
    public int ActionPoints { get; set; }
    public Piece Piece { get; set; }

    private Board board;

    private void Awake()
    {
        board = FindObjectOfType<Board>();    
    }

    public void MoveToPiece(Piece piece)
    {
        if (piece == Piece || ActionPoints - (MoveCost * board.GetDistanceBetweenPieces(Piece, piece)) < 0)
        {
            return;
        }
        transform.position = new Vector3(piece.transform.position.x, piece.transform.position.y, 0);
        ActionPoints -= MoveCost * board.GetDistanceBetweenPieces(Piece, piece);
        Piece = piece;
        HUDManager.Instance.OnPlayerMove();
    }

    public void InteractWithPiece(Piece piece)
    {
        if (piece.Resources == null
        || piece.Resources.Count == 0
        || piece != Piece
        || ActionPoints - InteractionCost < 0)
        {
            return;
        }
        
        foreach (var resource in piece.Resources)
        {
            AddResource(resource.resourceType, resource.ammount);
        }
        ActionPoints -= InteractionCost;
        HUDManager.Instance.OnPlayerInteraction();
    }

    public int GetResourceAmmount(ResourceTypeEnum resourceType)
    {
        var resource = GetResource(resourceType);
        return (resource != null) ? resource.ammount : 0;
    }

    public void ConsumeFood()
    {
        int foodAmmount = GetResourceAmmount(ResourceTypeEnum.Food);
        int newAmmount = foodAmmount - FoodNeeds;
        if (newAmmount >= 0)
        {
            AddResource(ResourceTypeEnum.Food, -FoodNeeds);
        }
        else
        {
            AddResource(ResourceTypeEnum.Food, -foodAmmount);
            TakeDamage(-newAmmount);
        }
    }

    public void AttackPlayer(Player target)
    {
        if (ActionPoints - AttackCost < 0)
        {
            return;
        }
        int damage = GameStateManager.Instance.RollDice();
        target.TakeDamage(damage);
        ActionPoints -= AttackCost;
        HUDManager.Instance.OnPlayerAttack();
    }

    private void TakeDamage(int ammount)
    {
        var health = GetResource(ResourceTypeEnum.Health);
        health.ammount -= ammount;
        if (health.ammount <= 0)
        {
            health.ammount = 0;
            Die();
        }
    }

    private void Die()
    {
        GameStateManager.Instance.OnPlayerDeath(this);
    }

    private Resource GetResource(ResourceTypeEnum resourceType)
    {
        for (int i = 0; i < Resources.Count; i++)
        {
            if (Resources[i].resourceType == resourceType)
            {
                return Resources[i];
            }
        }
        return null;
    }

    private void AddResource(ResourceTypeEnum resourceType, int ammount)
    {
        var resource = GetResource(resourceType);
        if (resource != null)
        {
            resource.ammount += ammount;
        }
    }
}