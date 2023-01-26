using Newtonsoft.Json;
using System;

namespace Balancy.Models.Game
{
#pragma warning disable 649

	public class GameConfig : BaseModel
	{

		[JsonProperty]
		private string unnyIdGoldItem;
		[JsonProperty]
		private string unnyIdGemsItem;


		[JsonIgnore]
		public Models.SmartObjects.Item GoldItem => DataEditor.GetModelByUnnyId<Models.SmartObjects.Item>(unnyIdGoldItem);

		[JsonIgnore]
		public Models.SmartObjects.Item GemsItem => DataEditor.GetModelByUnnyId<Models.SmartObjects.Item>(unnyIdGemsItem);

	}
#pragma warning restore 649
}