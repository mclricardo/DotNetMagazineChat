using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chat.Domain.Model;
using FluentNHibernate.Mapping;

namespace Chat.Domain.Mappings
{
    public class ComentarioMap : BaseMap<Comentario>
    {
        public ComentarioMap()
        {
            Map(x => x.Texto).Length(1000);
            Map(x => x.Ordem);
            References(x => x.ComentarioPai, "ComentarioPai_id");
            References(x => x.Usuario, "Usuario_id");
            HasMany(x => x.Respostas)
                .Cascade.All()
                .Inverse()
                .OrderBy("Ordem");
        }
    }
}
