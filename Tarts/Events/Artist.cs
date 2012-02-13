using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tarts.Assets;
using Tarts.Base;

namespace Tarts.Events
{
    public class Artist : EntityBase
    {
        public virtual string Name { get; set; }
        public virtual string Slug { get; set; }
        public virtual Image Image { get; set; }
        public virtual string Bio { get; set; }


        public virtual void UpdateInfo(string name, string slug, string bio)
        {
            Name = name;
            Slug = slug;
            Bio = bio;
        }
        public virtual void SetImage(Image img)
        {
            Image = img;
        }
        
    }
}
