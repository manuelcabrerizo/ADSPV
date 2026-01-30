using Rexar.Toolbox.Blueprint;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using System;
using System.Collections.Generic;
using ZooArchitect.Architecture.Data;
using ZooArchitect.Architecture.Exceptions;
using ZooArchitect.Architecture.GameLogic.Events;

namespace ZooArchitect.Architecture.GameLogic
{
    public sealed class Map
    {
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        private BlueprintRegistry BlueprintRegistry => ServiceProvider.Instance.GetService<BlueprintRegistry>();
        private BlueprintBinder BlueprintBinder => ServiceProvider.Instance.GetService<BlueprintBinder>();


        private uint sizeX;
        private uint sizeY;
        private Tile[,] grid;

        private Dictionary<int, TileData> tileDatas;
        private Dictionary<int, string> tileHashToNames;

        public Map(uint sizeX, uint sizeY) 
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            tileDatas = new Dictionary<int, TileData>();
            tileHashToNames = new Dictionary<int, string>();

            foreach (string tileTypeID in BlueprintRegistry.BlueprintsOf(TableNames.TILE_TYPES_TABLE_NAME))
            {
                object tileData = new TileData();
                
                try
                {
                    BlueprintBinder.Apply(ref tileData, TableNames.TILE_TYPES_TABLE_NAME, tileTypeID);
                }
                catch (DataMisalignedException exception)
                {
                    throw new DataEntryException($"Failed to read {TableNames.TILE_TYPES_TABLE_NAME} - {tileTypeID}\n({exception.Message})");
                }

                tileDatas.Add(tileTypeID.GetHashCode(), (TileData)tileData);
                tileHashToNames.Add(tileTypeID.GetHashCode(), tileTypeID);
            }

            grid = new Tile[sizeX, sizeY];
            
            int defaultDataHash = 0;
            foreach (int tileDataHash in tileDatas.Keys)
            {
                if (tileDatas[tileDataHash].IsDefault)
                {
                    defaultDataHash = tileDataHash;
                    break;
                }
            }

            if (defaultDataHash == 0)
            {
                throw new BrokenGameRuleException("Missing default tile definition in Blueprint.xlsx");
            }

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    grid[x, y] = new Tile(defaultDataHash);
                    EventBus.Raise<TileCreatedEvent>(defaultDataHash, x, y);
                }
            }

            EventBus.Raise<MapCreatedEvent>();
        }
    }
}
