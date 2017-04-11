using System;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;


namespace Dcn
{
	public class HttpServer
	{
		private HttpListener listener;
		private int port;
		private bool close = false;
		public JsonDoc jsonDoc;

		/// <summary>
		/// Costruttore
		/// </summary>
		/// <param name="formMain">Riferimento al main form per il refresh dei controllori grafici</param>
		public HttpServer(int port)
		{

			// Safe the reference
			this.port = port;

			// Create a thread
			Thread thread = new Thread(new ThreadStart(StartListen));
			thread.IsBackground = true;
			thread.Start();
		}

		/// <summary>
		/// Listening 
		/// </summary>
		private void StartListen()
		{

			// Forever
			while (!close)
			{

				// Create a listener.
				listener = new HttpListener();
				listener.Prefixes.Add(string.Format("http://*:{0}/", port));
				listener.Start();

				// Listening
				Console.WriteLine("Listening...");

				// Note: The GetContext method blocks while waiting for a request. 
				HttpListenerContext context = listener.GetContext();
				HttpListenerRequest request = context.Request;
				StreamReader inputStream = new StreamReader(request.InputStream);

				// Json text
				string inputString = inputStream.ReadToEnd();

				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();

				// Create a new json manager
				jsonDoc = new JsonDoc(inputString);

				HttpListenerResponse response;
				string responseString;
				byte[] buffer;
				Stream output;

				// If the json format is correct
				if (!jsonDoc.ParseJson())
				{

					// Obtain a response object.
					response = context.Response;

					// Crete response string
					responseString = "Error in json format";

					buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

					// Get a response stream and write the response to it.
					response.StatusCode = 400;
					response.AppendHeader("Access-Control-Allow-Origin", "*");

					response.ContentLength64 = buffer.Length;
					output = response.OutputStream;
					output.Write(buffer, 0, buffer.Length);

					Console.WriteLine(responseString);

					// Purge and sleep
					output.Flush();
					Thread.Sleep(50);

					// Close the output stream.
					output.Close();

					// Stop listener
					listener.Stop();
					continue;
				}

				// If the conversion was done
				if (!jsonDoc.CreateDSN())
				{

					// Obtain a response object.
					response = context.Response;

					// Crete response string
					responseString = "Error during the creation of the DSN document";

					buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

					// Get a response stream and write the response to it.
					response.StatusCode = 400;
					response.AppendHeader("Access-Control-Allow-Origin", "*");

					response.ContentLength64 = buffer.Length;
					output = response.OutputStream;
					output.Write(buffer, 0, buffer.Length);

					Console.WriteLine(responseString);

					// Purge and sleep
					output.Flush();
					Thread.Sleep(50);

					// Close the output stream.
					output.Close();

					// Stop listener
					listener.Stop();
					continue;
				}

				// Obtain a response object.
				response = context.Response;

				// Construct a response. 
				StringBuilder stringBuilder = new StringBuilder();

				// Copy response
				foreach (string s in jsonDoc.document)
					stringBuilder.AppendLine(s);

				// Ora inizio elaborazione
				stopwatch.Stop();

				// Crete response string
				responseString = stringBuilder.ToString();
				buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

				// Get a response stream and write the response to it.
				response.StatusCode = 200;
				response.AppendHeader("Access-Control-Allow-Origin", "*");

				response.ContentLength64 = buffer.Length;
				output = response.OutputStream;
				output.Write(buffer, 0, buffer.Length);

				Console.WriteLine(responseString);


				string pathAbs = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string path = Path.Combine(pathAbs, "spark_translate/src/main/java/spark_translate/spark_script.java");
				File.WriteAllText(path, responseString);

				//// This text is added only once to the file.
				//if (!File.Exists(path))
				//{
				//	// Create a file to write to.
				//	File.WriteAllText(path, responseString);
				//}

				// Purge and sleep
				output.Flush();
				Thread.Sleep(50);

				// Close the output stream.
				output.Close();

				// Stop listener
				listener.Stop();
			}
		}

		public void Close()
		{
			close = true;
		}
	}
}
