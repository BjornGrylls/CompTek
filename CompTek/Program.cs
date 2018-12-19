
using SuperWebSocket;
using System;

namespace ConsoleApp1 {
    class Program {
        private static WebSocketServer wsServer;
        static void Main(string[] args) {
            wsServer = new WebSocketServer();
            int port = 1337;
            wsServer.Setup(port);
            wsServer.NewSessionConnected += WsServer_NewSessionConnected;
            wsServer.NewMessageReceived += WsServer_NewMessageReceived;
            wsServer.NewDataReceived += WsServer_NewDataReceived;
            wsServer.SessionClosed += WsServer_SessionClosed;
            wsServer.Start();
            Console.WriteLine("Server is running on port " + port + ". Press ENTER to exit....");
            Console.ReadKey();
        }

        private static void WsServer_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value) {
            Console.WriteLine("SessionClosed");
        }

        private static void WsServer_NewDataReceived(WebSocketSession session, byte[] value) {
            Console.WriteLine("NewDataReceived");
        }

        private static void WsServer_NewMessageReceived(WebSocketSession session, string value) {
            Console.WriteLine("NewMessageReceived: " + value);
            if (value == "Hello server") {
                session.Send("Hello client");
            }
        }

        private static void WsServer_NewSessionConnected(WebSocketSession session) {
            Console.WriteLine("NewSessionConnected");
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data.SqlClient;
//using System.Net;
//using System.IO;
//using Microsoft.Web.WebSockets;
//using System.Timers;

//namespace CompTek {
//    class Program {
//        static void Main(string[] args) {
//            Methods methods = new Methods();


//            /*SqlConnection myConnection = new SqlConnection("server = localhost; user id = root; database = comptekbase" +

//                                           "Trusted_Connection=yes;" +
//                                           "Integrated security=true;" +
//                                           "database=comptekbase;" +
//                                           "connection timeout=15");

//            myConnection.Open();*/


//            HttpListener listener = new HttpListener();

//            listener.Prefixes.Add("http://127.0.0.1:1337/");
//            listener.Start();
//            string path = methods.GetParent(".\\", 4).ToString();
//            System.Diagnostics.Process.Start(path + "\\index.html");
//            bool running = true;
//            while (running) {
//                Console.WriteLine("Waiting");
//                HttpListenerContext context = listener.GetContext();
//                string message = "Ian";
//                context.Response.ContentLength64 = Encoding.UTF8.GetByteCount(message);
//                context.Response.StatusCode = (int)HttpStatusCode.OK;
//                using (Stream stream = context.Response.OutputStream) {
//                    using (StreamWriter writer = new StreamWriter(stream)) {
//                        writer.Write(message);
//                    }

//                }
//            }

//            Console.WriteLine("Message sent");
//        }



//    }

//    public class MicrosoftWebSockets : WebSocketHandler {
//        private static WebSocketCollection clients = new WebSocketCollection();
//        private string name;

//        public MicrosoftWebSockets() { }

//        public override void OnOpen() {
//            name = WebSocketContext.QueryString["chatname"];
//            clients.Add(this);
//            clients.Broadcast(name + " has connected.");

//            SetTimer();
//        }
//        private static Timer aTimer;
//        private static void SetTimer() {
//            aTimer = new Timer(2000);

//            aTimer.Elapsed += OnTimedEvent;
//            aTimer.AutoReset = true;
//            aTimer.Enabled = true;
//        }
//        private static void OnTimedEvent(object source, ElapsedEventArgs e) {
//            clients.Broadcast("The Elapsed event" + e.SignalTime);
//        }

//        public override void OnMessage(string message) {
//            clients.Broadcast("from server --" + message);
//        }

//        public override void OnClose() {
//            clients.Remove(this);
//            clients.Broadcast(string.Format(name + " has gone away."));

//            aTimer.Stop();
//            aTimer.Dispose();
//        }
//    }

//    class Methods {
//        public string GetParent(string directory, int times) {

//            for (int i = 0; i < times; i++) {
//                directory = Directory.GetParent(directory).ToString();
//            }
//            return directory;

//        }
//    }
//}
