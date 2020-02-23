using UnityEngine;

namespace Miximum
{
	public class Singleton<T> : MonoBehaviour
	{
		//-----------------------------------------------------------------

		public static T Instance;
	
		//-----------------------------------------------------------------

		public virtual void Awake() => Instance    =   GetComponent<T>();

		//-----------------------------------------------------------------
	}
}