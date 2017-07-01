using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using System.Diagnostics;
using System.Reflection;


using OpenWorld.Model.Hosting;




namespace OpenWorld.Model

{
    public class owBaseRepository
    {
        private ISession i;
        public owBaseRepository(ISession sess) { this.i = sess; }

        public virtual owBaseDataType FindbyID(Type t, int id)
        {
            return i.Get<owBaseDataType>(id);
        }

        public virtual owBaseDataType Find(Type t, string nl)
        {
            int r = FindIDByLongName(t, nl);
            return i.Get(t, r) as owBaseDataType;
        }

        public virtual owBaseDataType FindOrCreate(Type t, string nshort, string nl)
        {
            Object o;

            int r = FindIDByLongName(t, nl);
            if (r == 0)
            {
                ConstructorInfo ci = t.GetConstructor(new Type[] { typeof(string), typeof(string) });
                o = ci.Invoke(new object[] { nshort, nl }) as owBaseDataType;
                Add(o as owBaseDataType);
                return o as owBaseDataType;
            }
            else
            {
                return i.Get(t, r) as owBaseDataType;
            }
        }

        public virtual bool SeedMockData()
        {
           //NEW FEDERATED Initialisations

            //libMapping Create Base;
            //this.CreateLibMapping();

            //libHosting Create Base
            this.CreateLibHosting();

            //libRuleSet Create Base
            //this.CreateLibRuleset();

            //libRuleSet Create Base
            //this.CreateLibCampaign();

            /**
            try
            {
                //Seed Characters
                User steve = this.Find((typeof(User)), "unclemessy") as User;
                hostUser lewis = this.Find((typeof(User)), "lewis") as User;
                hostUser matt = this.Find((typeof(User)), "matsly") as User;

                if (FindIDByLongName(typeof(hostChar), "Messy da Anti Hero") == 0)
                {
                    hostChar c = new hostChar("Messy", "Messy da Anti Hero", steve);
                    c.RefreshCharacterStats(i);

                    //Chuck in some items
                    nwItem i1 = new nwItem("+1 Dagger", "+1 Dagger");
                    i1.Owner = c;
                    c.Items.Add(i1);
                    nwItem i2 = new nwItem("+4 LongBow", "+4 LongBow");
                    i2.Description = "This is a mega bow created by the elven lords";
                    i2.Owner = c;
                    c.Items.Add(i2);
                    Add(c);

                    //i.SaveOrUpdate(new campCharCampaign(traitPBEM, c, false));
                    //traitPBEM.Characters.Add(c);
                    //i.SaveOrUpdate(c);
                }
                if (FindIDByLongName(typeof(hostChar), "Sabbath") == 0)
                {
                    hostChar c = new hostChar("Sabbath", "Sabbath", matt);
                    c.RefreshCharacterStats(i);

                    //Chuck in some items
                    nwItem i1 = new nwItem("+5 Vorpal", "+5 Vorpal Sword");
                    i1.Owner = c;
                    c.Items.Add(i1);
                    Add(c);
                }

                if (FindIDByLongName(typeof(hostChar), "Muttley") == 0)
                {
                    hostChar c = new hostChar("Muttley", "Muttley", lewis);
                    c.RefreshCharacterStats(i);
                    this.Add(c);
                    //i.SaveOrUpdate(new campCharCampaign(traitPBEM, c, false));
                    //i.SaveOrUpdate(c);
                }


                this.Save(steve);
                this.Save(matt);
                //this.Save(traitPBEM);


                return true;
            }
            catch (GenericADOException ex)
            {
                var sql = ex.InnerException as SqlException;
                if (sql != null)
                {
                    Debug.Print("Caught GenericADO:" + ex.ToString());
                    Debug.Print("Caught Inner:" + sql.ToString());
                    Debug.Print("SQL ErrorNumber:" + sql.Number.ToString());
                    throw;
                }
                return false;
            }
            **/
            return true;
        }



        public virtual int FindIDByLongName(Type t, string n)
        {
            //Debug.Print(t.Name);
            var r = i.CreateCriteria(t.Name)
                  .Add(Restrictions.Eq("LongName", n))
                  .UniqueResult<owBaseDataType>();
            if (r == null)
            {
                return 0;
            }
            else
            {
                return r.id;
            }

        }

        public virtual bool DeleteByLongName(Type t, string n)
        {
            owBaseDataType nw = Find(t, n);
            if (nw == null)
            {
                // Not Found
                return false;
            }
            else
            {
                Delete(nw);
                return false;
            }
        }

        public void Delete(owBaseDataType nw)
        {
            Debug.Print("Delete called on:" + nw.GetType() + "param:" + nw);
            if (nw == null) { return; };
            using (ITransaction transaction = i.BeginTransaction())
            {
                i.Delete(nw);
                transaction.Commit();
            };
        }
        public void Add2(owBaseDataType nw)
        {

        }

        public void Add(owBaseDataType nw)
        {
            Debug.Print("Add called on:" + nw.GetType() + "params:" + nw.ToString());
            using (ITransaction transaction = i.BeginTransaction())
            {
                i.SaveOrUpdate(nw);
                transaction.Commit();
            }

        }


