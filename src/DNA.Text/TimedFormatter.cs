//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.dotnetage.com/home/en-US/the-mit-license-mit.html

using System;
using System.Threading;

namespace DNA.Text
{
    /// <summary>
    /// Present the formatter wrapper that prevent the Regex format dead lock.
    /// </summary>
    public class TimedFormatter : ITextFormatter
    {
        private ITextFormatter innerFormatter;
        private DateTime _started;
        private DateTime _stopped;

        public bool ThrowExceptionWhenTimeOut { get; set; }

        /// <summary>
        /// Write the trace message to trace context.
        /// </summary>
        public bool IsEnableTracing { get; set; }

        /// <summary>
        /// Gets/Sets the formatting time out in milliseconds
        /// </summary>
        public int TimeoutInterval { get; set; }

        public TimedFormatter(ITextFormatter formatter)
            : base()
        {
            TimeoutInterval = 2000;
            innerFormatter = formatter;
        }

        public virtual string Format(string text)
        {
            var formattedText = text;

            using (AutoResetEvent waitHandle = new AutoResetEvent(false))
            {
                Thread thread = new Thread(new ThreadStart(delegate()
                {
                    try
                    {
                        if (this.IsEnableTracing) _StartTrace();
                        formattedText = innerFormatter.Format(formattedText);
                        waitHandle.Set();
                    }
                    catch
                    {
                        //Trace.Warn(te.Message, te);
                    }
                }));

                thread.IsBackground = true;
                thread.Name = string.Format("{0} Thread", GetType().Name);
                thread.Start();

                bool timedOut = waitHandle.WaitOne(TimeoutInterval, false) == false;
                waitHandle.Close();

                if (timedOut)
                {
                    try
                    {
                        // Abort the regex thread.
                        thread.Abort();
                    }
                    catch 
                    {
                        //Trace.Warn(ce.Message, ce);
                    }
                    var timeOutException = new TimeoutException(string.Format("Timeout waiting for formatter: [{0}].", innerFormatter.GetType().ToString()));

                    if (IsEnableTracing)
                        _TimeOut(timeOutException);

                    if (ThrowExceptionWhenTimeOut)
                        throw timeOutException;
                }

            }
            if (IsEnableTracing) _EndTrace();
            return formattedText;
        }

        private void _TimeOut(Exception e)
        {
            //Trace.Write("--------------Format timeout--------------");
            //Trace.Write(e.Message);
        }

        private void _StartTrace()
        {
            //_started = DateTime.UtcNow;
            //Trace.Write("--------------Formatting start--------------");
        }

        private void _EndTrace()
        {
            _stopped = DateTime.UtcNow;
            var interval = _stopped - _started;
            //Trace.Write("-------------Format complete--------------");
            //Trace.Write(string.Format("Time using:{0} milliseconds", interval.TotalMilliseconds));
        }
    }
}
