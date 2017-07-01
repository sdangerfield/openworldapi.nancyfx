using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace OpenWorld.Model.Hosting { 

    public class UserEvent : owBaseDataType
    {

        public UserEvent() {
            if (this.DateOccured == DateTime.MinValue || this.DateOccured ==null ) { this.DateOccured = owBaseConst.minSQLDate; }; 
        }

        public UserEvent(string n, string nl)
            : this()
        {
            this.Name = n.Substring(0, Math.Min(owBaseConst.LEN_SHORT , n.Length ));
            this.LongName = nl.Substring(0, Math.Min(owBaseConst.LEN_LONG , nl.Length ));
        }
        
        /** refactor
        public UserEvent(string n, string nl,User usr, hostChar ch, campCampaign camp,string desc)
            : this(n,nl)
        {
            this.Character = ch;
            this.Campaign = camp;
            this.Description = desc;
            this.User = usr;
        }
        
       
        public virtual hostChar Character { get; set; }
        public virtual campCampaign Campaign { get; set; }
        **/
    
        public virtual User User { get; set; }
        public virtual string Description { get; set; }
        public virtual bool Viewed { get; set; }
        public virtual bool Sent { get; set; }
        public virtual DateTime DateOccured { get; set; }

    }
}
