﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Miximum;

namespace TicTacToe
{
    public class UIController : MonoBehaviour
    {
        // -------------------------------------------------------------------------------------
        private const int DURATION_FADE_TRANSITION = 300;
        // -------------------------------------------------------------------------------------
        public enum UIName 
        {
            None, 
            Menu, 
            GameInit, 
            GamePlay
        }

        [Serializable]
        public struct CanvasInfo
        {
            public UIName Name;
            public CanvasGroup Canvas;
        }
        // -------------------------------------------------------------------------------------
        [Header("Canvas Setting")]
        [SerializeField] private CanvasInfo[] m_CanvasInfos;
        [SerializeField] private UIName m_StartUI;
        // -------------------------------------------------------------------------------------
        private bool _IsChanging;
        private UIName _CurrentUI;
        private Dictionary<UIName, CanvasGroup> _CanvanInfoDict = new Dictionary<UIName, CanvasGroup>();
        private IDisposable _Disposable;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        private void Awake()
        {
            foreach (var info in m_CanvasInfos)
            {
                _CanvanInfoDict[info.Name] = info.Canvas;
            }
        }

        private void Start()
        {
            // Set up UI
            HideAll();
            ChangeUI(m_StartUI, true);
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        public void ChangeUI(UIName _name, bool _force = false, Action _onCompleted = null)
        {
            if(_CurrentUI == _name || _IsChanging) return;

            // Set Event
            Action onCompleted = () => 
            {
                _IsChanging = false;
                _onCompleted?.Invoke();
            };
            Action showEvent = () => 
            {
                _CurrentUI = _name;
                Show(_CurrentUI, _force, onCompleted);
            };

            // Process
            _IsChanging = true;
            if(_CurrentUI != UIName.None)
            {
                Hide(_CurrentUI, _force, showEvent);
            }
            else
            {
                showEvent?.Invoke();
            }
        }
        public void ChangeUIMenu() => ChangeUI(UIName.Menu);
        public void ChangeUIGameInit() => ChangeUI(UIName.GameInit);
        public void ChangeUIGamePlay() => ChangeUI(UIName.GamePlay);
        // -------------------------------------------------------------------------------------
        // Private Funtion
        private void Hide(UIName _name, bool _force, Action _onCompleted = null) => FadeUI(_name, _force, _onCompleted, GlobalConstant.ALPHA_VALUE_INVISIBLE);
        private void Show(UIName _name, bool _force, Action _onCompleted = null) => FadeUI(_name, _force, _onCompleted, GlobalConstant.ALPHA_VALUE_VISIBLE);
        private void FadeUI(UIName _name, bool _force, Action _onCompleted, float _alpha)
        {
            if(_force)
            {
                _CanvanInfoDict[_name].SetAlpha(_alpha);
                _onCompleted?.Invoke();
            }
            else
            {
                _Disposable?.Dispose();
                _Disposable = _CanvanInfoDict[_name].LerpAlpha(DURATION_FADE_TRANSITION, _alpha, true, _onCompleted);
            }
        }
        private void HideAll(bool _force = true)
        {
            foreach (var name in _CanvanInfoDict.Keys)
            {
                Hide(name, _force);
            }
        }
        // -------------------------------------------------------------------------------------
    }
}