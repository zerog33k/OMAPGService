using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OMAPGServiceData.Models
{
    public class Notification
    {

        public Device Device { get; set; }
        public string Message { get; set; }
        public int PokemonId { get; set; }
        public bool seen { get; set; }
        public long NotifyId { get; set; }
        public double Distance { get; set; }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
    }
}
