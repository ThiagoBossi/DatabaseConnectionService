using DatabaseConnectionService.Code.DAL;
using DatabaseConnectionService.Code.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Text.Json;

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

            string jsonData = File.ReadAllText(jsonPath);
            var listaClientes = JsonSerializer.Deserialize<List<dtoCliente>>(jsonData);

            foreach (var item in listaClientes)
            {
                var dalCliente = new dalCliente();
                if (dalCliente.verificarCpfExistente(item.cpf))
                {
                    _timer.Start();
                    Console.WriteLine("Registro já existente... Ignorando dados.");
                }
                else
                {
                    if (dalCliente.inserirRegistro(item))
                    {
                        Console.WriteLine("Registro inserido com sucesso! Realizando nova pesquisa em 5 segundos.");
                    }

                    _timer.Start();
                }
            }

            File.Delete(jsonPath);
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
