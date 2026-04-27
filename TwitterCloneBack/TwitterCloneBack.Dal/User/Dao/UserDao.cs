using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBack.Dal.Post.Dao;

namespace TwitterCloneBack.Dal.User.Dao;

[Index("Username", Name = "UQ__Users__536C85E4FB423C01", IsUnique = true)]
[Index("Email", Name = "UQ__Users__A9D105341996ED1A", IsUnique = true)]
public partial class UserDao
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(30)]
    public string DisplayUsername { get; set; } = null!;

    [StringLength(200)]
    public string? Bio { get; set; }

    public DateTime CreatedAt { get; set; }
    
    [MaxLength(256)]
    public byte[] PasswordHash { get; set; } = null!;

    [InverseProperty("Author")]
    public virtual ICollection<PostDao> Posts { get; set; } = new List<PostDao>();
}
