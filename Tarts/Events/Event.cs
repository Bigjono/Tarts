using System;
using System.Collections.Generic;
using Bronson.Utils;
using Tarts.Base;
using Tarts.Bookings;
using Tarts.Content;

namespace Tarts.Events
{
    public class Event : EntityBase
    {
        public virtual string Name { get; set; }
        public virtual string Slug { get; set; }
        public virtual string Description { get; set; }      
        public virtual string BookingConfirmation { get; set; }      
        public virtual DateTime StartTime { get; set; }        
        public virtual DateTime EndTime { get; set; }
        public virtual int TotalAllocation { get; set; }
        public virtual int TotalSold { get; set; }
        public virtual bool Cancelled { get; set; }

        public virtual IList<Artist> Lineup { get; set; }
        public virtual IList<Gallery> Galleries { get; set; }
        public virtual IList<Booking> Bookings { get; set; }
        public virtual IList<Ticket> Tickets { get; set; }

        public virtual string Status { 
            get
            {
                if (Cancelled) return "Cancelled";
                if (StartTime.ToStartOfDay() == DateTime.Now.ToStartOfDay()) return "Today";
                if (StartTime.AddDays(-7).ToStartOfDay() <= DateTime.Now.ToStartOfDay()) return "This Week";
                if (StartTime.AddDays(-28).ToStartOfDay() <= DateTime.Now.ToStartOfDay()) return "This Month";
                if (StartTime.ToStartOfDay() > DateTime.Now.ToStartOfDay()) return "Live";
                return "Ended";
            }
        }    

        public Event() {}
        public Event(string name, string slug, DateTime startTime, DateTime endTime)
        {
            Name = name;
            Slug = slug;
            StartTime = startTime;
            EndTime = endTime;
            Lineup = new List<Artist>();
            Galleries = new List<Gallery>();
            Bookings = new List<Booking>();
            Tickets = new List<Ticket>();
        }

        public virtual ReturnValue Update(string name, string slug, string description, DateTime startTime, DateTime endTime, string bookingConfirmation)
        {
            if(endTime < startTime) return new ReturnValue(false, "End time cannot be less that start time");
            if(startTime < DateTime.Now) return new ReturnValue(false, "start time cannot be in the past");

            Name = name;
            Slug = slug;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            BookingConfirmation = bookingConfirmation;

            return new ReturnValue();
        }

        public virtual void AddArtist(Artist artist)
        {
            if(! Lineup.Contains(artist))
                Lineup.Add(artist);
        }
        public virtual void RemoveArtist(Artist artist)
        {
            if (Lineup.Contains(artist))
                Lineup.Remove(artist);
        }
        public virtual void RemoveArtists()
        {
            Lineup.Clear();
        }
        public virtual void AddGallery(Gallery gallery)
        {
            if (!Galleries.Contains(gallery))
                Galleries.Add(gallery);
        }
        public virtual void RemoveGallery(Gallery gallery)
        {
            if (Galleries.Contains(gallery))
                Galleries.Remove(gallery);
        }
        public virtual Ticket AddTicket(string name, decimal price)
        {
            var ticket =new Ticket() {Name=name, Price=price};
            if (!Tickets.Contains(ticket))
                Tickets.Add(ticket);
            ReCalculateTicketTotals();
            return ticket;
        }
        public virtual void RemoveTicket(Ticket ticket)
        {
            if (Tickets.Contains(ticket))
            {
                if (ticket.Sold == 0)
                    Tickets.Remove(ticket);
                else
                    ticket.Disable();
            }
        }

        public virtual ReturnValue UpdateTicketInfo(Ticket ticket, string name, decimal price, decimal bookingFee, int allocation, bool enabled)
        {
            var retVal = ticket.UpdateTicketInfo(name, price, bookingFee, allocation, enabled);
            ReCalculateTicketTotals();
            return retVal;
        }
        public virtual void DisableTicket(Ticket ticket)
        {
            ticket.Disable();
            ReCalculateTicketTotals();
        }
        public virtual void EnableTicket(Ticket ticket)
        {
            ticket.Enable();
            ReCalculateTicketTotals();
        }

        public virtual void Cancel()
        {
            throw new Exception("Not Yet Implemented");
        }

        public virtual ReturnValue CanDelete()
        {
            if(!Cancelled && TotalSold > 0) return new ReturnValue(false, "Tickets have be sold. Please cancel the event before deleting");
            return new ReturnValue();
        }

        public virtual void ReCalculateTicketTotals()
        {
            TotalAllocation = 0;
            TotalSold = 0;
            foreach (var ticket in Tickets)
            {
                if(ticket.Enabled)
                    TotalAllocation += ticket.Allocation;
                TotalSold += ticket.Sold;
            }
        }
        

        public virtual ReturnValue Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) return new ReturnValue(false, "Please provide an event name");
            if (string.IsNullOrWhiteSpace(Slug)) return new ReturnValue(false, "Please provide a slug");
            return new ReturnValue();
        }
        


    }
}
