using System;
using System.Collections.Generic;

namespace DojoActivity.Models
{
    public class User : BaseEntity
    {
        public User()
        {
            MyActivities = new List<Activity>();
            Participating = new List<Participant>();
        }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Activity> MyActivities { get; set; }
        public List<Participant> Participating { get; set; }
    }
}