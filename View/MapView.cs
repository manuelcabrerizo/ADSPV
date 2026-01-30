using OpenTK.Mathematics;
using Rexar.Toolbox.Blueprint;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using System.Collections.Generic;
using View.Engine.Grid;
using ZooArchitect.Architecture.GameLogic.Events;
using ZooArchitect.View.Data;
using ZooArchitect.View.Resources;

namespace ZooArchitect.View
{
    internal sealed class MapView : ViewComponent
    {
        private Engine Engine => ServiceProvider.Instance.GetService<Engine>();
        private BlueprintRegistry BlueprintRegistry => ServiceProvider.Instance.GetService<BlueprintRegistry>();
        private BlueprintBinder BlueprintBinder => ServiceProvider.Instance.GetService<BlueprintBinder>();
        private PrefabsRegistryView PrefabsRegistryView => ServiceProvider.Instance.GetService<PrefabsRegistryView>();
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

        private Grid grid;
        private Dictionary<int, (string ID, string path)> pathToTilePrefabByIDHash;

        public override void Init()
        {
            base.Init();
            grid = gameObject.AddComponent<Grid>();

            LoadTilePrefabPath();

            EventBus.Subscribe<TileCreatedEvent>(OnTileCreated);
            EventBus.Subscribe<MapCreatedEvent>(OnMapCreated);
        }

        private void LoadTilePrefabPath()
        {
            pathToTilePrefabByIDHash = new Dictionary<int, (string ID, string path)>();

            foreach (string blueprint in BlueprintRegistry.BlueprintsOf(TableNamesView.TILES_VIEW_TABLE_NAME))
            {
                object tileViewData = new TileViewData();
                BlueprintBinder.Apply(ref tileViewData, TableNamesView.TILES_VIEW_TABLE_NAME, blueprint);
                pathToTilePrefabByIDHash.Add(((TileViewData)tileViewData).ArchitectureIHHash,
                    (((TileViewData)tileViewData).architectureID, ((TileViewData)tileViewData).prefabPath));
            }
        }

        private void OnMapCreated(in MapCreatedEvent mapCreatedEvent)
        {
            EventBus.Unsubscribe<TileCreatedEvent>(OnTileCreated);
        }

        private void OnTileCreated(in TileCreatedEvent tileCreatedEvent)
        {
            string pathToTilePrefab = pathToTilePrefabByIDHash[tileCreatedEvent.tileId].path;
            GameObject tileToSpawn = PrefabsRegistryView.Get(TableNamesView.TILES_VIEW_TABLE_NAME, pathToTilePrefabByIDHash[tileCreatedEvent.tileId].ID);
            GameObject tile = Engine.Instantiate(tileToSpawn, grid.CellToLocal(new Vector3(tileCreatedEvent.xCoord, tileCreatedEvent.yCoord, 0.0f)) + 
                new Vector3(grid.cellSize * 0.5f, grid.cellSize * 0.5f, 0.0f));
            tile.transform.parent = grid.gameObject.transform;
        }

        public override void LateInit()
        {
            base.LateInit();
        }

        public override void Dispose()
        {
            base.Dispose();
            EventBus.Unsubscribe<TileCreatedEvent>(OnTileCreated);
            EventBus.Unsubscribe<MapCreatedEvent>(OnMapCreated);
        }
    }

    struct TileViewData
    {
        [BlueprintParameter("Architecture ID")] public string architectureID;
        [BlueprintParameter("Prefab resource path")] public string prefabPath;
        public int ArchitectureIHHash => architectureID.GetHashCode();
    }
}
