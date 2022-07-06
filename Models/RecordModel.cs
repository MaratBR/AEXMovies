using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AEXMovies.Models;

/// <summary>
/// RecordModel is a common base model for *some* of the other models.
/// It is useful when you don't want to write some repository methods (GetById, MarkAsDeleted, etc) on your own
/// since with this model you can just extend IRecordRepositoryModel interface and EfRecordRepository implementation. 
/// </summary>
public class RecordModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}