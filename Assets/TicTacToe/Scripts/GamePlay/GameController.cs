using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Miximum;

namespace TicTacToe
{
    public enum GameTime
    {
        Infinity = 999999,
        FiveSeconds = 5,
        TenSeconds = 10,
        ThirtySeconds = 30,
    }
    public struct ActionInfo
    {
        public PlayerName PlayerName;
        public Position Position;

        public ActionInfo(PlayerName _playerName, Position _position)
        {
            PlayerName = _playerName;
            Position = _position;
        }
    }
    public class GameController : Singleton<GameController>
    {
        // -------------------------------------------------------------------------------------
        [SerializeField] private UIGamePlay m_UIGamePlay;
        [SerializeField] private UIBoard m_UIBoard;
        [SerializeField] private GameTime m_GameTime;
        // -------------------------------------------------------------------------------------
        private List<GameStage> _GameStageList;
        private Player[] _Players;
        private bool _Status;
        private int _TurnCount;
        private PlayerName _CurrentTurn;
        private Position _Selected;
        private BoardSize _BoardSize;
        private PlayerName[] _PlayersName;
        private IDisposable _Disposable;
        private bool _CanStartNextTurn;
        // -------------------------------------------------------------------------------------
        private Action<ActionInfo> _OnUndo;
        private Action<ActionInfo> _OnGameSelected;
        private Action<PlayerName> _OnGameCompleted;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        private void Start()
        {
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        public void GameReset(bool _continue = false)
        {
            // Load Data
            var playersInfo = DataManager.PlayersInfo;

            // Init Variable
            _GameStageList = new List<GameStage>();
            _BoardSize = DataManager.BoardSize;
            _TurnCount = 0;

            if(!_continue)
            {
                _PlayersName = new PlayerName[playersInfo.Length];
                _Players = new Player[playersInfo.Length];
                for (int i = 0; i < playersInfo.Length; i++)
                {
                    if(playersInfo[i].Controller == ControllerType.Human)
                    {
                        _Players[i] = new HumanPlayer(playersInfo[i].Name, playersInfo[i].Symbol, m_UIBoard);
                    }
                    else if(playersInfo[i].Controller == ControllerType.AI)
                    {
                        _Players[i] = new AIPlayer(playersInfo[i].Name, playersInfo[i].Symbol);
                    }
                    _PlayersName[i] = playersInfo[i].Name;
                }
            }
        }
        public void GameStart()
        {
            // Create Stage
            var boardData = new int[(int)_BoardSize, (int)_BoardSize];
            _TurnCount++;
            var stage = new GameStage(_BoardSize, boardData, _PlayersName, GetPlayerName());
            _GameStageList.Add(stage);
            _Status = true;
            _Disposable?.Dispose();
            _Disposable = OnTurnProcessAsObservable().Subscribe().AddTo(this);
            OnGameCompleted().Subscribe(_wonPlayer => {}).AddTo(this);
        }
        public void GamePause()
        {
            // No need
        }
        public void GameContinue()
        {
            // No need
        }
        public void Undo()
        {
            if(_TurnCount > 1)
            {
                _TurnCount--;
                var stage = GetGameStage();
                var ActionInfo = new ActionInfo(stage.CurrentTurn, stage.Selected);
                stage.PrintStage();
                Debug.Log(stage.Undo());
                stage.PrintStage();
                _OnUndo?.Invoke(ActionInfo);
                _Disposable?.Dispose();
            }
        }
        public IObservable<ActionInfo> OnUndoAsObservable() => Observable.FromEvent<ActionInfo>
        (
            _action => _OnUndo += _action,
            _action => _OnUndo -= _action
        );
        public IObservable<ActionInfo> OnGameSelectedAsObservable() => Observable.FromEvent<ActionInfo>
        (
            _action => _OnGameSelected += _action,
            _action => _OnGameSelected -= _action
        );
        public IObservable<PlayerName> OnGameCompletedAsObservable() => Observable.FromEvent<PlayerName>
        (
            _action => _OnGameCompleted += _action,
            _action => _OnGameCompleted -= _action
        );
        public void SwitchPlayers()
        {
            var players = new Player[_Players.Length];
            var playerNames = new PlayerName[_PlayersName.Length];

            for (int i = 0; i < _Players.Length; i++)
            {
                players[i] = _Players[(i+1)%_Players.Length];
            }
            for (int i = 0; i < _PlayersName.Length; i++)
            {
                Debug.Log($"i: {i}: {_PlayersName[(i+1)%_PlayersName.Length]}");
                playerNames[i] = _PlayersName[(i+1)%_PlayersName.Length];
                Debug.Log($"playerNames[{i}] =  {playerNames[i]}");
            }
            _Players = players;
            _PlayersName = playerNames;
            Debug.Log($"_Players[0] =  {_Players[0].PlayerName}, _Players[1] =  {_Players[1].PlayerName}");
            Debug.Log($"playerNames[0] =  {_PlayersName[0]}, playerNames[1] =  {_PlayersName[1]}");
        }
        public Player GetPlayer(PlayerName _playerName)
        {
            foreach (var player in _Players)
            {
                if(player.PlayerName == _playerName)
                {
                    return player;
                }
            }
            return null;
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion
        private IObservable<PlayerName> OnGameCompleted()
        {
            return Observable.Create<PlayerName>
            (
                _observer => 
                {
                    var disposable = Observable.EveryUpdate().Where(_ => _CanStartNextTurn).Subscribe(_ => 
                    {
                        _Disposable?.Dispose();
                        _Disposable = OnTurnProcessAsObservable().Where(_value => _value).Subscribe(_boardOver => 
                        {
                            var wonPlayer = GetGameStage().WonPlayer;
                            AddScore(wonPlayer);
                            _OnGameCompleted?.Invoke(wonPlayer);
                            _observer.OnNext(wonPlayer);
                            _observer.OnCompleted();
                        }).AddTo(this);
                    }).AddTo(this);
                    return Disposable.Create(() => disposable?.Dispose());
                }
            );
        }
        private IObservable<bool> OnTurnProcessAsObservable()
        {
            return Observable.Create<bool>
            (
                _observer => 
                {
                    _CanStartNextTurn = false;
                    var gameStage = GetGameStage();
                    var player = GetPlayer();
                    if(gameStage.Status == StageStatus.MatchOver)
                    {
                        _observer.OnNext(true);
                        _observer.OnCompleted();
                    }
                    var disposable = player.MakeDicision(gameStage, m_GameTime).Subscribe(_position =>
                    {
                        if(_position == Position.None)
                            _position = gameStage.RandomPosition();
                        gameStage.SelectPosition(_position);
                        _OnGameSelected?.Invoke(new ActionInfo(player.PlayerName, _position));
                        _TurnCount++;
                        
                        var nextStage = new GameStage(_BoardSize, gameStage.BoardData, _PlayersName, GetPlayerName());
                        while(_GameStageList.Count >= _TurnCount)
                        {
                            _GameStageList.RemoveAt(_GameStageList.Count - 1);
                        }
                        _GameStageList.Add(nextStage);

                        _observer.OnNext(false);
                        _observer.OnCompleted();
                    }).AddTo(this);
                    return Disposable.Create(() => 
                    {
                        _CanStartNextTurn = true;
                        disposable?.Dispose();
                    });
                }
            );
        }
        private Player GetPlayer() => _Players[(_TurnCount-1)%_Players.Length];
        private GameStage GetGameStage() => _GameStageList[_TurnCount-1];
        private PlayerName GetPlayerName() => _PlayersName[(_TurnCount-1)%_PlayersName.Length];
        private void AddScore(PlayerName _playerName)
        {
            foreach (var player in _Players)
            {
                if(player.PlayerName == _playerName)
                {
                    player.Score ++;
                    Debug.Log($"{player.PlayerName}: {player.Score}");
                    break;
                }
            }
        }
        // -------------------------------------------------------------------------------------
    }
}
