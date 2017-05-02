namespace Milyli.ScriptRunner.App_Start
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Newtonsoft.Json;

    public class JsonBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            if (request.Headers["content-type"].Equals("application/json", StringComparison.InvariantCultureIgnoreCase) && request.Form.Count > 0)
            {
                try
                {
                    return JsonConvert.DeserializeObject(request.Form[0], modelType);
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