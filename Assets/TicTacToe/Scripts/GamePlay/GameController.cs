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
        private Action<ActionInfo> _OnSelected;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        private void Start()
        {
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        // Call this function for 
        public void GameReset()
        {
            // Load Data
            var playersInfo = DataManager.PlayersInfo;

            // Init Variable
            _GameStageList = new List<GameStage>();
            _PlayersName = new PlayerName[playersInfo.Length];
            _Players = new Player[playersInfo.Length];
            _BoardSize = DataManager.BoardSize;
            _TurnCount = 0;

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
            OnBoardOverAsObservable().Subscribe(_wonPlayer => 
            {
                Debug.Log($"_wonPlayer: {_wonPlayer}");
            }).AddTo(this);
        }
        public void GamePause()
        {
            // No need
        }
        public void GameContinue()
        {
            // No need
        }
        public IObservable<ActionInfo> OnSelectedAsObservable() => Observable.FromEvent<ActionInfo>
        (
            _action => _OnSelected += _action,
            _action => _OnSelected -= _action
        );
        // -------------------------------------------------------------------------------------
        // Private Funtion
        private IObservable<PlayerName> OnBoardOverAsObservable()
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
                            _observer.OnNext(GetGameStage().WonPlayer);
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
                        Debug.Log("#### MatchOver ####");
                        _observer.OnNext(true);
                        _observer.OnCompleted();
                    }
                    var disposable = player.MakeDicision(gameStage, m_GameTime).Subscribe(_position =>
                    {
                        gameStage.SelectPosition(_position);
                        _OnSelected?.Invoke(new ActionInfo(player.PlayerName, _position));
                        _TurnCount++;

                        var nextStage = new GameStage(_BoardSize, gameStage.BoardData, _PlayersName, GetPlayerName());
                        _GameStageList.Add(nextStage);

                        _CanStartNextTurn = true;
                        _observer.OnNext(false);
                        _observer.OnCompleted();
                    }).AddTo(this);
                    return Disposable.Create(() => disposable?.Dispose());
                }
            );
        }
        private Player GetPlayer() => _Players[(_TurnCount-1)%_Players.Length];
        private GameStage GetGameStage() => _GameStageList[_TurnCount-1];
        private PlayerName GetPlayerName() => _PlayersName[(_TurnCount-1)%_PlayersName.Length];
        // -------------------------------------------------------------------------------------
    }
}
