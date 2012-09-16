using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;
using Chat.Data;
using Chat.Models;
using Chat.Domain.Model;

namespace Chat.Hubs
{
    public class SocialHub : Hub
    {
        private readonly ChatModel _chat;

        public void EnviarCurtirParaServidor(int comentarioId)
        {
            var comentarioRepository = new ComentarioRepository();
            comentarioRepository.Curtir(comentarioId, Context.User.Identity.Name, (usuario) =>
                {
                    Clients.atualizarCurtidas(comentarioId, new {Id = usuario.Id, Nome = usuario.Nome});
                });
        }

        public void EnviarDescurtirParaServidor(int messageId)
        {
            var messageRepository = new ComentarioRepository();
            messageRepository.Descurtir(messageId, Context.User.Identity.Name, (usuario) =>
            {
                Clients.atualizarDescurtidas(messageId, new { Id = usuario.Id, Nome = usuario.Nome });
            });
        }

        public void EnviarComentarioParaServidor(int? comentarioPaiId, string texto)
        {
            string nome = Context.User.Identity.Name;
            var usuarioRepository = new UsuarioRepository();
            var usuario = usuarioRepository.GetAllFilteredBy(x => x.Login.Equals(nome, StringComparison.InvariantCultureIgnoreCase)).Single();
            var comentarioRepository = new ComentarioRepository();
            Comentario novoComentario = comentarioRepository.AdicionarComentario(comentarioPaiId, texto, usuario, comentarioRepository);
            Clients.adicionarComentario(comentarioPaiId, novoComentario.Id, texto, new { Id = usuario.Id, Nome = usuario.Nome });
        }

        public void Join(string nome)
        {
            ChatModel.Clients.Add(new Client() { Name = nome, LastResponse = DateTime.Now });
            Caller.Nome = nome;
        }
    }
}