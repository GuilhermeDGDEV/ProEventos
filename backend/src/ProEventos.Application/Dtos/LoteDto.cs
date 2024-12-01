namespace ProEventos.Application.Dtos;

public class LoteDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public string DateInicio { get; set; }
    public string DateFim { get; set; }
    public int Quantidade { get; set; }
    public int EventoId { get; set; }
    public EventoDto Evento { get; set; }
}