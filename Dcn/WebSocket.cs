using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
//using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Linq;

namespace Dcn
{
    public class WebSocket
    {

        /*************************************************************************
           Shared variables
        *************************************************************************/
        #region THREAD_VARIABLES

        private readonly object _activeThreadLocker = new object();
        private int _activeThread = 0;

        public int activeThread
        {
            get { lock (_activeThreadLocker) { return _activeThread; } }
            set { lock (_activeThreadLocker) { _activeThread = value; } }
        }

        private readonly object _closeLocker = new object();
        private bool _close = false;

        public bool close
        {
            get { lock (_closeLocker) { return _close; } }
            set { lock (_closeLocker) { _close = value; } }
        }

        #endregion

        /*************************************************************************
            Proprierties
        *************************************************************************/
        private Int32 port;
        private IPAddress localAddr = IPAddress.Any;

        private TcpListener tcpListener;
        private HttpServer httpServer;

        /*************************************************************************
           Constructor
        *************************************************************************/
        public WebSocket(HttpServer httpServer, int port)
        {

            // Inizializza le variabili
            this.httpServer = httpServer;
            this.port = port;

            try
            {

                // Crea un nuovo listener
                tcpListener = new TcpListener(localAddr, port);
                tcpListener.Start();

                // Tempo attesa
                Thread.Sleep(250);

                // Apre un thread in attesa di connessione
                Thread thread = new Thread(Listening);
                thread.Start(tcpListener);
            }
            catch (Exception es)
            {
                Console.WriteLine("An Exception Occurred while Listening :" + es.ToString());
            }
        }

        /*************************************************************************
           Calcola i millisecondi assoluti 
        *************************************************************************/
        public long Milliseconds()
        {

            DateTime dt = new DateTime(2013, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            DateTime dtNow = DateTime.Now;
            TimeSpan result = dtNow.Subtract(dt);

            long seconds = Convert.ToInt64(result.TotalMilliseconds);
            return seconds;
        }

        /*************************************************************************
           Attende una nuova connessione   
        *************************************************************************/
        public void Listening(object tcpListener)
        {
            TcpListener localTcpListener = (TcpListener) tcpListener;

            try
            {
                while (!close)
                {

                    // Attende un nuovo client
                    Console.WriteLine("Waiting new web client");
                    Socket socket = localTcpListener.AcceptSocket();
                    Console.WriteLine("Web client request accepted");

                    // Tempo attesa
                    Thread.Sleep(250);

                    // Lancia un nuovo thread per il client
                    Thread thread = new Thread(Protocol);
                    thread.Start(socket);
                    activeThread++;
                }
            }
            catch (Exception es)
            {
                Console.WriteLine("Exception public void Listening(object tcpListener)");
            }
        }

        /*************************************************************************
           Gestisce la comunicazione con il browser 
        *************************************************************************/
        public void Protocol(object socket)
        {
            long newTime = 0;
            long oldTime = 0;
            long toutTime = 0;

            bool exit = false;

            Socket localSocket = (Socket)socket;

            try
            {

                // Ottiene gli stream
                NetworkStream stream = new NetworkStream(localSocket);
                StreamWriter streamWriter = new System.IO.StreamWriter(stream);
                StreamReader streamReader = new System.IO.StreamReader(stream);

                // Legge l'intestazione
                int sex = localSocket.Available;
                Byte[] bytes = new Byte[sex];
                stream.Read(bytes, 0, bytes.Length);

                //translate bytes of request to string
                String data = Encoding.UTF8.GetString(bytes);
                if (new Regex("^GET").IsMatch(data))
                {
                    Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
                        + "Connection: Upgrade" + Environment.NewLine
                        + "Upgrade: websocket" + Environment.NewLine
                        + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                            SHA1.Create().ComputeHash(
                                Encoding.UTF8.GetBytes(
                                    new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                )
                            )
                        ) + Environment.NewLine
                        + Environment.NewLine);

                    // Risponde per l'accettazione della connessione
                    stream.Write(response, 0, response.Length);

                    Thread.Sleep(500);

                    // Azzera i temporizzatori
                    toutTime = oldTime = newTime = Milliseconds();

                    // Rimane in loop sino alla richiesta di chiusura
                    while ((!close) && (!exit))
                    {

                        // Introduce una sospensione del thread per non sovraccaricare la cpu
                        Thread.Sleep(10);

                        // Aggiorna il timer
                        newTime = Milliseconds();

                        // ************** Time Out **************************************
                        if ((newTime - toutTime) > 5000)
                        {
                            toutTime = newTime;
                        }

                        // ************ Trasmette periodicamente ************************
                        else if ((newTime - oldTime) > 1000)
                        {
                            oldTime = newTime;
                        }

                        if (!SocketConnected(localSocket))
                        {
                            exit = true;
                        }

                        // ********** Legge ed interpreta i caratteri in arrivo *********
                        int count = localSocket.Available;
                        if (count > 0)
                        {

                            // Legge i caratteri in ingresso
                            byte[] inBytes = new byte[count];
                            stream.Read(inBytes, 0, count);

                            // Decodifica il messaggio in arrivo
                            byte[] decodedBytes = DecodeData(inBytes);

                            string decodedString = ASCIIEncoding.ASCII.GetString(decodedBytes, 0, decodedBytes.Length);

                            decodedString += "";

                            Console.Write(ASCIIEncoding.ASCII.GetString(decodedBytes, 0, decodedBytes.Length));

                            // Se ha trovato il messaggio di refresh
                            if (true) // ParseTopic(decodedString))
                            {

                                string outString = "";

                                if (CreateAnswer(decodedString, ref outString))
                                {

                                    byte[] encoded = Encoding.UTF8.GetBytes(outString);
                                    byte[] output = EncodeData(encoded);
                                    Console.WriteLine(outString);
                                    localSocket.Send(output);
                                }
                            }
                        }

                        if (!localSocket.Connected)
                        {
                            close = true;
                        }
                    }
                }
                localSocket.Disconnect(true);
                localSocket.Close();
                Console.WriteLine("Web client disconnected");
            }
            catch (Exception es)
            {
                Console.WriteLine("Exception public void Protocol(object socket)");
                
                localSocket.Disconnect(true);
                localSocket.Close();

                Console.WriteLine("Web client disconnected");
            }

