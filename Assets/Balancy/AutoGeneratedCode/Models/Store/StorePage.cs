using Newtonsoft.Json;
using System;

namespace Balancy.Models.Store
{
#pragma warning disable 649

	public class StorePage : BaseModel
	{



		[JsonProperty("systemName")]
		public readonly string SystemName;

		[JsonProperty("slots")]
		public readonly Models.Store.StoreSlot[] Slots;

		[JsonProperty("displayName")]
		public readonly string DisplayName;

	}
#pragma warning restore 649
}