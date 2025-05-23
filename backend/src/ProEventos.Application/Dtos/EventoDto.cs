using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos;

public class EventoDto
{
    public int Id { get; set; }
    public string Local { get; set; }
    public string DataEvento { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [MinLength(4, ErrorMessage = "{0} deve ter no mínimo 4 caracteres")]
    [MaxLength(50, ErrorMessage = "{0} deve ter no mínimo 50 caracteres")]
    public string Tema { get; set; }
    [Display(Name = "Qtd Pessoas")]
    [Range(1, 120000, ErrorMessage = "{0} não pode ser menor que 1 e maior que 120.000")]
    public int Qtdpessoas { get; set; }
    [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", 
        ErrorMessage = "Não é uma imagem válida. (gif, jpeg, jpg, bmp ou png)")]
    public string ImagemURL { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Phone(ErrorMessage = "O campo {0} está com número inválido")]
    public string Telefone { get; set; }
    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "(O campo {0} não está válido)")]
    public string Email { get; set; }
    public int UserId { get; set; }
    public UserDto UserDto { get; set; }
    public IEnumerable<LoteDto> Lotes { get; set; }
    public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
    public IEnumerable<PalestranteDto> Palestrantes { get; set; }
}