namespace OpenWorldAPI.nancyfx.Modules
{
    using Nancy;

    public class ModuleIndex : NancyModule
    {
        public ModuleIndex()
        {
            Get["/"] = parameters => {
                return View["index"];
            };

            Get["/swagger-ui"] = parameters =>
            {
                var url = $"{Request.Url.BasePath}/api-docs";
                return View["doc", url];
            };
        }
    }
}