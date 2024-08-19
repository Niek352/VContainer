using UnityEngine;

namespace VContainer.Extensions
{
	[CreateAssetMenu(menuName = "VContainer/GenericScriptableInstaller", fileName = "GenericScriptableInstaller")]
	public class GenericScriptableInstaller : ScriptableObjectInstaller
	{
		[SerializeField] private ScriptableObject[] ScriptableObjects;
		
		public override void Install(IContainerBuilder builder)
		{
			foreach (var installer in ScriptableObjects)
			{
				builder.RegisterInstance(installer).AsImplementedInterfaces();
			}
		}
	}
}