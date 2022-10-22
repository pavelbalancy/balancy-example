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
			ParseDictionary<Models.SmartObjects.Conditions.And>();
			ParseDictionary<Models.SmartObjects.Conditions.Or>();
			ParseDictionary<Models.SmartObjects.Conditions.DatesRage>();
			ParseDictionary<Models.SmartObjects.Conditions.DayOfTheWeek>();
			ParseDictionary<Models.SmartObjects.Conditions.TimeOfTheDay>();
			ParseDictionary<Models.SmartObjects.Conditions.ActiveEvent>();
			ParseDictionary<Models.SmartObjects.Conditions.ProfileFieldInRange>();
			ParseDictionary<Models.SmartObjects.Conditions.SegmentCondition>();
			ParseDictionary<Models.SmartObjects.Conditions.ABTestCondition>();
			ParseDictionary<Models.SmartObjects.Conditions.TimeRage>();
			ParseDictionary<Models.SmartObjects.Conditions.StoreItemWasPurchased>();
			ParseDictionary<Models.SmartObjects.Conditions.AppVersion>();
			ParseDictionary<Models.SmartObjects.Conditions.EngineVersion>();
			ParseDictionary<Models.SmartObjects.Conditions.ProfileFieldNumber>();
			ParseDictionary<Models.SmartObjects.Conditions.ProfileFieldString>();
			ParseDictionary<Models.SmartObjects.Conditions.ProfileFieldBool>();
			ParseDictionary<Models.SmartObjects.Conditions.Platform>();
			ParseDictionary<Models.SmartObjects.Conditions.SystemLanguage>();
			ParseDictionary<Models.SmartObjects.Conditions.Not>();
			Game.Init();
			SmartStorage.SetLoadSmartObjectMethod(LoadSmartObject);
		}
	}
#pragma warning restore 649
}