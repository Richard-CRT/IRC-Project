using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BouncerServerLibraryNS;

// State object for reading client data asynchronously
public class StateObject
{
    // Client  socket.
    public Socket workSocket = null;
    // Size of receive buffer.
    public const int BufferSize = 1024;
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
    // Received data string.
    public StringBuilder sb = new StringBuilder();
}

public class AsynchronousSocketListener
{
    public List<Socket> Handlers = new List<Socket>();

    // Thread signal.
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    private string cIP { get; set; }
    // The port number for the remote device.
    private int cPort { get; set; }
    private BouncerLibraryMessageCallBack CommandHandler;

    public AsynchronousSocketListener(BouncerLibraryMessageCallBack commandHandler, int port)
    {
        cPort = port;
        CommandHandler = commandHandler;
    }

    public void StartListening()
    {
        bool first = true;
        // Data buffer for incoming data.
        byte[] bytes = new Byte[1024];

        // Establish the local endpoint for the socket.
        // The DNS name of the computer
        // running the listener is "host.contoso.com".
        string ip = GetAllLocalIPv4(NetworkInterfaceType.Ethernet)[0];
        IPAddress ipAddress = IPAddress.Parse(ip);
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, cPort);

        // Create a TCP/IP socket.
        Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and listen for incoming connections.
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(100);


            while (true)
            {
                // Set the event to nonsignaled state.
                allDone.Reset();
                // Start an asynchronous socket to listen for connections.
                if (!first)
                {
                    first = false;
                    CommandHandler("NewS Connection!", false);
                }
                CommandHandler("Awaiting connection...",false);

                listener.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    listener);

                // Wait until a connection is made before continuing.
                allDone.WaitOne();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }

    public void AcceptCallback(IAsyncResult ar)
    {
        // Signal the main thread to continue.
        allDone.Set();

        // Get the socket that handles the client request.
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        Handlers.Add(handler);

        // Create the state object.
        StateObject state = new StateObject();
        state.workSocket = handler;
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(ReadCallback), state);
    }

    public void ReadCallback(IAsyncResult ar)
    {
        String content = String.Empty;

        // Retrieve the state object and the handler socket
        // from the asynchronous state object.
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;

        // Read data from the client socket.
        try
        {
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.UTF8.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.sb.ToString();
                if (content[content.Length - 1] == '\x01')
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    content = content.Substring(0, content.Length - 1);
                    CommandHandler(content, true);
                    // Echo the data back to the client.

                    Send(handler, content);

                    // Reset the state object.
                    state = new StateObject();
                    state.workSocket = handler;
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }
        catch (System.Net.Sockets.SocketException e)
        {
            Handlers.Remove(handler);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
            CommandHandler("Closed Connection!", false);
        }
    }

    private void Send(Socket handler, String data)
    {
        // Convert the string data to byte data using UTF8 encoding.
        byte[] byteData = Encoding.UTF8.GetBytes(data+'\x04');
        
        CommandHandler(data, false);

        // Begin sending the data to the remote device.
        handler.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), handler);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket handler = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.
            int bytesSent = handler.EndSend(ar);

            //handler.Shutdown(SocketShutdown.Both);
            //handler.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private string[] GetAllLocalIPv4(NetworkInterfaceType _type)
    {
        List<string> ipAddrList = new List<string>();
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddrList.Add(ip.Address.ToString());
                    }
                }
            }
        }
        return ipAddrList.ToArray();
    }
}