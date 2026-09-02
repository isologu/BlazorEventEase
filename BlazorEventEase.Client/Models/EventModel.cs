using System.ComponentModel.DataAnnotations;

namespace BlazorEventEase.Client.Models;

public sealed class EventModel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "El nombre del evento es obligatorio.")]
    [StringLength(
        100,
        MinimumLength = 3,
        ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required(ErrorMessage = "La ubicación es obligatoria.")]
    [StringLength(
        150,
        ErrorMessage = "La ubicación no puede exceder 150 caracteres.")]
    public string Location { get; set; } = string.Empty;

    [StringLength(
        500,
        ErrorMessage = "La descripción no puede exceder 500 caracteres.")]
    public string Description { get; set; } = string.Empty;
}