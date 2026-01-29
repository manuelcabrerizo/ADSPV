using Rexar.Toolbox.Services;
using ZooArchitect.Architecture;
using ZooArchitect.Vew.Controller;
using ZooArchitect.View.GameLogic.Entities.Systems;
using ZooArchitect.View.Logs;
using ZooArchitect.View.Resources;

namespace ZooArchitect.View
{
    [ViewOf(typeof(Gameplay))]
    public class GameplayView : Component
    {
        private string BlueprintsPath => "../Assets/StreamingAssets/Blueprints.xlsx";

        private Gameplay gameplay;
        private GameConsoleView consoleView;
        private EntityFactoryView entityFactoryView;
        private SpawnEntityControllerView spawnEntityControllerView;

        public override void Init()
        {
            ViewArchitectureMap.Init();
            gameplay = new Gameplay(BlueprintsPath);

            ServiceProvider.Instance.AddService<PrefabsRegistryView>(new PrefabsRegistryView());
            ServiceProvider.Instance.AddService<EntityRegistryView>(new EntityRegistryView());
            entityFactoryView = new EntityFactoryView();

            consoleView = new GameConsoleView();
        }

        public override void LateInit()
        {
            gameplay.Init();
            gameplay.LateInit();
            spawnEntityControllerView = new SpawnEntityControllerView();

        }

        public override void Tick(float deltaTime)
        {
            gameplay.Tick(deltaTime);
            spawnEntityControllerView.Tick(deltaTime);
        }

        public override void Copy(Component component)
        {
        }


        public override void OnDisable()
        {
            gameplay.Dispose();
            consoleView.Dispose();
            spawnEntityControllerView.Dispose();
        }
    }
}
