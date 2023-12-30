using System.Globalization;
using System.IO;
using System.Text;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace msfs_server.Helpers
{
    public class MySink(ITextFormatter formatter) : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            var buffer = new StringWriter(new StringBuilder(1024));
            formatter.Format(logEvent, buffer);
            //var message = buffer.ToString();

            // do stuff
        }
    }

    public static class MySinkExtensions
    {
        public static LoggerConfiguration MySink(
            this LoggerSinkConfiguration loggerConfiguration,
            string outputTemplate
        )
        {
            Serilog.Formatting.Display.MessageTemplateTextFormatter tf = new(outputTemplate, CultureInfo.InvariantCulture);

            return loggerConfiguration.Sink(new MySink(tf));
        }
    }
}
