using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IPedidoRepository pedidoRepository;
        private readonly IItemPedidoRepository itemPedidoRepository;
		private readonly ICategoriaRepository categoriaRepository;

        public PedidoController(IProdutoRepository produtoRepository,
            IPedidoRepository pedidoRepository,
            IItemPedidoRepository itemPedidoRepository,
			ICategoriaRepository categoriaRepository)
        {
            this.produtoRepository = produtoRepository;
            this.pedidoRepository = pedidoRepository;
            this.itemPedidoRepository = itemPedidoRepository;
			this.categoriaRepository = categoriaRepository;
        }

        public IActionResult Carrossel()
        {
            return View(produtoRepository.GetProdutos()); //passando a listagem de produtos para o controller
        }

        public async Task<IActionResult> Carrinho(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))  //verifica se o codigo do parametro veio preenchido ou não
            {
                await pedidoRepository.AddItem(codigo);
            }

            Pedido taskPedido = await pedidoRepository.GetPedido(); //metodo para receber o pedido atual da sessão
            List<ItemPedido> itens = taskPedido.Itens;
            CarrinhoViewModel carrinhoViewModel = new CarrinhoViewModel(itens);

            return base.View(carrinhoViewModel); // passanedo (retornando) para a view a soicitação de itens feita no repositorio
        }

        public async Task<IActionResult> Cadastro()
        {
            var pedido = await pedidoRepository.GetPedido();

            if (pedido == null)
            {
                return RedirectToAction("Carrossel"); //redirecionado a action de carrossel caso o pedido seja nulo, obrigando a ter uma pedido para a finalização
            }

            return View(pedido.Cadastro);
        }

        [HttpPost] //HttpPost -> restringe o acesso a funcões obrigatorias pela barra de endereços
        [ValidateAntiForgeryToken] //valida o token na hora de receber a chave de token, protege de acesso externo
        public async Task<IActionResult> Resumo(Cadastro cadastro)
        {
            if (ModelState.IsValid) //HttpPost -> restringe o acesso a funcões obrigatorias pela barra de endereços
            {
                return View(await pedidoRepository.UpdateCadastro(cadastro));
            }
            return RedirectToAction("Cadastro"); //caso o model não seja valido é redirecionado para a view cadastro
        }

        [HttpPost] //HttpPost -> //com o http post força os parametros a serem passados pelo corpo da pagina HTML, impedindo que os dados sejam passados pelo endereço do navagador
        [ValidateAntiForgeryToken] //valida o token na hora de receber a chave de token, protege de acesso externo
        public async Task<UpdateQuantidadeResponse> UpdateQuantidade([FromBody]ItemPedido itemPedido)
        {
            return await pedidoRepository.UpdateQuantidade(itemPedido);
        }

		public async Task<IActionResult> BuscaDeProdutos(string pesquisa) // criação da action para a view de bsuca de produtos
		{
			if (string.IsNullOrEmpty(pesquisa))
			{
				var viewBusca = new BuscaDeProdutosViewModel(await produtoRepository.GetProdutos(), true);
				return View(viewBusca);
			}

			if (produtoRepository.GetProdutos(pesquisa).Result.Count() == 0)
			{
				var viewBuscaSemResultado = new BuscaDeProdutosViewModel(false);
				return View(viewBuscaSemResultado);
			}

			return View(new BuscaDeProdutosViewModel(await produtoRepository.GetProdutos(pesquisa), true));
		}
	}
}
