using MauiAppMyPurchases;
using MauiAppMyPurchases.Models;

namespace MauiAppMyPurchases.Views;

// Página de edição de produto
// Comentários no estilo de aluno: explico o que cada parte faz de forma simples
public partial class EditarProduto : ContentPage
{
    // Construtor: inicializa os componentes XAML (campos, botões, etc.)
    public EditarProduto()
    {
        InitializeComponent();
    }

    // Função chamada quando clica no botão Salvar
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Pega o produto que foi selecionado
            Produto produto_anexado = BindingContext as Produto;

            // Cria o produto com os dados alterados
            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            // Atualiza os dados no banco
            await App.Db.Update(p);

            // Mostra uma mensagem avisando que deu certo
            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");

            // Volta para a tela da lista
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            // Mostra uma mensagem caso aconteça algum erro
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}