using Rexar.Toolbox.Blueprint;

namespace ZooArchitect.Architecture.GameLogic
{
    public struct TileData
    {
        [BlueprintParameter("Is Structure")] public bool IsStructure;
        [BlueprintParameter("Is Walkable")] public bool IsWalkable;
        [BlueprintParameter("Is Animal Habitat")] public bool IsAnimalHabitat;
        [BlueprintParameter("Can Spawn Humans")] public bool CanSpawnHumans;
        [BlueprintParameter("Can Dispawn Humans")] public bool CanDispawnHumans;
        [BlueprintParameter("Is Default")] public bool IsDefault;
    }
}
