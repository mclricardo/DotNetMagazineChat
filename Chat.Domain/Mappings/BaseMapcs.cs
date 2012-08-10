using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Chat.Domain.Model;

namespace Chat.Domain.Mappings
{
    public abstract class BaseMap<T> : ClassMap<T> where T : Entity
    {
        public BaseMap()
        {
            Id(x => x.Id);
            Map(x => x.CreatedOn);
        }
    }
}