        public void Save(owBaseDataType nw)
        {
            Debug.Print("Save called on:" + nw.GetType() + "params:" + nw.ToString());
            using (ITransaction transaction = i.BeginTransaction())
            {
                i.SaveOrUpdate(nw);
                transaction.Commit();
            }

        }
        public virtual string GetTableName(Type t)
        {
            var u = i.SessionFactory.GetClassMetadata(t.Name) as NHibernate.Persister.Entity.AbstractEntityPersister;
            return u.TableName;
        }

        public ISession GetSession()
        {
            return i;
        }

        public virtual IList<owBaseDataType> asList(Type t)
        {

            return i.CreateCriteria(t.Name)
                    .List<owBaseDataType>();
        }


        

        public virtual void CreateHostUsers(IList<UserType> usertypelist)
        {

            User steve, matt, lewis;
            UserType players, gms, admins;
            admins = usertypelist.Where(x => x.LongName == Constants.usrSysAdmin).First();
            gms = usertypelist.Where(x => x.LongName == Constants.usrRulesetAdmin).First();
            players = usertypelist.Where(x => x.LongName == Constants.usrPlayer).First();


            //seed users
            steve = new User("unclemessy", "unclemessy", "stephen_dangerfield@yahoo.co.uk", "muppet");
            steve.fbID = "10152572366130895";
            steve.fbAccessToken = "CAANSyHyJq9gBAFYZB2qpnJkZAEsicsnF0kYa7qPIoUZARqGZBhgXpNAZCRN9MplrL6ZCRXRMAUi5AV54NTi8CA9TI6aamXb7HvZAyqMv8Uw4mSNPt6Xtg6TIY3X3YqlvJeYHZC1laqeJUfvSFgZCuuECTmZCv3b2OefSOZBwl1xJC69H5s1Baj9lNMIWwPBbxFt1S2i36jTnUsBQwAm2fM9K0wJ";
            steve.fbGender = "male";
            steve.fbFirstName = "Steve";
            steve.fbSurname = "Dangerfield";
            //steve.CreateCampaign("Legends", "Land of Legends", "dangerfield.stephen@yahoo.com.au", "A Dark and nasty underworld Land plagued by the Mists of Krondell and the spawns of Chortos");


            matt = new User("matsly", "matsly", "matsilvester@hotmail.co.uk", "muppet");
            matt.fbID = "10152966328743429";
            matt.fbAccessToken = "CAANSyHyJq9gBAL332yEjJrLTZA1GH2rAAn9ZBTy9hXyIvaNkQJdNZBZCOe6y6pZCMeQ0VW0ARq0awxbFsla23lVKdZCpte4CvAR3690Y6X8UDcQJmhF25swYdJmeOUQvfFUEomAVWVcDYXrieywtFzRhrw3MdZAHjQXeKCuZCAH84MGfvU0Qo6pgbylvk09E6W3RnFWdhF45gtc1cFAIMgmI";
            matt.fbGender = "male";
            matt.fbFirstName = "Matthew";
            matt.fbSurname = "Silvester";
            //matt.CreateCampaign("traitPBEM", "Traits Play by Email", "dangerfield.stephen@yahoo.com.au", "This is a PBEM game for the Drowe Underworld");

            lewis = new User("lewis", "lewis", "lewis@hotmail.com", "muppet");

            //Add to correct host usertypelist after creation

            steve.UserType = admins;
            matt.UserType = gms;
            lewis.UserType = players;

            this.Save(steve);
            this.Save(matt);
            this.Save(lewis);

        }


        public virtual void CreateLibHosting()
        {

            //Creates initial data for libHosting

            //hostUserTypes
            this.Add(new UserType("Admin", Constants.usrSysAdmin));
            this.Add(new UserType("Player", Constants.usrPlayer));
            this.Add(new UserType("RuleAdmin", Constants.usrRulesetAdmin));
            this.Add(new UserType("Unreg", Constants.usrUnregistered));
            IList<UserType> huTypes = this.i.QueryOver<UserType>().List();
 
            CreateHostUsers(huTypes);
            //users are presisted by being added to their types.


        }

            /**
        public virtual void CreateLibCampaign()
        {
            //Creates initial data for the defalt d20 Campaign
            //need Milestonetypes for Campaign Milestone tree
            IList<sysMilestoneType> sysMileStoneTypes = i.QueryOver<sysMilestoneType>().List();

            //nwMileStoneTree
            campTreeMileStone milestoneTree = campTreeMileStone.SeedTreeData(sysMileStoneTypes);
            i.SaveOrUpdate(milestoneTree);
            
            //campTreeFaction
            campTreeFaction factionTree = campTreeFaction.SeedTreeData();
            i.SaveOrUpdate(factionTree);

            //campTreeFaction
            campTreeLocation locationTree = campTreeLocation.SeedTreeData();
            i.SaveOrUpdate(locationTree);


        }

        public virtual void CreateLibMapping()
        {
            //Creates initial data for the default Medieaveal Mapset

            mapTreeTerrain mt = new mapTreeTerrain("Terrain Type Tree", 0);
            mapTileSetManager.InitmapTerrainTree(mt);
            i.Save(mt);

            //Create defaults using existing mTree
            mapTileSetManager ttm = new mapTileSetManager(i);
            ttm.Init(mt,null);

            foreach (mapTerrainType st in ttm.TerrainTypes) { this.Add(st); }
            foreach (mapPOIType sp in ttm.POITypes) { this.Add(sp); }

        }
        **/
    }

}
