using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote366.Core.Domain.Entities
{
    public class DirigentePartido
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int PartidoPoliticoId { get; set; }
        public PartidoPolitico PartidoPolitico { get; set; }
    }
}
