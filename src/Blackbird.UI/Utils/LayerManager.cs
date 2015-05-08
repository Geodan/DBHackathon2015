using System;
using System.Collections.Generic;
using System.Linq;
using Blackbird.WPF.Logging;
using Mapsui;
using Mapsui.Layers;

namespace Blackbird.WPF.Utils
{
    /// <summary>
    /// Helper class for adding and removing layers on the map, layers shouldn't be 
    /// added directly to the map only trough the LayerManager
    /// </summary>
    public class LayerManager
    {
        private LayerCollection _layerCollection;
        private Dictionary<string, bool> RemovableByUser { get; set; }

        public List<ILayer> BackgroundLayers { get; private set; }
        public List<ILayer> ContextLayers { get; private set; }        
        public ILayer StartLayer { get; set; }

        public delegate void ChangedEventHandler(ILayer layer);
        public delegate void LayerAddedEvent(ILayer layer, bool removableByUser);

        public event LayerAddedEvent BackgroundLayerAdded;
        public event LayerAddedEvent ContextLayerAdded;
        public event ChangedEventHandler LayerMoved;
        public event ChangedEventHandler LayerRemoved;

        /// <summary>
        /// Creates the LayerManager for managing different layers on the map
        /// </summary>
        public LayerManager()
        {
            _layerCollection = new LayerCollection();
            BackgroundLayers = new List<ILayer>();
            ContextLayers = new List<ILayer>();            
            RemovableByUser = new Dictionary<string, bool>();
        }

        public LayerCollection LayerCollection
        {
            get
            {
                return _layerCollection;
            }
            set { _layerCollection = value; }
        }

        public bool IsLayerRemovableByUser(string guid)
        {
            foreach (var layerGuid in RemovableByUser.Where(layerGuid => layerGuid.Key.Equals(guid)))
                return layerGuid.Value;

            return true;
        }

        /// <summary>
        /// Get all layers added to the LayerManager in 1 List
        /// Sorted by LayerName
        /// </summary>
        public List<ILayer> GetAllLayers()
        {
            var layers = new List<ILayer>();

            layers.AddRange(BackgroundLayers);
            layers.AddRange(ContextLayers);            

            return layers.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Add a background layer to the map
        /// </summary>
        /// <param name="layer">layer to be added of type ILayer</param>
        /// <param name="ignorePlacement">if set to true the layer will not be added to the bottom</param>
        /// <param name="removableByUser">If set to false the remove slider will not be shown</param>
        public void AddBackgroundLayer(ILayer layer, bool ignorePlacement = false, bool removableByUser = true)
        {
            GenerateTagIfNotExists(layer);

            if (GetLayerById(layer.Tag.ToString()) != null)//already exists
                return;

            var position = BackgroundLayers.Count;
            if (ignorePlacement)
                position = BackgroundLayers.Count + ContextLayers.Count;

            _layerCollection.Insert(position, layer);

            BackgroundLayers.Add(layer);
            RemovableByUser.Add(layer.Tag.ToString(), removableByUser);
            OnBackgroundLayerAdded(layer, removableByUser);

            Log4netLogger.InfoFormat("Backgroundlayer added to map: {0}", layer.Name);
        }

        /// <summary>
        /// Add a context layer to the map, they will be displayed on top of
        /// a background layer, multiple context layers can be active
        /// </summary>
        /// <param name="layer">layer to be added of type ILayer</param>
        /// <param name="removableByUser">If set to false the remove slider will not be shown</param>
        public void AddContextLayer(ILayer layer, bool removableByUser = true)
        {
            GenerateTagIfNotExists(layer);

            if (GetLayerById(layer.Tag.ToString()) != null)//already exists
                return;

            var position = BackgroundLayers.Count + ContextLayers.Count;
            _layerCollection.Insert(position, layer);

            ContextLayers.Add(layer);
            RemovableByUser.Add(layer.Tag.ToString(), removableByUser);
            OnContextLayerAdded(layer, removableByUser);

            Log4netLogger.InfoFormat("Contextlayer added to map: {0}", layer.Name);
        }

        /// <summary>
        /// Move a layer to a new index
        /// </summary>
        /// <param name="layer">layer to move</param>
        /// <param name="newIndex">new index number in the collection</param>
        public void MoveLayer(ILayer layer, int newIndex)
        {
            _layerCollection.Move(newIndex, layer);
            OnLayerMoved(layer);

            Log4netLogger.InfoFormat("layer {0} moved to {1}, total layers = {2}", layer.Name, newIndex, _layerCollection.Count);
        }

        /// <summary>
        /// Get a layer by guid (own set GUID in the tag object)
        /// </summary>
        public ILayer GetLayerById(string guid)
        {
            var layerById = _layerCollection.FirstOrDefault(layer => guid.Equals(layer.Tag.ToString()));
            return layerById;
        }

        /// <summary>
        /// Get the Layer collection index number belonging to a layer
        /// </summary>
        public int GetLayerCollectionIdFromLayer(ILayer layer)
        {
            for (var i = 0; i < _layerCollection.Count; i++)
            {
                if (layer == _layerCollection[i])
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Remove a layer from the map
        /// </summary>
        /// <param name="layer">Layer to be removed</param>
        public void RemoveLayer(ILayer layer)
        {
            _layerCollection.Remove(layer);
            BackgroundLayers.Remove(layer);
            ContextLayers.Remove(layer);
            RemovableByUser.Remove(layer.Tag.ToString());

            OnLayerRemoved(layer);

            Log4netLogger.InfoFormat("Layer removed from map: {0}", layer.Name);
        }

        public void ClearMap()
        {
            var col = LayerCollection.ToList();
            foreach (var layer in col)
            {
                RemoveLayer(layer);
            }

            BackgroundLayers.Clear();
            ContextLayers.Clear();
            LayerCollection.Clear();

            Log4netLogger.InfoFormat("Map layers cleared");
        }

        protected virtual void OnBackgroundLayerAdded(ILayer layer, bool removableByUser)
        {
            if (BackgroundLayerAdded != null)
                BackgroundLayerAdded(layer, removableByUser);
        }

        protected virtual void OnContextLayerAdded(ILayer layer, bool removableByUser)
        {
            if (ContextLayerAdded != null)
                ContextLayerAdded(layer, removableByUser);
        }

        protected virtual void OnLayerMoved(ILayer layer)
        {
            if (LayerMoved != null)
                LayerMoved(layer);
        }

        protected virtual void OnLayerRemoved(ILayer layer)
        {
            if (LayerRemoved != null)
                LayerRemoved(layer);
        }

        private static void GenerateTagIfNotExists(ILayer layer)
        {
            Guid id;
            if (layer.Tag != null && Guid.TryParse(layer.Tag.ToString(), out id))
                return;

            layer.Tag = Guid.NewGuid();
        }
    }
}
