using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Miximum;
using UnityEngine.UI;

namespace TicTacToe
{
    public class UICell : MonoBehaviour
    {
        // -------------------------------------------------------------------------------------
        [SerializeField] private RawImage m_SymbolImage;
        // -------------------------------------------------------------------------------------
        // Unity Funtion

        // -------------------------------------------------------------------------------------
        // Public Funtion
        public void SetSymbol(Texture2D _texture)
        {
            m_SymbolImage.color = Color.white;
            m_SymbolImage.texture = _texture;
        }
        public void ClearSymbol()
        {
            m_SymbolImage.color = new Color(0f ,0f, 0f, 0f);
        }
        // -------------------------------------------------------------------------------------
        // Private Funtion

        // -------------------------------------------------------------------------------------
    }
}
