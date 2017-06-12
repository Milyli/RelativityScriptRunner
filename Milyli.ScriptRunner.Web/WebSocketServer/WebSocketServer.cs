using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Web;

namespace Milyli.ScriptRunner.Web.WebSocketServer
{
	public class WebSocketServer : IDisposable
	{
		private WebSocket webSocket;
		private bool disposedValue = false; // To detect redundant calls

		public WebSocketServer(WebSocket webSocket)
		{
			this.webSocket = webSocket;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this.webSocket.Dispose();
				}

				this.disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}