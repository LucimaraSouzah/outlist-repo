using System.ComponentModel.DataAnnotations;

public class AddOutlistRequest
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    [DateGreaterThan("StartDate", ErrorMessage = "EndDate must be greater than StartDate.")]
    public DateTime EndDate { get; set; }
}