            // Decrementa il numero di thread attivi
            activeThread--;
        }

        /*************************************************************************
           Testa la disconnessione della websocket 
        *************************************************************************/
        bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        /*************************************************************************
           Implementa la codifica della websocket 
        *************************************************************************/
        private byte[] EncodeData(byte[] decoded)
        {
            byte[] encoded;

            int indexStartRawData = -1;

            if (decoded.Length <= 125)
            {
                encoded = new byte[decoded.Length + 2];

                encoded[0] = 129;
                encoded[1] = (byte)decoded.Length;

                indexStartRawData = 2;
            }

            else if ((decoded.Length >= 126) && (decoded.Length <= 65535))
            {
                encoded = new byte[decoded.Length + 4];

                encoded[0] = 129;
                encoded[1] = 126;
                encoded[2] = (byte)((decoded.Length >> 8) & 0xFF);
                encoded[3] = (byte)((decoded.Length) & 0xFF);

                indexStartRawData = 4;
            }
            else
            {
                encoded = new byte[decoded.Length + 10];

                encoded[0] = 129;
                encoded[1] = 127;
                encoded[2] = (byte)((decoded.Length >> 56) & 0xFF);
                encoded[3] = (byte)((decoded.Length >> 48) & 0xFF);
                encoded[4] = (byte)((decoded.Length >> 40) & 0xFF);
                encoded[5] = (byte)((decoded.Length >> 32) & 0xFF);
                encoded[6] = (byte)((decoded.Length >> 24) & 0xFF);
                encoded[7] = (byte)((decoded.Length >> 16) & 0xFF);
                encoded[8] = (byte)((decoded.Length >> 8) & 0xFF);
                encoded[9] = (byte)((decoded.Length) & 0xFF);

                indexStartRawData = 10;
            }

            // put raw data at the correct index
            for (int i = 0; i < decoded.Length; i++)
            {
                encoded[indexStartRawData + i] = decoded[i];
            }

            return encoded;
        }

