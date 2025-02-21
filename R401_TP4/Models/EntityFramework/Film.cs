﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace R401_TP4.Models.EntityFramework
{
    [Table("t_e_film_flm")]
    [Index(nameof(Titre), Name = "ix_t_e_film_flm_titre")]
    public partial class Film
    {
        [Key]
        [Column("flm_id")]
        public int FilmId { get; set; }

        [Column("flm_titre")]
        [StringLength(100)]
        public string Titre { get; set; } = null!;

        [Column("flm_resume")]
        public string? Resume { get; set; }

        [Column("flm_datesortie", TypeName = "date")]
        public DateTime DateSortie { get; set; }

        [Column("flm_duree", TypeName = "date")]
        public decimal Duree { get; set; }

        [Column("flm_genre", TypeName = "numeric(3,0)")]
        [StringLength(30)]
        public string? Genre { get; set; }

        [InverseProperty(nameof(Notation.FilmId))]
        public virtual ICollection<Notation> NotesFilm { get; set; } = null!;
    }
}
