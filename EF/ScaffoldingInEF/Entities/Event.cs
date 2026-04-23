using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ScaffoldingInEF.Entities;

public partial class Event
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime StartAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndAt { get; set; }

    public int SpeakerId { get; set; }

    [ForeignKey("SpeakerId")]
    [InverseProperty("Events")]
    public virtual Speaker Speaker { get; set; } = null!;
}
