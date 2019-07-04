using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
	public interface ICategoriaRepository
	{
		Task <Categoria> CategoriaProduto(string nome);  // metodo a ser implementado nas classes que usarem a interface
	}

	public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository // imlementação de interface e herdando de baseRepository
	{
		public CategoriaRepository(ApplicationContext contexto) : base(contexto)
		{
		}

		public async Task<Categoria> CategoriaProduto(string nome) // 10 - método para salvar categoria
        {
			var categoria = dbSet
				.Where(l => l.Nome == nome) //variavel que esta recebendo a instancia da conexão com a base de dados
				.SingleOrDefault(); // verificando se na consulta na tabela categoria se o nome da categoria já existe...
			
			if(categoria == null) // se o nome da categoria não existir...
			{
				var nova = new Categoria() { Nome = nome }; // a criado uma variavel que ira receber o nome da nova categoria
				categoria = dbSet.Add(nova).Entity; //passando a variavel por parametro para ser adicionada no banco
			}
			await contexto.SaveChangesAsync(); //salvando as mudanças
			return categoria; // retronando a categoria ja preenchida e salva
		}
	}

}

