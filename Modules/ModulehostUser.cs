namespace OpenWorldAPI.nancyfx.Modules
{
    using Nancy;

    using System.Collections.Generic;
    using OpenWorld.Model.Hosting;
    using NHibernate;

    public class ModulehostUser : NancyModule
    {

        private readonly ISession _session;
        protected IList<User> hostUserList;
        protected IList<UserType> hostUserTypeList;


        public ModulehostUser(ISession session)
        {
            
            this._session = session;
            hostUserList = session.QueryOver<User>().List();


        Get["/hostuser"] = parameters => { return Response.AsJson(hostUserList); };
        }

    }
}