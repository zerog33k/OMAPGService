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
                if (NotifyPokemonStr == null || NotifyPokemonStr.Equals(""))
                {
                        return new int[] { };
                }
                else
                {
                    var rval = NotifyPokemonStr.Split(":").Select(p => int.Parse(p));
                    return rval;
                }
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
        public bool Notify90 { get; set; }
        public bool Notify100 { get; set; }
        public int MaxDistance {get; set; } = 20;
        public int MinLevelAlert {get; set;}
        public string IgnorePokemonStr {get; set;}
        public DateTime UpdatedAt { get; set; }
        [NotMapped]
        public IEnumerable<int> IgnorePokemon
        {
            get
            {
                if (IgnorePokemonStr == null || IgnorePokemonStr.Equals(""))
                {
                    return new int[] { };
                }
                else
                {
                    var rval = IgnorePokemonStr.Split(":").Select(p => int.Parse(p));
                    return rval;
                }
            }
            set
            {
                IgnorePokemonStr = string.Join(":", value);
            }
        }
    }
}
