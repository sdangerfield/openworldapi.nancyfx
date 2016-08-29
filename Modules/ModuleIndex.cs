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
        }
    }
}