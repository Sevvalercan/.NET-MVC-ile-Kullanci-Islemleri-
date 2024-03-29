﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Models
{
    public class PasswordCode
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

        [StringLength(7)] //veritabanını daha verimli kullanmak için
        public string Code { get; set; }
    }
}
