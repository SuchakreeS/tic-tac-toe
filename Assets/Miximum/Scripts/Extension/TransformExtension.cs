using UnityEngine;

namespace Miximum
{
	public static class TransformExtension
	{
		//--------------------------------------------------------------------------------------------------------------
		public struct TransformValue
		{
			public Vector3 position;
			public Quaternion rotation;
			public Vector3 scale;
			public TransformValue(Vector3 _position, Quaternion _rotation, Vector3 _scale)
			{
				position = _position;
				rotation = _rotation;
				scale = _scale;
			}
		}
		
		public static Vector3 OrbirAroundPivot(Vector3 _point, Vector3 _pivot, Quaternion _angle) 
		{
			return _angle * ( _point - _pivot) + _pivot;
		}
		
		//--------------------------------------------------------------------------------------------------------------
		
		public static Vector3 OrbirAroundPivot(this Transform _source, Vector3 _pivot, Quaternion _angle) 
		{
			return _source.localPosition	=	OrbirAroundPivot
				(
					_source.localPosition,
					_pivot,
					_angle
				);
		}
		//--------------------------------------------------------------------------------------------------------------
	}
}