using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using umfg.venda.app.Abstracts;
using umfg.venda.app.Interfaces;
using umfg.venda.app.UserControls;
using umfg.venda.app.ViewModels;

namespace umfg.venda.app.Commands
{
    internal sealed class PagarPedidoCommand : AbstractCommand
    {
        public override void Execute(object? parameter)
        {
            var rp = parameter as ReceberPedidoViewModel;
            if (rp == null) {
                MessageBox.Show("Falha ao processar o pagamento, tente novamente mais tarde");
                return;
            }

            if (rp?.CVV.ToString().Length != 3 )
            {
                MessageBox.Show("O CVV está invalido");
                return;
            }

            if (rp?.NomeCartao.Length < 1)
            {
                MessageBox.Show("O Nome está invalido");
                return;
            }

            if (!IsCartaoValido(rp?.NumeroCartao.ToString() ?? string.Empty))
            {
                MessageBox.Show("O campo Numero do Cartao está invalido");
                return;
            }

            if (!ValidarDataValidade(rp?.DataValidade ?? string.Empty))
            {
                MessageBox.Show("O campo Data de Validade está invalido. Use o formato MM/yyyy");
                return;
            }

            MessageBox.Show("Compra realizada com sucesso!!!!");

            ucListarProdutos.Show(rp.MainWindow);

        }

        public static bool IsCartaoValido(string numeroCartao)
        {
            numeroCartao = numeroCartao.Replace(" ", "");
            numeroCartao = numeroCartao.Replace("-", "");

            if (numeroCartao.Length < 16)
                return false;

            int soma = 0;
            bool dobrar = false;

            for (int i = numeroCartao.Length - 1; i >= 0; i--)
            {
                int digito = int.Parse(numeroCartao[i].ToString());

                if (dobrar)
                {
                    digito *= 2;

                    if (digito > 9)
                        digito -= 9;
                }

                soma += digito;
                dobrar = !dobrar;
            }

            var res = soma % 10 == 0;
            return res;
        }

        private static bool ValidarDataValidade(string dataValidade)
        {
            if (string.IsNullOrWhiteSpace(dataValidade))
                return false;

            var partes = dataValidade.Split('/');
            if (partes.Length != 2)
                return false;

            if (!int.TryParse(partes[0], out int mes) || !int.TryParse(partes[1], out int ano))
                return false;

            if (mes < 1 || mes > 12)
                return false;

            if (ano < 1000)
                ano += 2000;

            var dataCartao = new DateTime(ano, mes, 1);
            var ultimoDiaMes = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));

            return ultimoDiaMes >= DateTime.Now;
        }
    }
}
