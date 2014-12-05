using SQLite;

namespace Wiktionary.Model
{
    /// <summary>
    /// Classe Modele pour la gestion d'un mot
    /// </summary>
    public class Mot
    {
        [PrimaryKey, Column("valeur"), NotNull]
        public string Valeur { get; set; }
        [Column("definition"), NotNull]
        public string Definition { get; set; }
    }
}
