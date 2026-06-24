using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class MemberRegisterFromInviteDto
{
    public required string Token { get; set; }
    
    [Required(ErrorMessage = "Το DisplayName είναι υποχρεωτικό")]
    [MaxLength(50, ErrorMessage = "Μέγιστο 50 χαρακτήρες")]
    public required string DisplayName { get; set; }
    
    [Required(ErrorMessage = "Το Password είναι υποχρεωτικό")]
    [MinLength(6,
        ErrorMessage = "Ο κωδικός πρέπει να έχει τουλάχιστον 6 χαρακτήρες")]
    [MaxLength(100)]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Η επιβεβαίωση κωδικού είναι υποχρεωτική")]
    [Compare(nameof(Password),
        ErrorMessage = "Οι κωδικοί δεν ταιριάζουν")]
    public required string ConfirmPassword { get; set; }
}
