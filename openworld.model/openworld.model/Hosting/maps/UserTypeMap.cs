using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using OpenWorld.Model;


namespace OpenWorld.Model.Hosting.maps {
    
    
    public class UserTypeMap : ClassMap<UserType> {

        public UserTypeMap()
        {
			Table("host_usertypes");
			LazyLoad();
			Id(x => x.id).GeneratedBy.Identity().Column("id");
            Map(x => x.Name).Column("Name").Length(owBaseConst.LEN_SHORT);
            Map(x => x.LongName).Column("LongName").Length(owBaseConst.LEN_LONG).UniqueKey("hostUserType_LongName");
			Map(x => x.Email).Column("Email");
			Map(x => x.Description).Column("description").CustomType("StringClob").CustomSqlType("nvarchar(max)");

            
            //HasMany(x => x.Users).Inverse().Cascade.All(); //deleting a character implicitly deletes his items...consider????


;
        }
    }
}
