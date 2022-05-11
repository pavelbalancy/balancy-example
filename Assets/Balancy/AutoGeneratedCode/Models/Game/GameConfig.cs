using Newtonsoft.Json;
using System;

namespace Balancy.Models.Game
{
#pragma warning disable 649

	public class GameConfig : BaseModel
	{

		[JsonProperty]
		private string unnyIdGoldItem;
		private Models.SmartObjects.Item goldItem;
		[JsonProperty]
		private string unnyIdGemsItem;
		private Models.SmartObjects.Item gemsItem;


		[JsonIgnore]
		public Models.SmartObjects.Item GoldItem
		{
			get
			{
				if (goldItem == null)
					goldItem = DataEditor.GetModelByUnnyId<Models.SmartObjects.Item>(unnyIdGoldItem);
				return goldItem;
			}
		}

		[JsonIgnore]
		public Models.SmartObjects.Item GemsItem
		{
			get
			{
				if (gemsItem == null)
					gemsItem = DataEditor.GetModelByUnnyId<Models.SmartObjects.Item>(unnyIdGemsItem);
				return gemsItem;
			}
		}

	}
#pragma warning restore 649
}