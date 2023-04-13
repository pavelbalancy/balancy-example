using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class GeneralInfo : BaseData
	{
		[JsonProperty]
		private int gems;
		[JsonProperty]
		private int gold;

		[JsonIgnore]
		public int Gems
		{
			get => gems;
			set {
				if (UpdateValue(ref gems, value))
					_cache?.UpdateStorageValue(_path + "Gems", gems);
			}
		}

		[JsonIgnore]
		public int Gold
		{
			get => gold;
			set {
				if (UpdateValue(ref gold, value))
					_cache?.UpdateStorageValue(_path + "Gold", gold);
			}
		}

		protected override void InitParams() {
			base.InitParams();

		}

		public static GeneralInfo Instantiate()
		{
			GeneralInfo result = new GeneralInfo();
			result.Init();
			return result;
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "Gems", gems, newValue => Gems = Utils.ToInt(newValue), cache);
			AddCachedItem(path + "Gold", gold, newValue => Gold = Utils.ToInt(newValue), cache);
		}
	}
#pragma warning restore 649
}