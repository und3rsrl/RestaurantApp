using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class CurrencyResponse
    {
        public string Base { get; set; }

        public Dictionary<string, double> Rates { get; set; }

        public DateTime Date { get; set; }
    }
}
