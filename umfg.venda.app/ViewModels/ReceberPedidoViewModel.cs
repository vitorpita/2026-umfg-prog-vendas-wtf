using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using umfg.venda.app.Abstracts;
using umfg.venda.app.Commands;
using umfg.venda.app.Interfaces;
using umfg.venda.app.Models;

namespace umfg.venda.app.ViewModels
{
    internal sealed class ReceberPedidoViewModel : AbstractViewModel
    {
        private PedidoModel _pedido = new();
        private string _numeroCartao = string.Empty;
        private string _cvv = string.Empty;
        private string _dataValidade = string.Empty;
        private string _nomeCartao = string.Empty;
        private int _mesSelecionado = 1;
        private int _anoSelecionado = DateTime.Now.Year;

        public string NumeroCartao 
        {
            get => _numeroCartao;
            set => SetField(ref _numeroCartao, value);
        }

        public string CVV
        {
            get => _cvv;
            set => SetField(ref _cvv, value);
        }

        public string DataValidade
        {
            get => _dataValidade;
            set => SetField(ref _dataValidade, value);
        }

        public int MesSelecionado
        {
            get => _mesSelecionado;
            set
            {
                SetField(ref _mesSelecionado, value);
                AtualizarDataValidade();
            }
        }

        public int AnoSelecionado
        {
            get => _anoSelecionado;
            set
            {
                SetField(ref _anoSelecionado, value);
                AtualizarDataValidade();
            }
        }

        public List<int> Meses { get; } = Enumerable.Range(1, 12).ToList();
        public List<int> Anos { get; } = Enumerable.Range(DateTime.Now.Year, 20).ToList();

        private void AtualizarDataValidade()
        {
            DataValidade = $"{MesSelecionado:D2}/{AnoSelecionado}";
        }

        public string NomeCartao
        {
            get => _nomeCartao;
            set => SetField(ref _nomeCartao, value);
        }

        public PedidoModel Pedido
        {
            get => _pedido;
            set => SetField(ref _pedido, value);
        }

        public ReceberPedidoViewModel(UserControl userControl, IObserver observer, PedidoModel pedido) 
            : base("Receber Pedido")
        {
            UserControl = userControl ?? throw new ArgumentNullException(nameof(userControl));
            MainWindow = observer ?? throw new ArgumentNullException(nameof(observer));
            Pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));

            Add(observer);
        }

        public decimal TotalPedido
        {
            get => Pedido?.Produtos?.Sum(p => p.Valor) ?? 0;
        }

        public PagarPedidoCommand Pagar { get; private set; } = new();

    }
}
