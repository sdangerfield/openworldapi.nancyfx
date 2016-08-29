using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Serialization.JsonNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHibernate.Proxy;

namespace OpenworldAPI.nancyfx.siteUtils
{
    public class NHibernateContractResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            if (typeof(NHibernate.Proxy.INHibernateProxy).IsAssignableFrom(objectType))
                return base.CreateContract(objectType.BaseType);

            return base.CreateContract(objectType);
        }
    }
    
    public class CustomJsonSerializer : JsonSerializer
    {
        public CustomJsonSerializer()
        {
            this.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            this.ContractResolver = new NHibernateContractResolver();
            this.Formatting = Formatting.Indented;
        }
    }
}