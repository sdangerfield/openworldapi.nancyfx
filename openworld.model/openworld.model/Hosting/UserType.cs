using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;



namespace OpenWorld.Model.Hosting
{
    [Serializable()]
    public class UserType : owBaseDataType
{
         public UserType() : base() {
             this.Email = "all-logons@norwold.org";
             //Users = new List<hostUser>();
        }
 
        public UserType(string n,string nl):base(n,nl) {
            this.Email = "all-logons@norwold.org";
            //Users = new List<hostUser>();
        }
        

        public virtual string Email { get; set; }
        public virtual string Description { get; set; }
        // public virtual IList<hostUser> Users { get; set; }

 
    }
}
