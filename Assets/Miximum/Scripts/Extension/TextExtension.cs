using System;
using UnityEngine;
using UnityEngine.UI;

namespace Miximum
{
	public static class TextExtension
	{
		//--------------------------------------------------------------------------------------------------------------
		
		public static IDisposable LerpAlpha(this Text _source, int _milliseconds, float _target)
		{
			var progress = 0f;
			var current = _source.color.a;
			var different = _target - current;
			
			return LerpThread
				.Execute
				(
					_milliseconds,
					_count =>
					{
						progress += Time.deltaTime / (_milliseconds * GlobalConstant.MILLISECONDS_TO_SECONDS);
						_source.SetAlpha(Mathf.Clamp01(current + progress * different));
					},
					() => _source.SetAlpha(Mathf.Clamp01(_target))
				);
		}
		
		//--------------------------------------------------------------------------------------------------------------

		public static Text SetAlpha(this Text _source, float _alpha)
		{
			var color = _source.color;
			color	=	new Color
			(
				color.r,
				color.g,
				color.b,
				_alpha
			);
			_source.color = color;
			return _source;
		}
		
		//--------------------------------------------------------------------------------------------------------------
	}
}