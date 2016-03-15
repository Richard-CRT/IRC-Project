using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using BouncerClientLibraryNS;
using System.Windows.Forms;


// State object for receiving data from remote device.
public class StateObject
{
    // Client socket.
    public Socket workSocket = null;
    // Size of receive buffer.
    public const int BufferSize = 1024;
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
    // Received data string.
    public StringBuilder sb = new StringBuilder();
}


public class AsynchronousClient
{
    private string dataRead;
    private string cIP { get; set; }
    // The port number for the remote device.
    private int cPort { get; set; }
    public BouncerLibraryMessageCallBack CommandHandler;
    public bool connected { get; set;  }
    public Socket client;
    public AsynchronousClient(BouncerLibraryMessageCallBack commandHandler, string ip, int port)
    {
        cIP = ip;
        cPort = port;
        CommandHandler = commandHandler;
    }

    // ManualResetEvent instances signal completion.
    private ManualResetEvent connectDone =
        new ManualResetEvent(false);
    private ManualResetEvent sendDone =
        new ManualResetEvent(false);
    private ManualResetEvent receiveDone =
        new ManualResetEvent(false);

    // The response from the remote device.
    private String response = String.Empty;

    private void StartClient()
    {
        // Connect to a remote device.
        try
        {

            //IPHostEntry ipHostInfo = Dns.GetHostEntry(cIP);
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPAddress ipAddress = IPAddress.Parse(cIP);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, cPort);
            // Create a TCP/IP socket.
            client = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);


            // Connect to the remote endpoint.
            client.BeginConnect(remoteEP,
                new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();

            connected = true;
            CommandHandler("SERVER: CONNECTED", false);
            // Send test data to the remote device.

            // Receive the response from the remote device.
            Receive(client);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.
            client.EndConnect(ar);

            //base.Print("Socket connected to {0}",
            //    client.RemoteEndPoint.ToString());

            // Signal that the connection has been made.
            connectDone.Set();
        }
        catch (System.Net.Sockets.SocketException e)
        {
            CommandHandler("Cannot Connect", false);
        }
    }

    private void Receive(Socket client)
    {
        try
        {
            dataRead = "";
            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = client;

            // Begin receiving the data from the remote device.
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            CommandHandler(e.ToString(),false);
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        //try
        //{
            // Retrieve the state object and the client socket 
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;

        // Read data from the remote device.
        try
        {
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There might be more data, so store the data received so far.
                dataRead += Encoding.UTF8.GetString(state.buffer, 0, bytesRead);
                string line = "";
                int counter = 0;
                foreach (char character in dataRead)
                {
                    counter++;
                    if (character != '\x04')
                    {
                        line += character;
                    }
                    else
                    {
                        CommandHandler(line, true);
                        line = "";
                        if (dataRead.Length == 1)
                        {
                            dataRead = "";
                        }
                        else
                        {
                            dataRead = dataRead.Substring(counter);
                        }
                        counter = 0;
                    }
                }
                //state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

                // Get the rest of the data.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            else
            {
                // All the data has arrived; put it in response.
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                }
                // Signal that all bytes have been received.
                receiveDone.Set();
            }
        }
        catch (System.Net.Sockets.SocketException e)
        {
            CommandHandler("SERVER: DISCONNECTED", false);
            connected = false;
        }
        catch (System.ObjectDisposedException e)
        {

        }
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e.ToString());
        //}
    }

    public void Send(string data)
    {
        if (connected)
        {
            SendData(client, data + '\x01');
            if (data.Length >= 5)
            {
                if (data.Substring(0, 5) != "QUIT")
                {
                    CommandHandler(data, false);
                }
            }
            else
            {
                CommandHandler(data, false);
            }
        }
        else
        {
            CommandHandler("SERVER: NOT CONNECTED", false);
        }
    }

    private void SendData(Socket client, String data)
    {
        // Convert the string data to byte data using UTF8 encoding.
        byte[] byteData = Encoding.UTF8.GetBytes(data);

        // Begin sending the data to the remote device.
        client.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), client);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.
            int bytesSent = client.EndSend(ar);
            //base.Print("Sent {0} bytes to server.", bytesSent);

            // Signal that all bytes have been sent.
            sendDone.Set();
        }
        catch (Exception e)
        {
            CommandHandler(e.ToString(),false);
        }
    }

    public void Start()
    {
        StartClient();
    }
}