using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chat.Domain.Model;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate;

namespace Chat.Data
{
    public class DBHelper
    {
        public static void Generate()
        {
            // cria nosso factory de sessão NHibernate
            var sessionFactory = CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // popula a base de dados
                using (var transaction = session.BeginTransaction())
                {
                    var commonPassword = "123456";
                    var barriga = new Usuario { Login = "barriga", Senha = commonPassword, Nome = "Sr. Barriga" };
                    var madruga = new Usuario { Login = "madruga", Senha = commonPassword, Nome = "Sr. Madruga" };
                    var chaves = new Usuario { Login = "chaves", Senha = commonPassword, Nome = "Chaves" };
                    var chiquinha = new Usuario { Login = "chiquinha", Senha = commonPassword, Nome = "Chiquinha" };

                    AddAuthors(session, barriga, madruga, chaves, chiquinha);

                    var author = barriga;

                    var time = DateTime.Now.Add(new TimeSpan(-40, 0, 0));
                    var msg = AddMessage(author, "Tirem a mobília toda. Ponham tudo aí!", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(madruga, msg, "Meu senhor, pode me explicar o que significa isso?", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(barriga, msg, "Eu avisei que se não pagasse todo o aluguel atrasado, eu despejaria o senhor.", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(madruga, msg, "Mas eu já tenho o dinheiro.", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(barriga, msg, "Ah é? Então dá!", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(madruga, msg, "Está ali dentro, venha. Vamos entrar.", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);

                    time = DateTime.Now.Add(new TimeSpan(-1, 0, 0));
                    msg = AddMessage(chaves, "Chiquinha, Chiquinha!", time);
                    time = time.Add(new TimeSpan(0, 2, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(chiquinha, msg, "O que foi Chaves?", time);
                    time = time.Add(new TimeSpan(0, 3, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(chaves, msg, "Olha, isso aqui é igualzinho ao que você tem na sua casa.", time);
                    time = time.Add(new TimeSpan(0, 1, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(chiquinha, msg, "É mesmo!", time);
                    time = time.Add(new TimeSpan(0, 7, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(chaves, msg, "E a cama também é igualzinha a que tem na sua casa. E isso aqui também. Todos os móveis são iguaizinhos aos da sua casa.", time);
                    time = time.Add(new TimeSpan(0, 2, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(chiquinha, msg, "Não são iguaizinhos aos móveis da minha casa. São os móveis da minha casa.", time);
                    time = time.Add(new TimeSpan(0, 2, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(chaves, msg, "Nossa, e o que eles estão fazendo aqui fora?", time);
                    time = time.Add(new TimeSpan(0, 1, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(chiquinha, msg, "Ah, eu já sei! Meu pai vai comprar móveis novos, Chaves!", time);
                    session.SaveOrUpdate(msg);
                    AddMessage(chaves, msg, "E por isso ele botou aqui fora?", time);
                    time = time.Add(new TimeSpan(0, 1, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(chiquinha, msg, "Ele vai jogar tudo no lixo... Vamos, vamos brincar!", time);
                    session.SaveOrUpdate(msg);

                    time = DateTime.Now.Add(new TimeSpan(-1, 0, 0));
                    msg = AddMessage(madruga, "Chiquinha!", time);
                    time = time.Add(new TimeSpan(0, 2, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(chiquinha, msg, "Sim, pai.", time);
                    time = time.Add(new TimeSpan(0, 3, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(madruga, msg, "Chiquinha, você não viu onde eu deixei o dinheiro do aluguel?", time);
                    time = time.Add(new TimeSpan(0, 1, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(chiquinha, msg, "Sim, papai. O senhor disse que ia colocar em um envelope de papel e que depois ia colocar em algum móvel.", time);
                    time = time.Add(new TimeSpan(0, 7, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(madruga, msg, "Ah, tem razão! Mas em que móvel eu guardei?", time);
                    time = time.Add(new TimeSpan(0, 2, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(chiquinha, msg, "Nem imagino.", time);
                    time = time.Add(new TimeSpan(0, 2, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(madruga, msg, "Me ajude a achar então.", time);
                    time = time.Add(new TimeSpan(0, 1, 0));
                    session.SaveOrUpdate(msg);

                    transaction.Commit();
                }
            }
        }

        private static Comentario AddMessage(Usuario author, string text, DateTime criadoEm)
        {
            var msg = author.AdicionarComentario(new Comentario() { Texto = text, CriadoEm = criadoEm });
            return msg;
        }

        private static Comentario AddMessage(Usuario author, Comentario msg, string text, DateTime criadoEm)
        {
            return msg.AdicionarComentario(author, new Comentario() { Texto = text, CriadoEm = criadoEm });
        }

        private static void AddAuthors(ISession session, params Usuario[] authors)
        {
            foreach (var author in authors)
            {
                session.SaveOrUpdate(author);
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();

            return Fluently.Configure()
                .Database(MsSqlCeConfiguration.Standard
                    .ConnectionString(connectionString))
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<Chat.Domain.Model.Usuario>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
                .Create(false, true);
        }

        public static FluentNHibernate.Automapping.AutoPersistenceModel CreateAutomappings { get; set; }
    }
}
