using System;
using System.Web;

namespace Milyli.ScriptRunner.Web
{
	public class WebSocketHanlder : IHttpHandler
	{

		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			if(context.IsWebSocketRequest)
			{
				this.CreateWebSocketServer();
			}		
		}
	}
}
