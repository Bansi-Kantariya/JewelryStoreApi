using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JewelryStore.Model
{
    [Serializable]
    public class UserDetails
    {
        [Key]
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int UserType { get; set; }
    }
}
