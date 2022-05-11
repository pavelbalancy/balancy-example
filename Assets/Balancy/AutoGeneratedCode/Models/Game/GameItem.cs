using Newtonsoft.Json;
using System;

namespace Balancy.Models.Game
{
#pragma warning disable 649

	public class GameItem : SmartObjects.Item
	{



		[JsonProperty("icon")]
		public readonly UnnyObject Icon;

	}
#pragma warning restore 649
}