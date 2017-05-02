namespace Milyli.ScriptRunner.App_Start
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using Newtonsoft.Json;

    public class JsonBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            if (request.Headers["content-type"].Equals("application/json", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    var jsonSerializer = new JsonSerializer();
                    request.InputStream.Seek(0, SeekOrigin.Begin);
                    using (var streamReader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        return jsonSerializer.Deserialize(streamReader, modelType);
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}