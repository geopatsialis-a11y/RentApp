using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class MemberInviteDto
{
    [Required(ErrorMessage = "Το Email είναι υποχρεωτικό")]
    [EmailAddress(ErrorMessage = "Το Email δεν είναι έγκυρο")]
    [MaxLength(100)]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Το FirstName είναι υποχρεωτικό")]
    [MaxLength(50, ErrorMessage = "Μέγιστο 50 χαρακτήρες")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Το LastName είναι υποχρεωτικό")]
    [MaxLength(50, ErrorMessage = "Μέγιστο 50 χαρακτήρες")]
    public required string LastName { get; set; }
    public string Role { get; set; } = "Member";

}
