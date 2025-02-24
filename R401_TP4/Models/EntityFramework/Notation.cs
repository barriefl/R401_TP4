using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace R401_TP4.Models.EntityFramework
{
    [Table("t_j_notation_not")]
    public partial class Notation
    {
        [Key]
        [Column("utl_id")]
        [ForeignKey(nameof(UtilisateurId))]
        public int UtilisateurId { get; set; }

        [Key]
        [Column("flm_id")]
        [ForeignKey(nameof(Film.NotesFilm))]
        public int FilmId { get; set; }

        [Required]
        [Range(0,5)]
        [Column("not_note")]
        public int Note { get; set; }

        [InverseProperty(nameof(Utilisateur.NotesUtilisateur))]
        public virtual Utilisateur UtilisateurNotant { get; set; } = null!;

        [InverseProperty(nameof(Film.NotesFilm))]
        public virtual Film FilmNote { get; set; } = null!;
    }
}