        /*************************************************************************
           Implementa la decodifica della websocket 
        *************************************************************************/
        private byte[] DecodeData(byte[] encoded)
        {

            // Decodifica
            int bytesRec = encoded.Length;

            int second = encoded[1] & 127; // AND 0111 1111
            int maskIndex = 2;
            if (second < 126)
            {
                // length fit in second byte
                maskIndex = 2;
            }
            else if (second == 126)
            {
                // next 2 bytes contain length
                maskIndex = 4;
            }
            else if (second == 127)
            {
                // next 8 bytes contain length
                maskIndex = 10;
            }

            // get mask
            byte[] mask = { encoded[maskIndex], 
                            encoded[maskIndex+1], 
                            encoded[maskIndex+2], 
                            encoded[maskIndex+3]};
            int contentIndex = maskIndex + 4;

            // decode
            byte[] decoded = new byte[bytesRec - contentIndex];
            for (int i = contentIndex, k = 0; i < bytesRec; i++, k++)
            {

                // decoded = byte XOR mask
                decoded[k] = (byte)(encoded[i] ^ mask[k % 4]);
            }
            return decoded;
        }

        /*************************************************************************
           Termina l'esecuzione della classe 
        *************************************************************************/
        public void Close()
        {
            close = true;
            if (tcpListener != null)
                tcpListener.Stop();
        }

        /*************************************************************************
           Scandisce il topic del json
        
         {
	        "elements": {
		        "nodes": [{
			        "data": {
				        "id": "s1",
                        "object": "source",
                        "mode" : "bit_rate",
                        "refresh" : "all" "last"
			        }
		        },
                {
			        "data": {
				        "id": "s2",
                        "object": "source",
                        "mode" : "average"
			        }
		        }],
		        "topic": [{
			        "data": {
				        "type": "refresh"
			        }
		        }]
	        }
        }

        *************************************************************************/
        public bool ParseTopic(string json)
        {
            try
            {
                string jsonString = json;

                JObject root = JObject.Parse(jsonString);

                JArray items = (JArray)root["elements"]["topic"];

                JObject item;
                JToken jtoken;


                for (int i = 0; i < items.Count; i++)
                {
                    item = (JObject)items[i];
                    jtoken = item.First;

                    while (jtoken != null)
                    {

                        switch (((JProperty)jtoken).Value["type"].ToString())
                        {

                            case "refresh":
                                return true;
                        }

                        jtoken = jtoken.Next;
                    }
                }
                return false;
            }
            catch (Exception es)
            {

                // Error message
                Console.WriteLine("Error in json format: " + es.Message);
                return false;
            }
        }


        private string ToDateTime()
        {
            StringBuilder stringBuilder = new StringBuilder();
            DateTime dateTime = new DateTime();
            dateTime = DateTime.Now;

            stringBuilder.Append(dateTime.Month.ToString("00"));
            stringBuilder.Append("-");
            stringBuilder.Append(dateTime.Day.ToString("00"));
            stringBuilder.Append("-");
            stringBuilder.Append(dateTime.Year.ToString("00"));
            stringBuilder.Append(" ");

            stringBuilder.Append(dateTime.Hour.ToString("00"));
            stringBuilder.Append("-");
            stringBuilder.Append(dateTime.Minute.ToString("00"));
            stringBuilder.Append("-");
            stringBuilder.Append(dateTime.Second.ToString("00"));

            return stringBuilder.ToString();
        }


