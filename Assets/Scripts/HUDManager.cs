using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI resourceTextPrefab;
    public GameObject resourcesParent;
    public TextMeshProUGUI actionPointsText;
    public Image turnPlayerImage;
    public GameObject playersInfoObj;
    public GameObject playersInfoPanelPrefab;
    public TextMeshProUGUI resourceTextShortPrefab;
    public GameObject gameOverPanel;

    public static HUDManager Instance { get; private set; }

    private List<TextMeshProUGUI> resourceTexts;
    private List<GameObject> playersInfoPanels;

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
        if (FindObjectOfType<GameStateManager>().initialNumberOfPlayers < 2)
        {
            turnPlayerImage.GetComponentInParent<TextMeshProUGUI>().gameObject.SetActive(false);
        }
        resourceTexts = new List<TextMeshProUGUI>();
        playersInfoPanels = new List<GameObject>();
    }

    public void OnInitiatePlayerTurn()
    {
        SetActionPointsText();
        SetTurnPlayerImage();
        UpdateResourceTexts();
        SetPlayersInfo();
    }

    public void OnPlayerMove()
    {
        SetActionPointsText();
    }

    public void OnPlayerInteraction()
    {
        SetActionPointsText();
        UpdateResourceTexts();
        SetPlayersInfo();
    }

    public void OnPlayerAttack()
    {
        SetActionPointsText();
        UpdateResourceTexts();
        SetPlayersInfo();
    }

    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        var winner = GameStateManager.Instance.Players[0];
        var playerWonText = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
        playerWonText.text = winner.name.ToUpper() + " WON!";
    }

    public void EndTurnButton()
    {
        TurnManager.Instance.EndTurn();
    }

    public void RematchButton()
    {
        GameStateManager.Instance.Rematch();
    }

    private void SetActionPointsText()
    {
        var ap = TurnManager.Instance.TurnPlayer.ActionPoints.ToString();
        var maxAp = TurnManager.Instance.TurnPlayer.MaxActionPoints.ToString();
        actionPointsText.text = "Action Points: " + ap + "/" + maxAp;
    }

    private void SetTurnPlayerImage()
    {
        turnPlayerImage.color = TurnManager.Instance.TurnPlayer.GetComponent<SpriteRenderer>().color;
    }

    private void SetPlayersInfo()
    {
        foreach (var panel in playersInfoPanels)
        {
            Destroy(panel);
        }
        playersInfoPanels.Clear();
        foreach (var player in GameStateManager.Instance.Players)
        {
            if (player == TurnManager.Instance.TurnPlayer)
            {
                continue;
            }
            var panel = Instantiate(playersInfoPanelPrefab, playersInfoObj.transform);
            var playerImg = panel.GetComponentsInChildren<Image>().Where(e => e.gameObject != panel.gameObject).First();
            playerImg.color = player.GetComponent<SpriteRenderer>().color;
            foreach (var resource in player.Resources)
            {
                var resourceText = Instantiate(resourceTextShortPrefab, panel.GetComponentInChildren<GridLayoutGroup>().transform);
                switch (resource.resourceType)
                {
                    case ResourceTypeEnum.Health:
                        resourceText.text = "H: ";
                        break;
                    case ResourceTypeEnum.Food:
                        resourceText.text = "F: ";
                        break;
                    case ResourceTypeEnum.Material:
                        resourceText.text = "M: ";
                        break;
                    case ResourceTypeEnum.Ammo:
                        resourceText.text = "A: ";
                        break;
                    case ResourceTypeEnum.Knowledge:
                        resourceText.text = "K: ";
                        break;
                }
                resourceText.text += resource.ammount.ToString();
            }
            playersInfoPanels.Add(panel);
        }
    }

    private void UpdateResourceTexts()
    {
        foreach (var resourceText in resourceTexts)
        {
            Destroy(resourceText.gameObject);
        }
        resourceTexts.Clear();
        var turnPlayer = TurnManager.Instance.TurnPlayer;
        foreach (var resource in turnPlayer.Resources)
        {
            var resourceText = Instantiate(resourceTextPrefab, resourcesParent.transform);
            resourceText.text = resource.resourceType.ToString() + ": " + turnPlayer.GetResourceAmmount(resource.resourceType).ToString();
            resourceTexts.Add(resourceText);
        }
    }
}