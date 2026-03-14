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

            if (rp?.CVV.ToString().Length != 3 )
            {
                MessageBox.Show("O campo CVV está invalido");
                return;
            }

            if (rp?.NomeCartao.Length < 1)
            {
                MessageBox.Show("O campo NomeCartao está invalido");
                return;
            }

            if (!ValidarCartao(rp?.NumeroCartao.ToString() ?? string.Empty))
            {
                MessageBox.Show("O campo NumeroCartao está invalido");
                return;
            }

            if (rp?.DataValidade > DateTime.Now)
            {
                MessageBox.Show("O campo DataValidade está invalido");
                return;
            }

            MessageBox.Show("Compra realizada com sucesso!!!!");


        }

        public static bool ValidarCartao(string numeroCartao)
        {
            numeroCartao = numeroCartao.Replace(" ", "");
            numeroCartao = numeroCartao.Replace("-", "");

            if (numeroCartao.Length > 16)
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
    }
}
