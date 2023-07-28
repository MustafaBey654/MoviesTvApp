using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesTvApp.Models
{
    public class Tv
    {
        public int id { get; set; }
        public string tvLogo { get; set; } = string.Empty;
        public string tvUrl { get; set; } = string.Empty;
        public string tvName { get; set; } = string.Empty;

    }
}
