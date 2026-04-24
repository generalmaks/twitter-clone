using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TwitterCloneBack.Dal.Dao;

public partial class PostDao
{
    [Key]
    public int Id { get; set; }

    public int AuthorId { get; set; }

    [StringLength(200)]
    public string TextContent { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("Posts")]
    public virtual UserDao Author { get; set; } = null!;
}
