using System;
using System.Collections.Generic;
using Bronson.Utils;
using Tarts.Assets;
using Tarts.Base;
using Tarts.Events;

namespace Tarts.Content
{
    public class Gallery : EntityBase
    {
        public virtual string Name { get; set; }
        public virtual string Slug { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual IList<Image> Photos { get; set; }
        public virtual Image DefaultImage { get; set; }
        public virtual Event Event { get; set; }

        public virtual int DefaultImageID { get { return (DefaultImage != null) ? DefaultImage.ID : 0; } }
        public virtual int EventID { get { return (Event != null) ? Event.ID : 0; } }

        public Gallery()
        {
            Photos = new List<Image>();
        }
        public Gallery(string name, string slug, DateTime date)
        {
            Name = name;
            Slug = slug;
            Date = date;
            Photos = new List<Image>();
        }


        public virtual ReturnValue Update(string name, string slug, DateTime date, Event takenAt)
        {
            if (string.IsNullOrWhiteSpace(name)) return new ReturnValue(false, "Please provide a gallery name");
            Name = name;
            Slug = slug;
            Date = date;
            Event = takenAt;
            return new ReturnValue();
        }

        public virtual void SetDefaultImage(Image img)
        {
            AddPhoto(img);
            DefaultImage = img;
        }

        public virtual void AddPhoto(Image img)
        {
            if (! Photos.Contains(img))
                Photos.Add(img);
        }

        public virtual void RemovePhoto(Image img)
        {
            if (Photos.Contains(img))
                Photos.Remove(img);
            if (DefaultImage == img)
                DefaultImage = (Photos.Count > 0) ? Photos[0] : null;
        }
    }
}
