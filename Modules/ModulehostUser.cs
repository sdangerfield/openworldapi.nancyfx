namespace OpenWorldAPI.nancyfx.Modules
{
    using Nancy;
    using System.Collections.Generic;
    using DataModel.libHosting;
    using NHibernate;

    public class ModulehostUser : NancyModule
    {

        private readonly ISession _session;
        protected IList<hostUser> hostUserList;
        protected IList<hostUserType> hostUserTypeList;

        public ModulehostUser(ISession session)
        {
            
            this._session = session;
            hostUserList = session.QueryOver<hostUser>().List();
            Get["/hostuser"] = parameters => { return Response.AsJson(hostUserList); };

        }

    }
}