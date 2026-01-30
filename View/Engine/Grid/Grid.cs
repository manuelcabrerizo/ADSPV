using OpenTK.Mathematics;
using ZooArchitect.View;

namespace View.Engine.Grid
{
    public class Grid : Component
    {

        private int posX = -(5 * 64);
        private int posY = -(5 * 64);
        public int cellSize = 64;


        public Vector3 CellToLocal(Vector3 cell)
        {
            Vector3 localPosition = new Vector3(posX + cell.X * cellSize, posY + cell.Y * cellSize, cell.Z);
            return localPosition;
        }
    }
}
