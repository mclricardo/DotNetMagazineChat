using Chat.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace Chat.Data
{
    public class UsuarioRepository : BaseRepository<Usuario>
    {
        public bool ValidarUsuario(string login, string senha)
        {
            var usuarios = this.GetAllFilteredBy(x => x.Login.Equals(login) && x.Senha.Equals(senha));
            return (usuarios.Count() == 1);
        }
    }
}
