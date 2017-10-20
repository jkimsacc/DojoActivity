using System;
using System.Collections.Generic;

namespace DojoActivity.Models
{
    public class Activity : BaseEntity
    {
        public Activity()
        {
            Participants = new List<Participant>();
        }
        public int ActivityId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Duration { get; set; }
        public List<Participant> Participants { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
    }
}