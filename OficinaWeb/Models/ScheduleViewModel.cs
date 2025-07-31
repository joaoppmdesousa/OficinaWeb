using System;

namespace OficinaWeb.Models
{
    public class ScheduleViewModel
    {    
            public int Id { get; set; }
            public string Subject { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }

    }
}
