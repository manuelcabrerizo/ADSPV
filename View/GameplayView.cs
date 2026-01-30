using Rexar.Toolbox.Services;
using ZooArchitect.Architecture;
using ZooArchitect.View.Logs;
using ZooArchitect.View.Resources;

namespace ZooArchitect.View
{
    [ViewOf(typeof(Gameplay))]
    public class GameplayView : Component
    {
        private static GameScene GameScene => ServiceProvider.Instance.GetService<GameScene>();
        private string BlueprintsPath => "../Assets/StreamingAssets/Blueprints.xlsx";

        private Gameplay gameplay;
        private GameConsoleView consoleView;

        public override void Awake()
        {
            ViewArchitectureMap.Init();

            gameplay = new Gameplay(BlueprintsPath);
            ServiceProvider.Instance.AddService<PrefabsRegistryView>(new PrefabsRegistryView());
            
            ServiceProvider.Instance.AddService<GameScene>(
                GameScene.AddSceneComponent<GameScene>("Scene", this.transform));

            consoleView = new GameConsoleView();
        }

        public override void Start()
        {
            gameplay.Init();
            GameScene.Init();

            gameplay.LateInit();
            GameScene.LateInit();
        }

        public override void Update(float deltaTime)
        {
            gameplay.Tick(deltaTime);
            GameScene.Tick(deltaTime);
        }

        public override void OnDisable()
        {
            gameplay.Dispose();
            GameScene.Dispose();
            consoleView.Dispose();
        }
    }
}
