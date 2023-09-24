﻿using SAM.Entities.Enum;

namespace SAM.Entities
{
    public class OrderService
    {
        public int IdOrderService { get; set; }
        public string? Description { get; set; }
        public OrderServiceStatusEnum Status { get; set; }
        public DateTime Opening { get; set; }
        public DateTime Closed { get; set; }
        public Unit? Unit { get; set; }
        public Machine? Machine { get; set; }
    }
}