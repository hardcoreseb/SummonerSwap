using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonerSwap.Models
{
    public class ProfileMetaData
    {
        public string Image { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string Notes { get; set; } = "";
    }
}
