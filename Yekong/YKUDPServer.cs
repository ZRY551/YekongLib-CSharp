namespace YekongLib.Yekong;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

// Define a delegate type for the user-defined data handler function
public delegate void DataHandler(byte[] data, IPEndPoint remoteEndPoint,UdpClient udpClient);

// Define a class for the UDP server
public class YKUDPServer
{
    // Declare a UdpClient field to communicate with the clients
    private UdpClient udpClient;

    // Declare a DataHandler field to store the user-defined data handler function
    private DataHandler dataHandler;

    // Define a constructor that takes a data handler function, a port number and an optional address
    public YKUDPServer(DataHandler dataHandler, int port, string address = "0.0.0.0")
    {
        // Initialize the UdpClient with the given port and address
        udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(address), port));

        // Initialize the DataHandler with the given function
        this.dataHandler = dataHandler;
    }

    // Define a method to start listening for incoming data
    public async Task StartAsync()
    {
        // Loop indefinitely
        while (true)
        {
            try
            {
                // Receive a datagram from any remote endpoint and get the data and the remote endpoint
                var result = await udpClient.ReceiveAsync();
                var data = result.Buffer;
                var remoteEndPoint = result.RemoteEndPoint;

                // Call the user-defined data handler function with the data and the remote endpoint
                dataHandler(data, remoteEndPoint, udpClient);
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine(ex.Message);
            }
        }
    }

    // Define a method to stop listening and close the UdpClient
    public void Stop()
    {
        // Close the UdpClient
        udpClient.Close();
    }
}
