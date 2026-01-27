namespace ZooArchitect.View
{
    public class Program
    {
        // Entry point of the program
        public static void Main()
        {
            using (Game game = new Game(800, 600, "Arquitectura de Software para Videojuegos"))
            {
                game.Run();
            }
        }
    }
}
