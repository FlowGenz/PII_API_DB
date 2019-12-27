using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API_DbAccess
{
    public partial class SentencesOfTheDay
    {
        public string Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Sentence { get; set; }
    }
}
