using System;

namespace Rexar.View
{
    public class Program
    {
        // Entry point of the program
        public static void Main()
        {
            using (GameplayView view = new GameplayView(800, 600, "Arquitectura de Software para Videojuegos"))
            {
                view.Run();
            }
        }
    }
}
