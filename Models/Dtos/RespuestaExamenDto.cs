public class RespuestaExamenDto
{
    public int CapacitacionId { get; set; }
    public List<SeleccionDto> Respuestas { get; set; }
}

public class SeleccionDto
{
    public int PreguntaId { get; set; }
    public int OpcionIdSeleccionada { get; set; }
}