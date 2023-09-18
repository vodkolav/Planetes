using Newtonsoft.Json;
using PolygonCollision;
using System;
using System.Collections.Generic;

namespace GameObjects.Model
{
    public  class Corpus 
    {
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
        private List<Figure> _parts { get; set; }

        [JsonIgnore]
        public List<Figure> Parts { get => _parts; }

        public Corpus()
        {
            _parts = new List<Figure>();
        }

        public int Count { get => _parts.Count; }

        internal void Add(Figure f)
        {
            _parts.Add(f);
        }

        internal void ForEach(Action<Figure> action)
        {
            _parts.ForEach(action);
        }

        public Figure this[int i]
        {
            get { return _parts[i]; }
            set { _parts[i] = value; }
        }
    }
}
