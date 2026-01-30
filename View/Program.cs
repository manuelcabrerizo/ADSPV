using System;
using System.Globalization;

namespace ZooArchitect.View
{
    public class Program
    {
        public static void Main()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            using (Framework framework = new Framework(1280, 720, "Arquitectura de Software para Videojuegos"))
            {
                framework.Run();
            }
        }
    }
}
