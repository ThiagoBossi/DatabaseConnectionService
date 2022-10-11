using DatabaseConnectionService.Code.DAL;
using DatabaseConnectionService.Code.DTO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Timers;

namespace DatabaseConnectionService.Code.Services
{
    internal class DataRead
    {
        private readonly Timer _timer;

        public DataRead()
        {
            _timer = new Timer(5000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            string jsonPath = @"C:\Trabalho\Services\DatabaseConnectionService\clienteData.txt";

            if (!File.Exists(jsonPath))
            {
                _timer.Start();
                Console.WriteLine("Arquivo inexistente... Realizando nova pesquisa em 5 segundos.");
                return;
            }

            var jsonData = File.ReadAllText(jsonPath);
            var clientes = JsonConvert.DeserializeObject<dtoCliente>(jsonData);

            var dalCliente = new dalCliente();
            if (dalCliente.verificarCpfExistente(clientes.cpf))
            {
                File.Delete(jsonPath);
                _timer.Start();
                Console.WriteLine("Registro já existente... Removendo arquivo.");
                return;
            }

            if (dalCliente.inserirRegistro(clientes))
            {
                Console.WriteLine("Registro inserido com sucesso! Realizando nova pesquisa em 5 segundos.");
                File.Delete(jsonPath);
            }

            _timer.Start();
        }

        public void iniciarTimer()
        {
            _timer.Start();
        }

        public void pararTimer()
        {
            _timer.Stop();
        }
    }
}
