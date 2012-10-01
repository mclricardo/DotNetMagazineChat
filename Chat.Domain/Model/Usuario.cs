using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Chat.Domain.Model
{
    public class Usuario : Entidade
    {
        public Usuario()
        {
            Comentarios = new List<Comentario>();
        }

        public virtual string Login { get; set; }
        [ScriptIgnore]
        public virtual string Senha { get; set; }
        public virtual string Nome { get; set; }
        public virtual string SmallPicturePath
        {
            get { return "Content/images/actor" + this.Id.ToString() + "_small.gif"; }
        }
        public virtual string MediumPicturePath
        {
            get { return "Content/images/actor" + this.Id.ToString() + "_medium.gif"; }
        }
        public virtual string LargePicturePath
        {
            get { return "Content/images/actor" + this.Id.ToString() + "_large.gif"; }
        }
        public virtual string PicturePath
        {
            get { return "Content/images/actor" + this.Id.ToString() + ".gif"; }
        }
        [ScriptIgnore]
        public virtual IList<Comentario> Comentarios { get; set; }

        public virtual Comentario AdicionarComentario(Comentario comentario)
        {
            comentario.Usuario = this;
            comentario.Ordem = Comentarios.Count() + 1;
            Comentarios.Add(comentario);
            return comentario;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Usuario;
            if (other == null)
                return false;
            else
                return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
