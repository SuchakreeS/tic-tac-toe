﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Miximum;
using TMPro;
using UnityEngine.UI;

namespace TicTacToe
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class UIBoard : MonoBehaviour
    {
        [Serializable]
        public struct SymbolInfo
        {
            public SymbolType Type;
            public Texture2D Texture;
        }
        // -------------------------------------------------------------------------------------
        [Header("Player object")]
        [SerializeField] private RawImage m_Player1Symbol;
        [SerializeField] private RawImage m_Player2Symbol;
        [Header("Board object")]
        [SerializeField] private UICell m_UICellPrefab;
        [SerializeField] private List<SymbolInfo> m_SymbolInfo;
        [SerializeField] private RawImage m_WinSymbol;
        [SerializeField] private TextMeshProUGUI WaitNextGameText;
        // -------------------------------------------------------------------------------------
        private GridLayoutGroup _GridLayout;
        private Dictionary<SymbolType, SymbolInfo> _SymbolInfos = new Dictionary<SymbolType, SymbolInfo>();
        private Action<Position> _OnClickBoard;
        private int _Size;
        private UICell[,] _UICells;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        private void Awake()
        {
            _GridLayout = GetComponent<GridLayoutGroup>();
        }
        private void Start()
        {
            foreach (var info in m_SymbolInfo)
            {
                _SymbolInfos[info.Type] = info;
            }
            GameController.Instance.OnGameSelectedAsObservable().Subscribe(_info => SetCell(DataManager.GetPlayerInfo(_info.PlayerName).Symbol, _info.Position)).AddTo(this);
            GameController.Instance.OnUndoAsObservable().Subscribe(_info => SetCell(SymbolType.None, _info.Position)).AddTo(this);
            GameController.Instance.OnGameCompletedAsObservable().Subscribe(_wonPlayer => 
            {
                if(_wonPlayer != PlayerName.None)
                {
                    m_WinSymbol.color = Color.white;
                    m_WinSymbol.texture = _SymbolInfos[DataManager.GetPlayerInfo(_wonPlayer).Symbol].Texture;
                    WaitNextGameText.text = $"{_wonPlayer} Won!";
                }
                else
                {
                    m_WinSymbol.color = new Color(0f, 0f, 0f, 0f);
                    WaitNextGameText.text = $"Draw!";
                }
            }).AddTo(this);
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        public void SetSize(int _size) => _Size = _size;
        public void SetCell(SymbolType _type, Position _pos)
        {
            if(_pos.Row >= _Size || _pos.Column >= _Size) return;
                _UICells[_pos.Row, _pos.Column].SetSymbol(_SymbolInfos[_type].Texture);
            if(_type == SymbolType.None)
                _UICells[_pos.Row, _pos.Column].ClearSymbol();
        }
        public IObservable<Position> OnClickBoardAsObservable() => Observable.FromEvent<Position>
        (
            _action => _OnClickBoard += _action,
            _action => _OnClickBoard -= _action
        );
        public void Refresh(int _size = 3)
        {
            _Size = _size;
            foreach (var child in transform.GetComponentsInChildren<Transform>())
            {
                if(child == transform)
                    continue;
                Destroy(child.gameObject);
            }
            var sizeDelta = GetComponent<RectTransform>().sizeDelta;
            _GridLayout.cellSize = new Vector2(sizeDelta[0] / (float)_Size, sizeDelta[1] / (float)_Size);
            _UICells = new UICell[_Size, _Size];
            CreateCells();

            // Update Symbol
            m_Player1Symbol.texture = _SymbolInfos[DataManager.PlayersInfo[0].Symbol].Texture;
            m_Player2Symbol.texture = _SymbolInfos[DataManager.PlayersInfo[1].Symbol].Texture;
        }
        public void UpdatePlayerSymbol(PlayerName _playerName)
        {
            if(_playerName == PlayerName.Player1)
            {
                m_Player1Symbol.color = Color.white;
                m_Player2Symbol.color = Color.gray;
            }
            else
            {
                m_Player2Symbol.color = Color.white;
                m_Player1Symbol.color = Color.gray;
            }
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion
        private void CreateCells()
        {
            for (int i = 0; i < _Size; i++)
            {
                var indexI = i;
                for (int j = 0; j < _Size; j++)
                {
                    var indexJ = j;
                    var cell = Instantiate(m_UICellPrefab, transform);
                    cell.ClearSymbol();
                    cell.Button.OnClickAsObservable().Subscribe(_ => 
                    {
                        _OnClickBoard?.Invoke(new Position(indexI, indexJ));
                    }).AddTo(this);
                    _UICells[i, j] = cell;
                }
            }
        }

        // -------------------------------------------------------------------------------------
    }
}
