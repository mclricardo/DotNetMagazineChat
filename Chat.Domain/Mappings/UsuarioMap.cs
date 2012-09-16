using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Chat.Domain.Model;

namespace Chat.Domain.Mappings
{
    public class UsuarioMap : BaseMap<Usuario>
    {
        public UsuarioMap()
        {
            Map(x => x.Nome);
            Map(x => x.Login);
            Map(x => x.Senha);
            HasMany(x => x.Comentarios)
                .Cascade.All()
                .Inverse();
        }
    }
}
