using System;
using System.Collections.Generic;
using Brandy.Grapes.FluentNHibernate;
using Brandy.Grapes;


namespace OpenWorld.Model

{
    /**
     * Core Tabling structure, rather than traditional linked groups/categories, this is linked trees
     * The core branches of the tree are the traditional groups, then (optional) subgroups and (optional) subsubgroups
     * branches by definition are XTypes of X (foreign keys)
     * 
     * Deleting tree branches is bad and can have massive effect on database (e.g. delete everythinig that depends on them)
     * Changing trees (swapping) is almost as bad with same implications.
     * 
     * Implicit in this is the linked subclass where the tree is the master(menu) structure and the subclass the detail
     * For this we use the following nomenclature
     * 
     * Tier       Name            ItemTypeExample
     * Root       Root            ItemTypes <<not part of tree>>
     * Branch1:   Class           Equipment, Resources
     * Branch2:   SubClass        Weapons, Minerals
     * Branch3:   TertiaryClass   Polearms , <<resources are 2 tiers hence branch3 ==ItemType for resources>>
     * Branch4:   Quartary        <<not implemented yet, branch4 i== ItemType for Equipment>>
     * 
     * 
     * */
    
    public abstract class owBaseTree : TreeEntry<owBaseTree> , IComparable
        {
            public virtual int Id { get; set; }
            public virtual string Name { get; set; }
            public virtual int Value { get; set; }
            //public virtual int OrderDisplay { get; set; }
            //public virtual int OrderPrint { get; set; }
            
            //This is an abstracted reference to any LeafNodes [TODO]
            public virtual IList<owBaseDataType> LeafNodes { get; protected set; }

            public virtual int? ParentRecordID
            {
                get
                {
                    if (Parent == null)
                    {
                        return null;
                    }
                    else
                    {
                        return Parent.Id;
                    }
                }
            }
            public virtual int CompareTo(object obj)
            {
                if (obj == null) return 1;
                owBaseTree otherSysTree = obj as owBaseTree;
                if (otherSysTree != null)
                {
                    return Id.CompareTo(otherSysTree.Id);
                }
                else
                    throw new ApplicationException("Compare to called on" + this.GetType() + "ID=" + this.Id + " Comparing to non nwdbTree");
            }


            public owBaseTree()
            {
                
            }


            public owBaseTree(string n, int v)
            {
                this.Name = n;
                this.Value = v;
            }

            public virtual owBaseTree FindfromRoot(owBaseTree root, string match)
            {
                //Performance on this is such an utter dog - only use in setups etc
                foreach (owBaseTree st in root.Descendants)
                {
                    if (st.Name == match) { return st;  }
                }
                return null;
            }


            public virtual String Path(string separator)
            {
                owBaseTree node;
                string res;

                res = "";
                //Push Parents
                Stack<owBaseTree> ancestor = new Stack<owBaseTree>();
                ancestor.Push(this);
                node = this.Parent;
                while (node != null) {
                    ancestor.Push(node);
                    node = node.Parent;
                }
                //Pop Path
                while (ancestor.Count>0) {
                    node = ancestor.Pop();
                    res = res + separator + node.Name;
                }
                return res;
            }

            public virtual int Depth()
            {
                int r =0;
                owBaseTree node = this;
                while (node.Parent != null)
                {
                    r++;
                    node = Parent;
                }
                return r;
            }

       }



 
}

