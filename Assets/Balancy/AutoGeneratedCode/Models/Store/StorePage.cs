using Newtonsoft.Json;
using System;

namespace Balancy.Models.Store
{
#pragma warning disable 649

	public class StorePage : BaseModel
	{



		[JsonProperty("systemName")]
		public readonly string SystemName;

		[JsonProperty("displayName")]
		public readonly string DisplayName;

		[JsonProperty("slots")]
		public readonly Models.Store.StoreSlot[] Slots;

	}
#pragma warning restore 649
}