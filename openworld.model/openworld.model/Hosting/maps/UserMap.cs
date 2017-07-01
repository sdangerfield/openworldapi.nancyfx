using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;




namespace OpenWorld.Model.Hosting.maps {
    
    
    public class UserMap : ClassMap<User> {

        public UserMap()
        {
			Table("host_users");
			LazyLoad();
			Id(x => x.id).GeneratedBy.Identity().Column("id");
            Map(x => x.Name).Column("Name").Length(owBaseConst.LEN_SHORT);
            Map(x => x.LongName).Column("LongName").Length(owBaseConst.LEN_LONG).UniqueKey("hostUser_UK_LongName");
			Map(x => x.Email).Column("Email");
            Map(x => x.Password).Column("Password");
            Map(x => x.fbID);
            Map(x => x.fbGender);
            Map(x => x.fbAccessToken);
            Map(x => x.fbFirstName);
            Map(x => x.fbSurname);
            Map(x => x.fbTokenExpires);

            References(x => x.UserType).Column("hostUserType_id");
            //HasMany(x => x.Characters).Cascade.All();
            //HasMany(x => x.GMCampaigns).Cascade.All();
            //HasMany(x => x.Events).Cascade.All();

        }
    }
}
