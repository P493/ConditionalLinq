using System;

namespace ConditionalLinq.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int State { get; set; }

        public DateTime CreationDate { get; set; }

        public int ExtraProperty1 { get; set; }

        public bool ExtraProperty2 { get; set; }
    }
}
