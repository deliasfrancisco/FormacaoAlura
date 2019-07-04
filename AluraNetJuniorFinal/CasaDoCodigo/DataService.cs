using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CasaDoCodigo
{
    class DataService : IDataService
    {
        private readonly ApplicationContext contexto;
        private readonly IProdutoRepository produtoRepository;
        private readonly ICategoriaRepository categoriaRepository;

        public DataService(ApplicationContext contexto,
            IProdutoRepository produtoRepository,
			ICategoriaRepository categoriaRepository)
        {
            this.contexto = contexto;
            this.produtoRepository = produtoRepository;
			this.categoriaRepository = categoriaRepository;

		}

        public async Task InicializaDB()
        {
            await contexto.Database.MigrateAsync();// usando o objeto de aplicação do contexto para conexão a base de dados usando as migrações para utiliza-las para montagem da estrutura da tabela

            List<Livro> livros = await GetLivros(); // passagem da leitura do json

            await produtoRepository.SaveProdutos(livros); //passando o objeto preenchido de livros para preenchimento da tabela
        }

        private static async Task<List<Livro>> GetLivros() //metodo de leitura do arquivo json
        {
            var json = await File.ReadAllTextAsync("livros.json"); //lendo arquivo json 
            var livros = JsonConvert.DeserializeObject<List<Livro>>(json); //converter json numa lista de objetos e desializando para uma classe de livro
            return livros;
        }
    }


}
