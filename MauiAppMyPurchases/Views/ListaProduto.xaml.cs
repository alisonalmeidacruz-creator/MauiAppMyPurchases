using MauiAppMyPurchases.Models;
using MauiAppMyPurchases;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiAppMyPurchases.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;

        // Começa mostrando todas as categorias
        picker_categoria.SelectedIndex = 0;
    }

    protected async override void OnAppearing()
    {
        try
        {
            await CarregarProdutos();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Carrega os produtos do banco e aplica o filtro de categoria
    private async Task CarregarProdutos()
    {
        lista.Clear();

        List<Produto> produtos = await App.Db.GetAll();

        string categoria = picker_categoria.SelectedItem?.ToString();

        if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
        {
            produtos = produtos
                .Where(p => p.Categoria == categoria)
                .ToList();
        }

        foreach (Produto produto in produtos)
        {
            lista.Add(produto);
        }
    }

    // Adiciona um novo produto
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Pesquisa os produtos pela descrição
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    // Filtra os produtos pela categoria escolhida
    private async void picker_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            await CarregarProdutos();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Soma o total dos produtos
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";

        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    // Remove um produto
    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;

            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert(
                "Tem Certeza?",
                $"Remover {p.Descricao}?",
                "Sim",
                "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);

                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Abre a tela de edição do produto
    private void lst_produtos_ItemSelected(
        object sender,
        SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Atualiza a lista quando o usuário puxa para baixo
    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            await CarregarProdutos();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }
}