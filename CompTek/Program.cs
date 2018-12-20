using SuperWebSocket;
using System;
using System.IO;

namespace CompTek {
    class Program {
        static void Main(string[] args) {
            Methods methods = new Methods();
            LoginMethods loginMethods = new LoginMethods();
            string input;

            Console.WriteLine("Welcome to my login page made in collaboration with MadsBock \n");
            Console.WriteLine("Please choose a login method: \n\n" +
                "  1) local      2) MadsBock \n");

            bool loggingIn = true;
            while (loggingIn) {

                input = Console.ReadLine();

                switch (input.Trim().ToLower()) {
                    case "1":
                    case "local":
                        loginMethods.LocalLogin();
                        loggingIn = false;
                        break;
                    case "2":
                    case "madsbock":
                        loginMethods.MadsBockLogin();
                        loggingIn = false;
                        break;
                    default:
                        Console.WriteLine("Please write 1 or 2");
                        break;
                }
            }


        }

    }

    class LoginMethods {
        Methods methods = new Methods();
        private static WebSocketServer wsServer;

        public void LocalLogin() {
            Console.Write("\nUsername: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");

            string pass = "";
            ConsoleKeyInfo key;

            do {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace) {
                    pass += key.KeyChar;
                }
            }
            while (key.Key != ConsoleKey.Enter);

        }

        public void MadsBockLogin() {

            wsServer = new WebSocketServer();


            int port = 1337;
            wsServer.Setup(port);
            wsServer.NewSessionConnected += WsServer_NewSessionConnected;
            wsServer.NewMessageReceived += WsServer_NewMessageReceived;
            wsServer.NewDataReceived += WsServer_NewDataReceived;
            wsServer.SessionClosed += WsServer_SessionClosed;
            wsServer.Start();

            string path = methods.GetParent(".\\", 4).ToString();
            System.Diagnostics.Process.Start(path + "\\index.html");

            bool running = true;
            while (running) {
                string input = Console.ReadLine();

                switch (input) {
                    case "exit":
                        wsServer.Stop();
                        running = false;
                        break;
                    default:
                        //wsServer.Send();
                        break;
                }

            }
        }


        private static void WsServer_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value) {
            //Console.WriteLine("SessionClosed");
        }

        private static void WsServer_NewDataReceived(WebSocketSession session, byte[] value) {
            //Console.WriteLine("NewDataReceived");
        }

        private static void WsServer_NewMessageReceived(WebSocketSession session, string value) {
            Methods methods = new Methods();
            string[] splittet = value.Split(',');
            methods.SQLLogin(splittet[0], splittet[1]);

            /*Console.WriteLine("Webpage> " + value);
            if (value == "b,b") {
                session.Send("Luk");
            }*/
        }

        private static void WsServer_NewSessionConnected(WebSocketSession session) {
            //Console.WriteLine("NewSessionConnected");
        }
    }
}

class Methods {

    /// <summary>
    /// Gets the parent of the directory repeatedly "times" times
    /// </summary>
    /// <param name="directory">The start directory</param>
    /// <param name="times">Amount of parent folders you go up</param>
    public string GetParent(string directory, int times) {
        for (int i = 0; i < times; i++) {
            directory = Directory.GetParent(directory).ToString();
        }
        return directory;
    }

    public void SQLLogin(string username, string password) {
        Console.WriteLine(username + password);
    }

    public void FinalPage(string username) {
        Console.WriteLine("Welcome " + username);
        Console.WriteLine("You can exit by pressing ALT+F4");
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
