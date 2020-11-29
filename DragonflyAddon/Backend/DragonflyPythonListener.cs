using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using VoiceControl;

namespace DragonflyAddon
{
    public class DragonflyPythonListener : IDisposable
    {
        private readonly string file;
        private readonly ILog log;

        public event Action<string> OnRecognition = x =>{};
        Process process;

        public DragonflyPythonListener(string file, ILog log)
        {
            this.file = file;
            this.log = log;
        }

        public void RegisterGrammar()
        {
            ExecutePythonCommand(@"C:\Python27\python.exe", $"-u -m dragonfly load --engine natlink {Path.GetFileName(file)}", file);
        }

        private void ListenToStream(StreamReader stream)
        {
            
            log.RunTask(() =>
            {
                while (!process.HasExited)
                {
                    var result = stream.ReadLine();
                    if (result == null) return;
                    log.RunTask(() =>
                    {
                        HandleRecognition(result);
                    });
                }
            });
        }
        private void StartListening()
        {
            ListenToStream(output);
            ListenToStream(error);
        }


        public void ExecutePythonCommand(string pythonPath, string command, string file)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = pythonPath;
            start.WorkingDirectory = Path.GetDirectoryName(file);
            start.Arguments = command;
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.RedirectStandardInput = true;

            process = Process.Start(start);

            output = process.StandardOutput;
            error = process.StandardError;
            process.StandardInput.WriteLine("\x3");
            process.StandardInput.Flush();
            StartListening();
        }

        StreamReader output;
        StreamReader error;


        public void HandleRecognition(string result)
        {
            Console.WriteLine(result);

            var match = Regex.Match(result, @"(\w+):(.+)");
            if (match.Success)
            {

                var type = match.Groups[1].Value;
                var message = match.Groups[2].Value;
                switch (type)
                {
                    case "Recognized":
                        OnRecognition(message.Trim());
                        break;
                    case "ERROR":
                        log.Error(message);
                        break;
                    default:
                        //log.Info(message);
                        break;
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: verwalteten Zustand (verwaltete Objekte) entsorgen.
                }
                
                if (!process.HasExited)
                {
                    error.Close();
                    output.Close();
                    process.Kill();
                }
                // TODO: nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer weiter unten überschreiben.
                // TODO: große Felder auf Null setzen.

                
                disposedValue = true;
            }
        }

        //TODO: Finalizer nur überschreiben, wenn Dispose(bool disposing) weiter oben Code für die Freigabe nicht verwalteter Ressourcen enthält.
        ~DragonflyPythonListener()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
            Dispose(false);
        }

        // Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
            Dispose(true);
            // TODO: Auskommentierung der folgenden Zeile aufheben, wenn der Finalizer weiter oben überschrieben wird.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}