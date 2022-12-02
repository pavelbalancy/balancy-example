using System;
using Balancy.Data;
using System.Collections.Generic;

namespace Balancy
{
#pragma warning disable 649

	public partial class DataEditor
	{

		private static void LoadSmartObject(string name, string key, Action<ParentBaseData> callback)
		{
			switch (name)
			{
				case "DefaultProfile":
				{
					SmartStorage.LoadSmartObject<Data.DefaultProfile>(key, responseData =>
					{
						callback?.Invoke(responseData.Data);
					});
					break;
				}
				case "SmartObjects.UnnyProfile":
				{
					SmartStorage.LoadSmartObject<Data.SmartObjects.UnnyProfile>(key, responseData =>
					{
						callback?.Invoke(responseData.Data);
					});
					break;
				}
				default:
					UnnyLogger.Critical("No SmartObject found by name " + name);
					break;
			}
		}

		public static class Game
		{
			public static List<Models.Game.GameItem> GameItems { get; private set; }
			public static Models.Game.GameConfig GameConfig { get; private set; }

			public static void Init()
			{
				GameItems = DataManager.ParseList<Models.Game.GameItem>();
				GameConfig = DataManager.ParseSingleton<Models.Game.GameConfig>();
			}
		}

		static partial void PrepareGeneratedData() {
			Game.Init();
			SmartStorage.SetLoadSmartObjectMethod(LoadSmartObject);
		}
	}
#pragma warning restore 649
}