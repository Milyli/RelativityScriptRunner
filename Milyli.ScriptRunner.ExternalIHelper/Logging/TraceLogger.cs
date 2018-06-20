namespace Milyli.ScriptRunner.ExternalIHelper.Logging
{
	using System;
	using Relativity.API;

	public class TraceLogger : IAPILog
	{
		public IAPILog ForContext<T>()
		{
			return this;
		}

		public IAPILog ForContext(Type source)
		{
			return this;
		}

		public IAPILog ForContext(string propertyName, object value, bool destructureObjects)
		{
			return this;
		}

		public IDisposable LogContextPushProperty(string propertyName, object obj)
		{
			return new NullDisposable();
		}

		public void LogDebug(string messageTemplate, params object[] propertyValues)
			=> WriteMessage(messageTemplate, propertyValues);

		public void LogDebug(Exception exception, string messageTemplate, params object[] propertyValues)
			=> WriteMessage(messageTemplate, propertyValues, exception);

		public void LogError(string messageTemplate, params object[] propertyValues)
			=> WriteMessage(messageTemplate, propertyValues);

		public void LogError(Exception exception, string messageTemplate, params object[] propertyValues)
			=> WriteMessage(messageTemplate, propertyValues, exception);

		public void LogFatal(string messageTemplate, params object[] propertyValues)
			=> WriteMessage(messageTemplate, propertyValues);

		public void LogFatal(Exception exception, string messageTemplate, params object[] propertyValues)
			=> WriteMessage(messageTemplate, propertyValues, exception);

		public void LogInformation(string messageTemplate, params object[] propertyValues)
			=> WriteMessage(messageTemplate, propertyValues);

		public void LogInformation(Exception exception, string messageTemplate, params object[] propertyValues)
			=> WriteMessage(messageTemplate, propertyValues, exception);

		public void LogVerbose(string messageTemplate, params object[] propertyValues)
			=> WriteMessage(messageTemplate, propertyValues);

		public void LogVerbose(Exception exception, string messageTemplate, params object[] propertyValues)
			=> WriteMessage(messageTemplate, propertyValues, exception);

		public void LogWarning(string messageTemplate, params object[] propertyValues)
			=> WriteMessage(messageTemplate, propertyValues);

		public void LogWarning(Exception exception, string messageTemplate, params object[] propertyValues)
			=> WriteMessage(messageTemplate, propertyValues, exception);

		private static void WriteMessage(string messageTemplate, object[] propertyValues, Exception exception = null)
		{
			var message = string.Format(messageTemplate, propertyValues);
			if (exception != null)
			{
				message += Environment.NewLine + exception.ToString();
			}

			System.Diagnostics.Trace.Write(message);
		}

		private sealed class NullDisposable : IDisposable
		{
			public void Dispose()
			{
			}
		}
	}
}
