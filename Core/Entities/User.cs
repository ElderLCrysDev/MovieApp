using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class User
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string TipoUsuario { get; set; } = "Normal";
}
