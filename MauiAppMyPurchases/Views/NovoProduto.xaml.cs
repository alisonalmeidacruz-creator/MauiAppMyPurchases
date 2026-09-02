using MauiAppMyPurchases.Models;

namespace MauiAppMyPurchases.Views;

public partial class NovoProduto : ContentPage
{
    // Página para inserir um novo produto no banco.
    public NovoProduto()
    {
        InitializeComponent();
    }

    // Manipulador do botão Salvar na toolbar
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Cria o objeto Produto a partir dos campos do formulário
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                // Convert.ToDouble pode lançar exceção se o texto for inválido;
                // por enquanto deixamos a validação simples para estudo.
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            // Insere no banco e informa sucesso ao usuário
            await App.Db.Insert(p);
            await DisplayAlert("sucesso", "registro inserido", "ok");
        }
        catch (Exception ex)
        {
            // Em caso de erro, mostramos mensagem (útil para debug ou quando o usuário
            // digita algo errado). Em produção, adicionar validações antes é recomendado.
            await DisplayAlert("Ops", ex.Message, "ok");
        }
    }
}
