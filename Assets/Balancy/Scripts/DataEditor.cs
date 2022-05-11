using System.Collections.Generic;
using Balancy.Models;

namespace Balancy
{
    public partial class DataEditor
    {
        public static Models.SmartObjects.SmartConfig SmartConfig => DataManager.SmartConfig;
        public static List<Models.SmartObjects.GameOffer> GameOffers => DataManager.GameOffers;
        public static List<Models.SmartObjects.GameEvent> GameEvents => DataManager.GameEvents;
        public static List<Models.SmartObjects.Analytics.ABTest> ABTests  => DataManager.ABTests;

        static partial void PrepareGeneratedData();

        public static void Init()
        {
            Storage.OnPrepareModelsAndData += PrepareModelsAndData;
        }

        private static void PrepareModelsAndData()
        {
            DataManager.Init();
            PrepareGeneratedData();
        }

        protected static DataManager.ParseWrapper<T> ParseDictionary<T>() where T : BaseModel
        {
            return DataManager.ParseDictionary<T>();
        }
        
        public static T GetModelByUnnyId<T>(string unnyId) where T : BaseModel
        {
            return DataManager.GetModelByUnnyId<T>(unnyId);
        }
    }
}
