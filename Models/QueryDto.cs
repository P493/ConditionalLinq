using System;

namespace ConditionalLinq.Models
{
    public class QueryDto
    {
        public string CustomerName { get; set; }

        public string CustomerActive { get; set; }

        public string State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool NeedMoreCondition { get; set; }

        public string SelectedValue1 { get; set; }

        public string SelectedValue2 { get; set; }
    }
}
