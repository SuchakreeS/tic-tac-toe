using System;
using UnityEngine;
using UnityEngine.UI;

namespace Miximum
{
	public static class ButtonExtension
	{
		//--------------------------------------------------------------------------------------------------------------

		public static bool SetInteractive(this Button _source, bool _interactAble)
		{
			return _source.interactable = _interactAble;
		}
	}
}