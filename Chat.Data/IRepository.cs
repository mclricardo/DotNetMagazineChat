using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chat.Domain.Model;
using Chat.Domain;

namespace Chat.Data
{
    public interface IRepository<T> where T : Entidade
    {
        T Get(int id);
        IList<T> GetByExample(T example);
        IList<T> GetAll();
        void Add(T T);
        void Delete(int id);
        void DeleteAll();
    }
}
