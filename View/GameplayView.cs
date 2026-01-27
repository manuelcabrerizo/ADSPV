using Rexar.Toolbox.Services;
using ZooArchitect.Architecture;
using ZooArchitect.View.GameLogic.Entities.Systems;
using ZooArchitect.View.Logs;
using ZooArchitect.View.Resources;

namespace ZooArchitect.View
{
    public class GameplayView : Component
    {
        private string BlueprintsPath => "../Assets/StreamingAssets/Blueprints.xlsx";

        private Gameplay gameplay;
        private ConsoleView consoleView;

        private EntityFactoryView entityFactoryView;
        public GameplayView(GameObject owner) : base(owner) 
        {
        }

        public override void Init()
        {
            gameplay = new Gameplay(BlueprintsPath);
            consoleView = new ConsoleView();

            ServiceProvider.Instance.AddService<PrefabsRegistryView>(new PrefabsRegistryView());
            ServiceProvider.Instance.AddService<EntityRegistryView>(new EntityRegistryView());
        }

        public override void LateInit()
        {
            gameplay.Init();

            entityFactoryView = new EntityFactoryView();

            gameplay.LateInit();
        }

        public override void Tick(float deltaTime)
        {
            gameplay.Tick(deltaTime);
        }

        public override void Copy(Component component)
        {
        }

        // TODO: ...
        /*
        protected override void OnUnload()
        {
            consoleView.Dispose();
            Graphics.Dispose();
        }
        */
    }
}
