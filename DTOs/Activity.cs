using System;
using System.Collections.Generic;

namespace ToDoAPI.DTOs
{
    public class Activity
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public DateTime When { get; set; }
    }
}