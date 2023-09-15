using System;
using System.Collections.Generic;

namespace ToDoAPI.DTOs
{
    public class User
    {
        public string? Id { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
    }
}