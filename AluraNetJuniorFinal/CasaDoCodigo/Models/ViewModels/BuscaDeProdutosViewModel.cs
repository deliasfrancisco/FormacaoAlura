using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Models.ViewModels
{
	public class BuscaDeProdutosViewModel
	{

		public IList<Produto> Produtos { get; set; } //lista de produtos a serem exibidos na view
        public string Pesquisa { get; set; } //propriedade para o texto de pesquisa
        public bool Encontrou;

		public BuscaDeProdutosViewModel(bool encontrou)
		{
			Encontrou = encontrou;
		}


		public BuscaDeProdutosViewModel(IList<Produto> produtos, bool encontrouResultados)
		{
			Produtos = produtos;
			Encontrou = encontrouResultados;
		}
	}
}
