namespace Phantom.Core.Builtins {
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using Boo.Lang;

    public class Exec {
        //private StreamReader _stdError;
        private StreamReader _stdOut;
        private TextWriter _outputWriter;
        //private TextWriter _errorWriter;
        private string _exeName;

        /// <summary>
        /// Will be used to ensure thread-safe operations.
        /// </summary>
        private object _lockObject = new object();
        
        public virtual FileInfo Output { get; set; }

        /// <summary>
        /// Gets a value indicating whether output will be appended to the 
        /// <see cref="Output" />.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if output should be appended to the <see cref="Output" />; 
        /// otherwise, <see langword="false" />.
        /// </value>
        public virtual bool OutputAppend
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the <see cref="TextWriter" /> to which standard output
        /// messages of the external program will be written.
        /// </summary>
        /// <value>
        /// The <see cref="TextWriter" /> to which standard output messages of 
        /// the external program will be written.
        /// </value>
        /// <remarks>
        /// By default, standard output messages wil be written to the build log
        /// with level <see cref="Level.Info" />.
        /// </remarks>
        public virtual TextWriter OutputWriter
        {
            get
            {
                //if (_outputWriter == null)
                //{
                //    _outputWriter = new TextWriter();
                //    _outputWriter = new LogWriter(this, Level.Info,
                //        CultureInfo.InvariantCulture);
                //}
                return _outputWriter;
            }
            set { _outputWriter = value; }
        }

        /// <summary>
        /// The name of the executable that should be used to launch the 
        /// external program.
        /// </summary>
        /// <value>
        /// The name of the executable that should be used to launch the external
        /// program, or <see langword="null" /> if no name is specified.
        /// </value>
        /// <remarks>
        /// If available, the configured value in the NAnt configuration
        /// file will be used if no name is specified.
        /// </remarks>
        public virtual string ExeName
        {
            get { return _exeName; }
            set { _exeName = value; }
        }

        public void Execute(string args, Hash options) {
            Thread outputThread = null;

            string workingDir = options.ObtainAndRemove("WorkingDir", ".");
            bool ignoreNonZeroExitCode = options.ObtainAndRemove("IgnoreNonZeroExitCode", false);
            var psi = new ProcessStartInfo(_exeName, args)
            {
                WorkingDirectory = workingDir,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = (Output != null) ? true : false
            };
            var process = Process.Start(psi);

            if (Output != null) {
                outputThread = new Thread(StreamReaderThread_Output);
                _stdOut = process.StandardOutput;
                outputThread.Start();
            }

            process.WaitForExit();

            if (Output != null) {
                outputThread.Join(2000);
            }

            var exitCode = process.ExitCode;

            if (exitCode != 0 && ignoreNonZeroExitCode == false)
            {
                var errortext = process.StandardError.ReadAllAsString();
                throw new ExecutionFailedException(exitCode, errortext);
            }
        }


        /// <summary>
        /// Reads from the stream until the external program is ended.
        /// </summary>
        private void StreamReaderThread_Output()
        {
            StreamReader reader = _stdOut;
            bool doAppend = false;// OutputAppend;

            while (true)
            {
                string logContents = reader.ReadLine();
                if (logContents == null)
                {
                    break;
                }

                // ensure only one thread writes to the log at any time
                lock (_lockObject)
                {
                    if (Output != null)
                    {
                        StreamWriter writer = new StreamWriter(Output.FullName, doAppend);
                        writer.WriteLine(logContents);
                        doAppend = true;
                        writer.Close();
                    }
                    else
                    {
                        //OutputWriter.WriteLine(logContents);
                    }
                }
            }

            //lock (_lockObject)
            //{
            //    OutputWriter.Flush();
            //}
        }
    }
}
