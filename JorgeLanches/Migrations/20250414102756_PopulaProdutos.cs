using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JorgeLanches.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

           
            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Coca-Cola', 'Coca lata 350ml', '5.50', 'CocaLata.jpg', '50', NOW(), 1);");

            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Pepsi', 'Pepsi lata 350ml', '5.00', 'PepsiLata.jpg', '40', NOW(), 1);");

            
            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Hamburguer', 'Hamburguer de carne', '7.99', 'Hamburguer.jpg', '30', NOW(), 2);");

            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Batata Frita', 'Porção de batata frita', '3.50', 'BatataFrita.jpg', '20', NOW(), 2);");

            
            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Sorvete', 'Sorvete de chocolate', '4.99', 'Sorvete.jpg', '15', NOW(), 3);");

            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Bolo de Chocolate', 'Bolo de chocolate caseiro', '8.50', 'BoloChocolate.jpg', '10', NOW(), 3);");

            
            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Água Mineral', 'Água mineral 500ml', '2.00', 'AguaMineral.jpg', '100', NOW(), 1);");

            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Suco de Laranja', 'Suco natural de laranja', '4.99', 'SucoLaranja.jpg', '30', NOW(), 1);");

            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Café', 'Café puro', '3.50', 'Cafe.jpg', '50', NOW(), 1);");

            
            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Sanduíche de Frango', 'Sanduíche de frango grelhado', '6.99', 'SanduicheFrango.jpg', '25', NOW(), 2);");

            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Pizza Margherita', 'Pizza de queijo e tomate', '11.50', 'Pizza.jpg', '15', NOW(), 2);");

            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Batata Chips', 'Batata chips salgada', '2.99', 'BatataChips.jpg', '40', NOW(), 2);");

            
            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Sorvete de Morango', 'Sorvete de morango cremoso', '5.50', 'SorveteMorango.jpg', '20', NOW(), 3);");

            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Torta de Limão', 'Torta de limão com merengue', '7.99', 'TortaLimao.jpg', '10', NOW(), 3);");

            migrationBuilder.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImagemUrl, QtdEstoque, DataCadastro, CategoriaId)" +
                               "VALUES ('Pudim', 'Pudim de leite condensado', '4.50', 'Pudim.jpg', '15', NOW(), 3);");



        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from Produtos");
        }
    }
}
