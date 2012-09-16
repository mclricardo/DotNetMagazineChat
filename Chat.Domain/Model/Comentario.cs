using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Chat.Domain.Model
{
    public class Comentario : Entidade
    {
        public Comentario()
        {
            Comentarios = new List<Comentario>();
            Curtiram = new List<Usuario>();
        }

        public virtual int Ordem  { get; set; }
        public virtual Usuario Usuario { get; set; }
        [ScriptIgnore]
        public virtual Comentario ComentarioPai { get; set; }
        public virtual string Texto { get; set; }
        public virtual IList<Comentario> Comentarios { get; set; }
        public virtual IList<Usuario> Curtiram { get; set; }

        public virtual Comentario AdicionarComentario(Comentario comentario)
        {
            comentario.ComentarioPai = this;
            Comentarios.Add(comentario);
            return comentario;
        }

        public virtual Comentario AdicionarComentario(Usuario usuario, Comentario comentario)
        {
            comentario.Usuario = usuario;
            comentario.ComentarioPai = this;
            comentario.Ordem = Comentarios.Count() + 1;
            Comentarios.Add(comentario);

            usuario.Comentarios.Add(comentario);
            return comentario;
        }

        public virtual Usuario Curtir(Usuario usuario)
        {
            Curtiram.Add(usuario);
            return usuario;
        }
    }
}
