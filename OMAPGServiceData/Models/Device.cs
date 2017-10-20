using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OMAPGServiceData.Models
{
    public class Device
    {
        public long Id { get; set; }
        public string DeviceId { get; set; }
        public int OSType { get; set; }
        [NotMapped]
        public IEnumerable<int> NotifyPokemon
        {
            get 
            {
                var rval = NotifyPokemonStr.Split(":").Select(p => int.Parse(p));
                return rval;
            }
            set
            {
                NotifyPokemonStr = string.Join(":", value);
            }
        }
        public string NotifyPokemonStr { get; set; }
        public double LocationLat { get; set; }
        public double LocationLon { get; set; }
        public int DistanceAlert { get; set; }
        public bool NotifyEnabled { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
    }
}
