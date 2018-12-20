using SuperWebSocket;
using System;
using System.IO;
using MySql.Data.MySqlClient;
using System.Diagnostics;

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

                input = (input ?? "");

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

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter) {
                    pass += key.KeyChar;
                }
            }
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            if (methods.SQLLogin(username, pass)) {
                methods.FinalPage(username);
            } else {

            }

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
            Process.Start(path + "\\index.html");

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
        /// <summary>
        /// Asks MySQL server for users and return true if chosen user is found
        /// </summary>
        /// <param name="username">Username of user</param>
        /// <param name="password">Password of user</param>
        public bool SQLLogin(string username, string password) {
            MySqlConnection connection;
            string server = "localhost";
            string database = "comptekbase";
            string uid = "test";
            string passwordSQL = "tester";
            string port = "3307";
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + passwordSQL + ";" + "PORT=" + port + ";";

            connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = @"SELECT username FROM Users WHERE username = '" + username + @"' AND password = '" + password + "';";
            string result = "";
            if (connection.State.ToString() == "Open") {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read()) {
                    result = dataReader["username"].ToString();
                }

                dataReader.Close();

                connection.Close();
            }

            return (result == username ? true : false);
        }

        public void FinalPage(string username) {
            Console.WriteLine("Welcome to the club " + username);
            Console.WriteLine("You can exit by pressing Enter");
            Console.Read();
        }
    }


    class DBConnect {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string port;

        //Constructor
        public DBConnect() {
            Initialize();
        }

        //Initialize values
        private void Initialize() {
            server = "localhost";
            database = "comptekbase";
            uid = "test";
            password = "tester";
            port = "3307";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + "PORT=" + port + ";";

            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        //open connection to database
        private bool OpenConnection() {

            try {
                Console.WriteLine("virker");
                return true;
            } catch (MySqlException ex) {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number) {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection() {
            try {
                connection.Close();
                return true;
            } catch (MySqlException ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert() {
            string query = "INSERT INTO tableinfo (name, age) VALUES('John Smith', '33')";

            //open connection
            if (this.OpenConnection() == true) {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update() {
            string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

            //Open connection
            if (this.OpenConnection() == true) {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete() {
            string query = "DELETE FROM tableinfo WHERE name='John Smith'";

            if (this.OpenConnection() == true) {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement
        public string SELECT() {

            string query = "SELECT * FROM Users WHERE username = 'admin' AND password = 'admin';";

            string result = "";

            if (this.OpenConnection() == true) {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read()) {
                    result = dataReader["username"].ToString();
                }

                dataReader.Close();

                CloseConnection();

                return result;
            } else {
                return result;
            }
        }

        //Count statement
        public int Count() {

            string query = "SELECT Count(*) FROM tableinfo";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true) {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            } else {
                return Count;
            }
        }

        //Backup
        public void Backup() {

            try {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;
                int millisecond = Time.Millisecond;

                //Save file to C:\ with the current date as a filename
                string path;
                path = "C:\\MySqlBackup" + year + "-" + month + "-" + day +
            "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
                StreamWriter file = new StreamWriter(path);


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysqldump";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
            } catch (IOException ex) {
                Console.WriteLine("Error , unable to backup!");
            }
        }

        //Restore
        public void Restore() {

            try {
                //Read file from C:\
                string path;
                path = "C:\\MySqlBackup.sql";
                StreamReader file = new StreamReader(path);
                string input = file.ReadToEnd();
                file.Close();

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysql";
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;


                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
            } catch (IOException ex) {
                Console.WriteLine("Error , unable to Restore!");
            }
        }
    }
}