using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homework_CRUD_Scafolding_04.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MaxLength(50)]
    public string Position { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }
}