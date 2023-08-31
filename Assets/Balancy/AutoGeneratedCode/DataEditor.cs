using System;
using Balancy.Data;
using System.Collections.Generic;

namespace Balancy
{
#pragma warning disable 649

	public partial class DataEditor
	{

		private static void LoadSmartObject(string userId, string name, string key, Action<ParentBaseData> callback)
		{
			switch (name)
			{
				case "DefaultProfile":
				{
					SmartStorage.LoadSmartObject<Data.DefaultProfile>(userId, key, responseData =>
					{
						callback?.Invoke(responseData.Data);
					});
					break;
				}
				case "SmartObjects.UnnyProfile":
				{
					SmartStorage.LoadSmartObject<Data.SmartObjects.UnnyProfile>(userId, key, responseData =>
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

		static partial void MoveAllData(string userId)
		{
			MigrateSmartObject(userId, "DefaultProfile");
			MigrateSmartObject(userId, "UnnyProfile");
		}

		static partial void TransferAllSmartObjectsFromLocalToCloud(string userId)
		{
			TransferSmartObjectFromLocalToCloud<Data.DefaultProfile>(userId);
			TransferSmartObjectFromLocalToCloud<Data.SmartObjects.UnnyProfile>(userId);
		}

		static partial void ResetAllSmartObjects(string userId)
		{
			ResetSmartObject<Data.DefaultProfile>(userId);
			ResetSmartObject<Data.SmartObjects.UnnyProfile>(userId);
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