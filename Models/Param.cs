using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models;

public class Param
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ID { get; set; } = null!;

    [Required]
    public string ValueLog { get; set; } = null!;

    [Required]
    public string ValueConverted { get; set; } = null!;
    [Required]
    public bool IsToSend { get; set; } = true;

    public DateTime? Created_at { get; set; }
    [JsonIgnore]
    public DateTime? Updated_at { get; set; }

    public Param()
    {
        Created_at = DateTime.UtcNow;
        Updated_at = DateTime.UtcNow;
    }



}