using System;
using System.Collections.Generic;

namespace GameObjects.Model
{
    public class Resources
    {

        private readonly static Lazy<Resources> _instance = new Lazy<Resources>(() => new Resources());
        public static Resources Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public Dictionary<string, Corpus> keyValuePairs ;  

        private Resources()
        {
           
        }

        public static void Init()
        {
            Instance.keyValuePairs = new Dictionary<string, Corpus>();
        }

        public static Corpus Get(string asset, Player owner)
        {
            return Instance.keyValuePairs[asset + owner.ID]; 
        }

        public static void Set(string asset, Player owner, Corpus value)
        {
            Instance.keyValuePairs[asset + owner.ID] = value;
        }

        internal static void LoadFrom(Resources modelStore)
        {
            Instance.keyValuePairs = modelStore.keyValuePairs;
        }
    }
}
