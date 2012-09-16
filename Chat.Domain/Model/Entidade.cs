using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Chat.Domain.Model
{
    public abstract class Entidade
    {
        public Entidade()
        {
            CriadoEm = DateTime.Now;
        }

        public virtual int Id { get; set; }
        public virtual DateTime CriadoEm { get; set; }
    }
}
