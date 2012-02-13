using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tarts.Base;
using Tarts.Content;

namespace Tarts.Assets
{
    public class Image : EntityBase
    {
        public virtual string Name { get; set; }
        public virtual string Large { get; set; }
        public virtual string Medium { get; set; }
        public virtual string Thumb { get; set; }
        public virtual DateTime Created { get; set; }

        public virtual IList<Gallery> Galleries { get; set; }


        public Image(){}
        public Image(string name, string large, string medium, string thumb)
        {
            Name = name;
            Large = large;
            Medium = medium;
            Thumb = thumb;
            Created = DateTime.Now;
        }


    }
}