        		/// <summary>
		/// Parse the json file
		/// </summary>
		/// <param name="jsonString">Json string</param>
		/// <returns>True if the syntax is correct</returns>
		public bool CreateAnswer(string jsonString, ref string outString)
		{
            StringBuilder stringBuilder = new StringBuilder();

			try
			{

				JObject root = JObject.Parse(jsonString);

				JArray items = (JArray)root["elements"]["nodes"];

                stringBuilder.Append("{");
                stringBuilder.Append("\"elements\": {");
		        stringBuilder.Append("\"nodes\": {");
                stringBuilder.Append("\"data\": ");

				JObject item;
				JToken jtoken;


				for (int i = 0; i < items.Count; i++)
				{
					item = (JObject)items[i];
					jtoken = item.First;

					while (jtoken != null)
					{

                        string id = "";

                        switch (((JProperty)jtoken).Value["object"].ToString())
                        {

                            case "source":

                                try
                                {
                                    id = ((JProperty)jtoken).Value["id"].ToString();
                                    if (httpServer.jsonDoc.sources.ContainsKey(id))
                                    {
                                        stringBuilder.Append("{");
                                        stringBuilder.Append("\"id\": \"s1\",");
                                        stringBuilder.Append("\"value\": \"1.23\",");
                                        stringBuilder.Append("\"time\": \"" + ToDateTime() + "\"");
                                        stringBuilder.Append("},");
                                        stringBuilder.Append("{");
                                        stringBuilder.Append("\"id\": \"s1\",");
                                        stringBuilder.Append("\"value\": \"1.23\",");
                                        stringBuilder.Append("\"time\": \"" + ToDateTime() + "\"");
                                        stringBuilder.Append("}");
                                    }
                                }
                                catch (Exception e)
                                {
                                }
                                break;

                            case "destination":

                                try
                                {
                                    id = ((JProperty)jtoken).Value["id"].ToString();
                                    if (httpServer.jsonDoc.destinations.ContainsKey(id))
                                    {
                                        stringBuilder.Append("{");
                                        stringBuilder.Append("\"id\": \"d1\",");
                                        stringBuilder.Append("\"value\": \"1.23\",");
                                        stringBuilder.Append("\"time\": \"" + ToDateTime() + "\"");
                                        stringBuilder.Append("},");
                                        stringBuilder.Append("{");
                                        stringBuilder.Append("\"id\": \"d1\",");
                                        stringBuilder.Append("\"value\": \"1.23\",");
                                        stringBuilder.Append("\"time\": \"" + ToDateTime() + "\"");
                                        stringBuilder.Append("}");
                                    }
                                }
                                catch (Exception e)
                                {
                                }
                                break;

                            case "filter":

                                try
                                {
                                    id = ((JProperty)jtoken).Value["id"].ToString();
                                    if (httpServer.jsonDoc.filters.ContainsKey(id))
                                    {
                                        stringBuilder.Append("{");
                                        stringBuilder.Append("\"id\": \"f1\",");
                                        stringBuilder.Append("\"value\": \"1.23\",");
                                        stringBuilder.Append("\"time\": \"" + ToDateTime() + "\"");
                                        stringBuilder.Append("}");
                                        //stringBuilder.Append("\"id\": \"f1\",");
                                        //stringBuilder.Append("\"value\": \"1.23\",");
                                        //stringBuilder.Append("\"time\": \"" + ToDateTime() + "\"");
                                        //stringBuilder.Append("}");
                                    }
                                }
                                catch (Exception e)
                                {
                                }
                                break;

                            case "cull_time":

                                //id = ((JProperty)jtoken).Value["id"].ToString();
                                break;

                            case "cull_space":

                                //id = ((JProperty)jtoken).Value["id"].ToString();
                                break;

                            case "aggregate":

                                //id = ((JProperty)jtoken).Value["id"].ToString();
                                break;

                            case "triggerEvent":

                                //id = ((JProperty)jtoken).Value["id"].ToString();
                                break;

                            case "triggerAction":

                                //id = ((JProperty)jtoken).Value["id"].ToString();
                                break;

                            default:
                                break;
                        }

						jtoken = jtoken.Next;
					}
				}

				// Discover edges
				items = (JArray)root["elements"]["edges"];

				// Foreach egde
				for (int i = 0; i < 0; i++) //items.Count; i++)
				{
					item = (JObject)items[i];
					jtoken = item.First;

					try
					{

						while (jtoken != null)
						{

                            string id = ((JProperty)jtoken).Value["id"].ToString();
							jtoken = jtoken.Next;
						}
					}
					catch (Exception es)
					{
						Console.WriteLine(es.Message);
					}
				}

                //stringBuilder.Append("]");
                stringBuilder.Append("}");
	            stringBuilder.Append("}");
                stringBuilder.Append("}");

                outString = stringBuilder.ToString();
				return true;
			}
			catch (Exception es)
			{

				// Error message
				Console.WriteLine("Error in json format: " + es.Message);
				return false;
			}

            /* Return value
             * 
                 {
	                "elements": {
		                "nodes": [{
			                "data": [{
				                "id": "s1",
				                "value": "1.23",
				                "time": "gg-mm-aa-hh-mm-ss"
			                }, {
				                "id": "s1",
				                "value": "1.23",
				                "time": "gg-mm-aa-hh-mm-ss"
			                }]
		                }]
	                }
                }              
             */
        }
    }
}


