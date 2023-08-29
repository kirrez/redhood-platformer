using UnityEngine;

namespace Platformer
{
	public interface IView
	{
		void Enable();
		void Disable();

		void SetParent(Transform parent);
	}
}