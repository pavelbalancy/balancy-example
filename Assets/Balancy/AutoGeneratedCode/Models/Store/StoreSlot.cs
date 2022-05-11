using Newtonsoft.Json;
using System;

namespace Balancy.Models.Store
{
#pragma warning disable 649

	public class StoreSlot : BaseModel
	{

		[JsonProperty]
		private string unnyIdCondition;
		private Models.SmartObjects.Conditions.Logic condition;
		[JsonProperty]
		private string unnyIdStoreItem;
		private Models.SmartObjects.StoreItem storeItem;


		[JsonProperty("type")]
		public readonly Models.Store.StoreSlotType Type;

		[JsonIgnore]
		public Models.SmartObjects.Conditions.Logic Condition
		{
			get
			{
				if (condition == null)
					condition = DataEditor.GetModelByUnnyId<Models.SmartObjects.Conditions.Logic>(unnyIdCondition);
				return condition;
			}
		}

		[JsonIgnore]
		public Models.SmartObjects.StoreItem StoreItem
		{
			get
			{
				if (storeItem == null)
					storeItem = DataEditor.GetModelByUnnyId<Models.SmartObjects.StoreItem>(unnyIdStoreItem);
				return storeItem;
			}
		}

	}
#pragma warning restore 649
}