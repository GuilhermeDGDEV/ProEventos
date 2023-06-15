using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos;

public class EventoDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "{0} é obrigatório")]
    public string? Local { get; set; }

    [Required(ErrorMessage = "{0} é obrigatório")]
    public DateTime? DataEvento { get; set; }

    [Required(ErrorMessage = "{0} é obrigatório")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "{0} deve ter entre 4 e 50 caracteres")]
    public string? Tema { get; set; }

    [Required(ErrorMessage = "{0} é obrigatório")]
    [Range(0, 120000, ErrorMessage = "{0} deve ter no máximo 12000")]
    [Display(Name = "Quantidade de Pessoas")]
    public int QtdPessoas { get; set; }
    
    [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "Não é uma imagem válida")]
    public string? ImagemURL { get; set; }

    [Required(ErrorMessage = "{0} é obrigatório")]
    [Phone(ErrorMessage = "{0} inválido")]
    public string? Telefone { get; set; }

    [Required(ErrorMessage = "{0} é obrigatório")]
    [EmailAddress(ErrorMessage = "{0} inválido")]
    [Display(Name = "E-mail")]
    public string? Email { get; set; }

    public IEnumerable<LoteDto> Lotes { get; set; }
    public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
    public IEnumerable<PalestranteDto> Palestrantes { get; set; }

    public EventoDto()
    {
        Lotes = new List<LoteDto>();
        RedesSociais = new List<RedeSocialDto>();
        Palestrantes = new List<PalestranteDto>();
    }
}
