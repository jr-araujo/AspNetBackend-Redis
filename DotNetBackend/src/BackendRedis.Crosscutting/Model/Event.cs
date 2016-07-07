using System;

namespace BackendRedis.Crosscutting.Model
{
    public class Event
    {
        public Guid Id { get
            {
                return Guid.NewGuid();
            }
        }
        public string Name { get; set; }
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LocalDescription { get; set; }
    }
}