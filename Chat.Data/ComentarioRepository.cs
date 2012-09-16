using Chat.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace Chat.Data
{
    public class ComentarioRepository : BaseRepository<Comentario>
    {
        public Comentario Get(int id)
        {
            var ret = this.GetAllFilteredBy(x => x.Id == id);
            if (ret.Any())
            {
                return ret.First();
            }
            else
            {
                return null;
            }
        }

        public Comentario AdicionarComentario(int? comentarioPaiId, string texto, Usuario usuario, ComentarioRepository comentarioRepository)
        {
            Comentario novoComentario = new Comentario() { Texto = texto, CriadoEm = DateTime.Now };
            // cria nosso factory de sessão NHibernate
            var sessionFactory = CreateSessionFactory();
            using (var session = sessionFactory.OpenSession())
            {
                // popula o banco de dados
                using (var transaction = session.BeginTransaction())
                {
                    if (comentarioPaiId.HasValue)
                    {
                        var comentarioPai = comentarioRepository.Get(comentarioPaiId.Value);
                        comentarioPai.AdicionarComentario(usuario, novoComentario);
                        comentarioRepository.Add(novoComentario);
                    }
                    else
                    {
                        novoComentario.Usuario = usuario;
                        comentarioRepository.Add(novoComentario);
                    }
                    session.SaveOrUpdate(novoComentario);
                    transaction.Commit();
                }
            }
            return novoComentario;
        }

        public void Curtir(int comentarioId, string nome, Action<Usuario> callback)
        {
            var usuarioRepository = new UsuarioRepository();
            var usuario = usuarioRepository.GetAllFilteredBy(x => x.Login.Equals(nome,
                StringComparison.InvariantCultureIgnoreCase)).Single();
            var comentario = this.Get(comentarioId);
            if (!comentario.Curtiram.Contains(usuario))
            {
                // cria nosso factory de sessão NHibernate
                var sessionFactory = CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        comentario.Curtiram.Add(usuario);
                        transaction.Commit();
                    }
                }
                if (callback != null)
                    callback(usuario);
            }
        }

        public void Descurtir(int comentarioId, string nome, Action<Usuario> callback)
        {
            var usuarioRepository = new UsuarioRepository();
            var usuario = usuarioRepository.GetAllFilteredBy(x => x.Login.Equals(nome,
                StringComparison.InvariantCultureIgnoreCase)).Single();
            var comentario = this.Get(comentarioId);
            if (comentario.Curtiram.Contains(usuario))
            {
                // cria nosso factory de sessão NHibernate
                var sessionFactory = CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        comentario.Curtiram.Remove(usuario);
                        transaction.Commit();
                    }
                }
                if (callback != null)
                    callback(usuario);
            }
        }
    }
}
