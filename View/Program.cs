namespace ZooArchitect.View
{
    public class Program
    {
        public static void Main()
        {
            using (Framework framework = new Framework(800, 600, "Arquitectura de Software para Videojuegos"))
            {
                framework.Run();
            }
        }
    }
}
