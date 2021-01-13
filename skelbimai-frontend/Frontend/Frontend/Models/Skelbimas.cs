using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Skelbimas
    {
        public int Id { get; set; }
        public int FkUserId { get; set; }
        public int FkCategoryId { get; set; }
        public string Picture { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public int ViewCounter { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public decimal? Rating { get; set; }
    }
}
