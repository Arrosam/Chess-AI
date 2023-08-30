using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Chess.Game
{
    public class GamePlayerManager : MonoBehaviour

    {
    public static GamePlayerManager Instance { get; private set; }
    PlayerType whitePlayerType;
    PlayerType blackPlayerType;
    public AISettings whiteAISettings;
    public AISettings blackAISettings;
    public GameSettings gameSettings;

    Player _whitePlayer;
    Player _blackPlayer;
    public Player PlayerToMove { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public GamePlayerManager(AISettings whiteAISettings, AISettings blackAISettings, GameSettings gameSettings)
    {
        this.whiteAISettings = whiteAISettings;
        this.blackAISettings = blackAISettings;
        this.gameSettings = gameSettings;
    }

    public void InitializePlayer()
    {
        PlayerFactory.LoadGameSettings(gameSettings);
        PlayerFactory.LoadAISettings(whiteAISettings);
        _whitePlayer = PlayerFactory.CreatePlayer(whitePlayerType);
        PlayerFactory.LoadAISettings(blackAISettings);
        _blackPlayer = PlayerFactory.CreatePlayer(blackPlayerType);
    }

    public void UpdatePlayerToMove()
    {
        PlayerToMove.Update();
    }

    public void UpdateCurrentPlayer(bool whitePlayerToMove)
    {
        PlayerToMove = whitePlayerToMove ? _whitePlayer : _blackPlayer;
        PlayerToMove.StartTurnPhase();
    }

    public void LoadPlayerType(PlayerType whitePlayerType, PlayerType blackPlayerType)
    {
        this.whitePlayerType = whitePlayerType;
        this.blackPlayerType = blackPlayerType;
    }

    public void TriggerAIAssistanceMode(bool isWhiteToSwitch)
    {
        HybridPlayer hybridPlayer = (HybridPlayer)(isWhiteToSwitch ? _whitePlayer : _blackPlayer);
        hybridPlayer.TriggerAIAssistanceSearch();
    }
    }
}