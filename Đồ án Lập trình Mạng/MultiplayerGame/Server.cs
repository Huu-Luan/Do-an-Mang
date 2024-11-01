using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGame
{
    public class Server
    {
        private List<TcpClient> clients = new List<TcpClient>();
        public Server(int port, string IP)
        {
            IPAddress myIP = IPAddress.Parse(IP);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint myEP = new IPEndPoint(myIP, port);
            socket.Bind(myEP);
            socket.Listen(10);
            Console.WriteLine($"Server is running on {myIP} : {port}...");

            Task.Run(() => AcceptClients(socket));
        }
        private async Task AcceptClients(Socket socket)
        {
            while (true)
            {
                Socket clientSocket = await socket.AcceptAsync();
                clients.Add(new TcpClient { Client = clientSocket });
                IPEndPoint clientEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;
                if (clientEndPoint != null)
                {
                    string clientIp = clientEndPoint.Address.ToString();
                    int clientPort = clientEndPoint.Port;
                    Console.WriteLine($"Client connected from IP: {clientIp}, Port: {clientPort}");
                }
            }

        }

        public int Count => clients.Count;

        public async Task Receive(TcpClient client)
        {
            using (var networkStream = client.GetStream())
            using (var memoryStream = new MemoryStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, bytesRead);

                    if (!networkStream.DataAvailable) break;
                }
                string json = Encoding.UTF8.GetString(memoryStream.ToArray());
                Console.WriteLine($"Received: {json}");
                DistributePacket(json, client);
            }

            clients.Remove(client);
            client.Close();
            Console.WriteLine("Client disconnected.");
        }

        private void DistributePacket(string json, TcpClient senderClient)
        {
            byte[] data = Encoding.UTF8.GetBytes(json);

            foreach (var client in clients)
            {
                if (client != senderClient && client.Connected)
                {
                    try
                    {
                        client.GetStream().WriteAsync(data, 0, data.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send to client: {ex.Message}");
                    }
                }
            }
        }
        public void SendInitialPos(string jsonPositions)
        {
            byte[] data = Encoding.UTF8.GetBytes(jsonPositions);

            foreach (var client in clients)
            {
                if (client.Connected)
                {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(data, 0, data.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send initial positions to client: {ex.Message}");
                    }
                }
            }
        }

    }
}
