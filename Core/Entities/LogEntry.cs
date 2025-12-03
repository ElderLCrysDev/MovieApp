using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class LogEntry
{
    public int Id { get; set; }
    public int? IdUsuario { get; set; }
    public string EndpointRequisicao { get; set; } = string.Empty;
    public DateTime DataHoraRequisicao { get; set; }
    public bool ObteveSucesso { get; set; }
}
