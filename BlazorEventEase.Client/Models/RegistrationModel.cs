using System.ComponentModel.DataAnnotations;

namespace BlazorEventEase.Client.Models;

public sealed class RegistrationModel
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(
        100,
        MinimumLength = 3,
        ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Captura un correo electrónico válido.")]
    [StringLength(
        254,
        ErrorMessage = "El correo electrónico no puede exceder 254 caracteres.")]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;
}