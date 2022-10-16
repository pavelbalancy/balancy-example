using Newtonsoft.Json;
using System;

namespace Balancy.Models.Store
{
#pragma warning disable 649

	public class StoreConfig : BaseModel
	{

		[JsonProperty]
		private string unnyIdCondition;
		private Models.SmartObjects.Conditions.Logic condition;
		[JsonProperty]
		private string[] unnyIdPages;
		private Models.Store.StorePage[] pages;


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
		public Models.Store.StorePage[] Pages
		{
			get
			{
				if (pages == null) {
					if (unnyIdPages == null)
						return pages = new Models.Store.StorePage[0];
					pages = new Models.Store.StorePage[unnyIdPages.Length];
					for (int i = 0;i < unnyIdPages.Length;i++)
						pages[i] = DataEditor.GetModelByUnnyId<Models.Store.StorePage>(unnyIdPages[i]);
				}
				return pages;
			}
		}

	}
#pragma warning restore 649
}