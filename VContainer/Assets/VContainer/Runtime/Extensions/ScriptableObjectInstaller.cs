using UnityEngine;
using VContainer.Unity;

namespace VContainer.Extensions
{
	public abstract class ScriptableObjectInstaller : ScriptableObject, IInstaller
	{
		public abstract void Install(IContainerBuilder builder);
	}
}