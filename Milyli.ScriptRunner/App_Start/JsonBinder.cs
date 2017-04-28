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
            var json = request.Form.Get("json");
            if (json == null)
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject(json, modelType);
            }
            catch
            {
                return null;
            }
        }
    }
}