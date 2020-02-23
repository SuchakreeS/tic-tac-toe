using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Miximum;

namespace TicTacToe
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UICanvas : MonoBehaviour
    {
        // -------------------------------------------------------------------------------------
        private CanvasGroup _Canvas;
        // -------------------------------------------------------------------------------------
        public CanvasGroup Canvas => _Canvas;
        // -------------------------------------------------------------------------------------
        // Unity Funtion
        private void Awake()
        {
            _Canvas = GetComponent<CanvasGroup>();
        }
        // -------------------------------------------------------------------------------------
        // Public Funtion
        public abstract void InitCanvas();
        // -------------------------------------------------------------------------------------
        // Private Funtion

        // -------------------------------------------------------------------------------------
    }
}
