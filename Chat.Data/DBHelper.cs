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
                var hamlet = new Usuario { Login = "hamlet", Senha = commonPassword, Nome = "Hamlet" };
                var horacio = new Usuario { Login = "horacio", Senha = commonPassword, Nome = "Horácio" };

                AddAuthors(session, hamlet, horacio);

                var author = hamlet;

                var time = DateTime.Now.Add(new TimeSpan(-40, 0, 0));
                var msg = AddMessage(author, "Deixa-me vê-lo. Pobre Yorick! Conheci-o, Horácio; um sujeito de chistes inesgotáveis e de uma fantasia soberba. Carregou-me muitas vezes às costas. E agora, como me atemoriza a imaginação! Sinto engulhos. Era aqui que se encontravam os lábios que eu beijei não sei quantas vezes. Onde estão agora os chistes, as cabriolas, as canções, os rasgos de alegria que faziam explodir a mesa em gargalhadas? Não sobrou uma ao menos, para rir de tua própria careta? Tudo descarnado! Vai agora aos aposentos da senhora e dize-lhe que embora se retoque com uma camada de um dedo de espessura, algum dia ficará deste jeito. Faze-a rir com semelhante pilhéria. Dize-me uma coisa, Horácio, por obséquio.", time);
                time = time.Add(new TimeSpan(0, 149, 0));
                session.SaveOrUpdate(msg);
                AddMessage(horacio, msg, "Que é, príncipe?", time);
                time = time.Add(new TimeSpan(0, 149, 0));
                session.SaveOrUpdate(msg);
                AddMessage(hamlet, msg, "Acreditas que Alexandre, depois de enterrado, tivesse este mesmo aspecto?", time);
                time = time.Add(new TimeSpan(0, 149, 0));
                session.SaveOrUpdate(msg);
                AddMessage(horacio, msg, "Igual, igual, príncipe.", time);
                time = time.Add(new TimeSpan(0, 149, 0));
                session.SaveOrUpdate(msg);
                AddMessage(hamlet, msg, "E este cheiro? Puá!", time);
                time = time.Add(new TimeSpan(0, 149, 0));
                session.SaveOrUpdate(msg);
                AddMessage(horacio, msg, "O mesmo, príncipe.", time);
                time = time.Add(new TimeSpan(0, 149, 0));
                session.SaveOrUpdate(msg);
                time = DateTime.Now.Add(new TimeSpan(-1, 0, 0));
                msg = AddMessage(hamlet, msg, "A que usos ínfimos temos de prestar-nos, Horácio. Por que não acompanhar a imaginação as nobres cinzas de Alexandre, até encontrá-las servindo para tapar um barril?", time);
                time = time.Add(new TimeSpan(0, 2, 0));
                session.SaveOrUpdate(msg);
                AddMessage(horacio, msg, "É ir muito longe, considerar as coisas por esse modo.", time);
                time = time.Add(new TimeSpan(0, 3, 0));
                session.SaveOrUpdate(msg);
                AddMessage(hamlet, msg, "De forma alguma. Acompanhemo-las com bastante modéstia, deixando-nos guiar apenas pela verossimilhança. Mais ou menos deste jeito: Alexandre morreu; Alexandre foi enterrado; Alexandre tornou-se pó. O pó é terra; da terra faz-se argila; por que, então, não se poderá tapar um barril de cerveja com a argila em que ele se converteu? O grande César morto e em pó tornado, pode a fenda vedar ao vento irado. O pó que o mundo inteiro trouxe atento, ora o muro protege contra o vento. Mas, silêncio; cautela. Afastemo-nos. Aí vem o rei.", time);
                time = time.Add(new TimeSpan(0, 1, 0));
                session.SaveOrUpdate(msg);


                msg = AddMessage(author, "Olá, Horácio!", time);
                time = time.Add(new TimeSpan(0, 149, 0));
                session.SaveOrUpdate(msg);
                AddMessage(horacio, msg, "Aqui me tendes, senhor, às vossas ordens.", time);
                time = time.Add(new TimeSpan(0, 149, 0));
                session.SaveOrUpdate(msg);
                AddMessage(hamlet, msg, "Horácio, és a pessoa mais talhada para meu companheiro e confidente.", time);
                time = time.Add(new TimeSpan(0, 149, 0));
                session.SaveOrUpdate(msg);
                AddMessage(horacio, msg, "Meu príncipe...", time);
                time = time.Add(new TimeSpan(0, 149, 0));
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
