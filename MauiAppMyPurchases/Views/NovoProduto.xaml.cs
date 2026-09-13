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
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Categoria = picker_categoria.SelectedItem?.ToString(),
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            await App.Db.Insert(p);

            await DisplayAlert("Sucesso", "Registro inserido", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
