using UnityEngine;

namespace Miximum
{
	public class EasingVector
	{
		//--------------------------------------------------------------------------------------------------------------

		public static float EaseingFloat(Easing.Ease _ease, float _start, float _end, float _progress)
		{
			return  Easing
				.GetEasingFunction(_ease)
				.Invoke
					(
						_start,
						_end,
						_progress
					);
		}
		
		//--------------------------------------------------------------------------------------------------------------

		public static Vector3 EaseTypeVector(Easing.Ease _ease, Vector3 _start, Vector3 _end, float _process)
		{
			return new Vector3
				(
					EaseingFloat
					(
						_ease,
						_start.x,
						_end.x,
						_process
					),
					EaseingFloat
					(
						_ease,
						_start.y,
						_end.y,
						_process
					),
					EaseingFloat
					(
						_ease,
						_start.z,
						_end.z,
						_process
					)
				);
		}
		
		//--------------------------------------------------------------------------------------------------------------

		public static Vector3 EaseInSineVector(Vector3 _start, Vector3 _end, float _process)
		{
			return new Vector3
			(
				Easing.EaseInSine
				(
					_start.x,
					_end.x,
					_process
				),
				Easing.EaseInSine
				(
					_start.y,
					_end.y,
					_process
				),
				Easing.EaseInSine
				(
					_start.z,
					_end.z,
					_process
				)
			);
		}
		//--------------------------------------------------------------------------------------------------------------

		public static Vector3 EaseOutQuartVector(Vector3 _start, Vector3 _end, float _process)
		{
			return new Vector3
			(
				Easing.EaseOutQuart
				(
					_start.x,
					_end.x,
					_process
				),
				Easing.EaseOutQuart
				(
					_start.y,
					_end.y,
					_process
				),
				Easing.EaseOutQuart
				(
					_start.z,
					_end.z,
					_process
				)
			);
		}
		
		//--------------------------------------------------------------------------------------------------------------
		
		public static Vector3 EaseOutBounceVector(Vector3 _start, Vector3 _end, float _process)
		{
			return new Vector3
			(
				Easing.EaseOutBounce
				(
					_start.x,
					_end.x,
					_process
				),
				Easing.EaseOutBounce
				(
					_start.y,
					_end.y,
					_process
				),
				Easing.EaseOutBounce
				(
					_start.z,
					_end.z,
					_process
				)
			);
		}
		
		//--------------------------------------------------------------------------------------------------------------
		
		public static Vector3 EaseOutElasticVector(Vector3 _start, Vector3 _end, float _process)
		{
			return new Vector3
			(
				Easing.EaseOutElastic
				(
					_start.x,
					_end.x,
					_process
				),
				Easing.EaseOutElastic
				(
					_start.y,
					_end.y,
					_process
				),
				Easing.EaseOutElastic
				(
					_start.z,
					_end.z,
					_process
				)
			);
		}
		
		//--------------------------------------------------------------------------------------------------------------
	}
}