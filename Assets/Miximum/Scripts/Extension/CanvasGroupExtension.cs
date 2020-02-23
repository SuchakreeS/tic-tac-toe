using System;
using UnityEngine;

namespace Miximum
{
	public static class CanvasGroupExtension
	{
		//--------------------------------------------------------------------------------------------------------------
		
		public static IDisposable LerpAlpha(this CanvasGroup _source, int _milliseconds, float _target, bool _adjustInteractAble = true, Action _onComplete = null)
		{
			var progress = 0f;
			var current = _source.alpha;
			var different = _target - current;
			return LerpThread
				.Execute
				(
					_milliseconds,
					_count =>
					{
						progress += Time.deltaTime / (_milliseconds * GlobalConstant.MILLISECONDS_TO_SECONDS);
						_source.SetAlpha(Mathf.Clamp01(current + progress * different), _adjustInteractAble);
					},
					() =>
					{
						_source.SetAlpha(Mathf.Clamp01(_target), _adjustInteractAble);
						_onComplete?.Invoke();
					}
				);
		}
		public static IDisposable LerpAlpha(this CanvasGroup _source, Easing.Ease _easing, int _milliseconds, float _target, bool _adjustInteractAble = true, Action _onComplete = null)
		{
			var progress = 0f;
			var current = _source.alpha;
			var different = _target - current;
			
			return LerpThread
				.Execute
				(
					_milliseconds,
					_count =>
					{
						progress += Time.deltaTime / (_milliseconds * GlobalConstant.MILLISECONDS_TO_SECONDS);
						var easingProgress = EasingVector.EaseingFloat(_easing, current, _target, progress);
						_source.SetAlpha(easingProgress, _adjustInteractAble);
					},
					() =>
					{
						_source.SetAlpha(Mathf.Clamp01(_target), _adjustInteractAble);
						_onComplete?.Invoke();
					}
				);
		}
		
		//--------------------------------------------------------------------------------------------------------------

		public static bool SetInteractive(this CanvasGroup _source, bool _interactAble)
		{
			return _source.interactable = _source.blocksRaycasts = _interactAble;
		}
		
		//--------------------------------------------------------------------------------------------------------------

		public static CanvasGroup SetAlpha(this CanvasGroup _source, float _alpha, bool _adjustInteractAble = true)
		{
			if(_source == null)	 return null;
			_source.alpha = _alpha;
			if (_adjustInteractAble)
				_source.SetInteractive(_alpha >= GlobalConstant.ALPHA_VALUE_VISIBLE);
			return _source;
		}
		
		//--------------------------------------------------------------------------------------------------------------
	}
}