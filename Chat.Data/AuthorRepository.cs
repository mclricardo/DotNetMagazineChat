using Chat.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace Chat.Data
{
    public class AuthorRepository : BaseRepository<Author>
    {
        public bool ValidateUser(string userName, string password)
        {
            var authors = this.GetAllFilteredBy(x => x.Login.Equals(userName) && x.Password.Equals(password));
            return (authors.Count() == 1);
        }
    }
}
