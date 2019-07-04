using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
		private readonly ICategoriaRepository categoriaRepository; // 13 - método para salvar categoria

        public ProdutoRepository(ApplicationContext contexto,
			ICategoriaRepository categoriaRepository) : base(contexto)
        {
			this.categoriaRepository = categoriaRepository;
        }

        public async Task<IList<Produto>> GetProdutos()
        {
			return await dbSet.Include(p => p.Categoria).ToListAsync();
		}

		public async Task<IList<Produto>> GetProdutos(string pesquisa) // overload para aceitar uma string de pesquisa
		{
			IQueryable<Produto> retorno = dbSet.Include(p => p.Categoria); //fazendo a consulta a categoria

			if (!string.IsNullOrWhiteSpace(pesquisa)) //eliminação de espaços na string passada
			{
				retorno = retorno.Where(p => p.Categoria.Nome.Contains(pesquisa) || p.Nome.Contains(pesquisa)); // verificando a variavel pesquisa tem o que a consulta de retorno tem
			}

			return await retorno.ToListAsync(); // retornando a variavel 

		}

		public async Task SaveProdutos(List<Livro> livros)
        {
			var categoria = new Categoria();

            foreach (var livro in livros)
            {
                if (!dbSet.Where(p => p.Codigo == livro.Codigo).Any())
                { // na leitura de cada livro do objeto 
					categoria = await categoriaRepository.CategoriaProduto(livro.Categoria);
                    dbSet.Add(new Produto(livro.Codigo, livro.Nome, livro.Preco, categoria));
                }
            }
            await contexto.SaveChangesAsync();
        }
    }

    public class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public decimal Preco { get; set; }
    }
}
