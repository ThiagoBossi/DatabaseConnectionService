using DatabaseConnectionService.Code.DAL;
using DatabaseConnectionService.Code.Services;
using System;
using Topshelf;

namespace DatabaseConnectionService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var conexao = new dalConexao();
            conexao.Conectar();

            var exitCode = HostFactory.Run(x =>
            {
                x.Service<DataRead>(s =>
                {
                    s.ConstructUsing(dataread => new DataRead());
                    s.WhenStarted(dataread => dataread.iniciarTimer());
                    s.WhenStopped(dataread => dataread.pararTimer());
                });

                x.RunAsLocalSystem();
                x.SetServiceName("DatabaseConnectionService");
                x.SetDisplayName("Database Connection Service");
                x.SetDescription("Service that get an json and insert it into database");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());

            Environment.ExitCode = exitCodeValue;
        }
    }
}
