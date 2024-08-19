using UnityEngine;
using VContainer.Unity;

namespace VContainer.Extensions
{
	public abstract class MonoInstaller : MonoBehaviour, IInstaller
	{
		public abstract void Install(IContainerBuilder builder);
	}
}