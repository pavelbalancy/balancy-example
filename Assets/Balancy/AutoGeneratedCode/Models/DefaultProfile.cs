using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class DefaultProfile : ParentBaseData
	{

		[JsonProperty]
		private Data.GeneralInfo info;


		[JsonIgnore]
		public Data.GeneralInfo Info => info;

		protected override void InitParams() {
			base.InitParams();

			ValidateData(ref info);
		}

		public static DefaultProfile Instantiate()
		{
			DefaultProfile result = new DefaultProfile();
			result.Init();
			return result;
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "Info", Info, null, cache);
		}
	}
#pragma warning restore 649
}