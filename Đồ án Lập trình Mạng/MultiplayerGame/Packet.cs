using System;

namespace MultiplayerGame
{
    internal class Packet
    {
        
    }

    public class Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Point Central_symmetry(Point other)
        {
            int symX = 2 * other.x - this.x;
            int symY = 2 * other.y - this.y;

            return new Point(symX, symY);
        }
    }
}
