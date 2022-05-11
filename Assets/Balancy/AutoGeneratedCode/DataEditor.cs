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

		public static List<Models.Game.GameItem> GameItems { get; private set; }
		public static List<Models.SmartObjects.StoreItem> StoreItems { get; private set; }
		public static List<Models.SmartObjects.Item> Items { get; private set; }
		public static List<Models.Store.StorePage> StorePages { get; private set; }
		public static List<Models.SmartObjects.SegmentOption> SegmentOptions { get; private set; }
		public static List<Models.Store.StoreConfig> StoreConfigs { get; private set; }
		public static Models.Game.GameConfig GameConfig { get; private set; }

		static partial void PrepareGeneratedData() {
			var gameItemWrapper = ParseDictionary<Models.Game.GameItem>();
			if (gameItemWrapper == null || gameItemWrapper.List == null)
				GameItems = new List<Models.Game.GameItem>(0);
			else {
				GameItems = new List<Models.Game.GameItem>(gameItemWrapper.List.Length);
				foreach (var child in gameItemWrapper.List)
					GameItems.Add(child);
			}

			ParseDictionary<Models.SmartObjects.Conditions.And>();

			ParseDictionary<Models.SmartObjects.Conditions.Or>();

			ParseDictionary<Models.SmartObjects.Conditions.DatesRage>();

			ParseDictionary<Models.SmartObjects.Conditions.DayOfTheWeek>();

			ParseDictionary<Models.SmartObjects.Conditions.TimeOfTheDay>();


			var storeItemWrapper = ParseDictionary<Models.SmartObjects.StoreItem>();
			if (storeItemWrapper == null || storeItemWrapper.List == null)
				StoreItems = new List<Models.SmartObjects.StoreItem>(0);
			else {
				StoreItems = new List<Models.SmartObjects.StoreItem>(storeItemWrapper.List.Length);
				foreach (var child in storeItemWrapper.List)
					StoreItems.Add(child);
			}


			var itemWrapper = ParseDictionary<Models.SmartObjects.Item>();
			if (itemWrapper == null || itemWrapper.List == null)
				Items = new List<Models.SmartObjects.Item>(0);
			else {
				Items = new List<Models.SmartObjects.Item>(itemWrapper.List.Length);
				foreach (var child in itemWrapper.List)
					Items.Add(child);
			}


			ParseDictionary<Models.SmartObjects.Conditions.ActiveEvent>();

			var storePageWrapper = ParseDictionary<Models.Store.StorePage>();
			if (storePageWrapper == null || storePageWrapper.List == null)
				StorePages = new List<Models.Store.StorePage>(0);
			else {
				StorePages = new List<Models.Store.StorePage>(storePageWrapper.List.Length);
				foreach (var child in storePageWrapper.List)
					StorePages.Add(child);
			}


			var segmentOptionWrapper = ParseDictionary<Models.SmartObjects.SegmentOption>();
			if (segmentOptionWrapper == null || segmentOptionWrapper.List == null)
				SegmentOptions = new List<Models.SmartObjects.SegmentOption>(0);
			else {
				SegmentOptions = new List<Models.SmartObjects.SegmentOption>(segmentOptionWrapper.List.Length);
				foreach (var child in segmentOptionWrapper.List)
					SegmentOptions.Add(child);
			}

			ParseDictionary<Models.SmartObjects.Conditions.ProfileFieldEquals>();

			ParseDictionary<Models.SmartObjects.Conditions.ProfileFieldInRange>();

			ParseDictionary<Models.SmartObjects.Conditions.ProfileFieldGreaterOrEqual>();

			ParseDictionary<Models.SmartObjects.Conditions.SegmentCondition>();

			ParseDictionary<Models.SmartObjects.Conditions.ProfileFieldLower>();

			ParseDictionary<Models.SmartObjects.Conditions.ABTestCondition>();

			var storeConfigWrapper = ParseDictionary<Models.Store.StoreConfig>();
			if (storeConfigWrapper == null || storeConfigWrapper.List == null)
				StoreConfigs = new List<Models.Store.StoreConfig>(0);
			else {
				StoreConfigs = new List<Models.Store.StoreConfig>(storeConfigWrapper.List.Length);
				foreach (var child in storeConfigWrapper.List)
					StoreConfigs.Add(child);
			}

			ParseDictionary<Models.SmartObjects.Conditions.Not>();

			var gameConfigWrapper = ParseDictionary<Models.Game.GameConfig>();
			if (gameConfigWrapper != null && gameConfigWrapper.List != null && gameConfigWrapper.List.Length > 0 && gameConfigWrapper.Config != null)
			{
				for (int i = 0; i < gameConfigWrapper.List.Length; i++)
				{
					if (gameConfigWrapper.List[i].UnnyId == gameConfigWrapper.Config.Selected)
					{
						GameConfig = gameConfigWrapper.List[i];
						break;
					}
				}
			}
			else
				GameConfig = null;

			SmartStorage.SetLoadSmartObjectMethod(LoadSmartObject);
		}
	}
#pragma warning restore 649
}