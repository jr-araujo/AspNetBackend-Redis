using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GabBsb2016BackEnd.Model
{
    public class Presentation
    {
        public Guid Id
        {
            get
            {
                return Guid.NewGuid();
            }
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SpeakerName { get; set; }
        public string Room { get; set; }
        public string EventName { get; set; }
    }
}