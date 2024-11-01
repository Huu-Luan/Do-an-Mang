using System;
using System.Collections.Generic; 
using System.Text.Json;

namespace MultiplayerGame
{
    internal class Game_Event
    {
    }

    public class InitialPos
    {
        private Server server;

        public InitialPos(Server serverInstance)
        {
            server = serverInstance;
        }

        public void Initialize()
        {
            // Nhận số lượng client
            int num = server.Count;

            // Tạo danh sách các vị trí ban đầu cho các client
            List<Point> initialPositions = new List<Point>();
            Random random = new Random();

            for (int i = 0; i < num; i++)
            {
                int initY = random.Next(0, 100);
                Point point = new Point(0, initY);
                initialPositions.Add(point);
            }

            // Chuyển danh sách các vị trí thành chuỗi JSON
            string jsonPositions = JsonSerializer.Serialize(initialPositions);
            Console.WriteLine($"Initial positions JSON: {jsonPositions}");

            // Gửi vị trí đến các client
            server.SendInitialPos(jsonPositions);
        }
    }

    public class Rank
    {
        // Xếp hạng người chơi sau mỗi section
    }
}
