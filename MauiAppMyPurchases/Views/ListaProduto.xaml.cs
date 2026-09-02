using MauiAppMyPurchases.Models;
using MauiAppMyPurchases;
using System.Collections.ObjectModel;
using System.Linq;

// Arquivo: ListaProduto.xaml.cs
// Objetivo: lógica por trás da página de listagem de produtos.
// Comentários explicativos: descrevo o comportamento para facilitar
// entendimento e manutenção por outros desenvolvedores.

namespace MauiAppMyPurchases.Views;

public partial class ListaProduto : ContentPage
{
    // ObservableCollection é usada para que o ListView atualize automaticamente
    // quando itens são adicionados ou removidos.
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        // Vincula a coleção ao controle ListView definido no XAML
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            // Ao aparecer a página, recarregamos os dados do banco
            lista.Clear();
            List<Produto> tmp = await App.Db.GetAll();
            // Adiciona um a um para disparar notificações da coleção
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Navega para a página de criação de novo produto
            Navigation.PushAsync(new Views.NovoProduto());

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;
            // Marca como atualizando para mostrar feedback ao usuário
            lst_produtos.IsRefreshing = true;

            // Limpa e busca novamente filtrando pelo texto digitado
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

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        // Calcula o total somando a propriedade Total de cada produto
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";

        // Mostra o resultado em um alerta simples
        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecinado = sender as MenuItem;
            Produto p = selecinado.BindingContext as Produto;
            // Pergunta confirmação antes de excluir
            bool confirm = await DisplayAlert(
                "Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                // Remove do banco e da lista (o ListView atualiza automaticamente)
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender,
        SelectedItemChangedEventArgs e)
    {
        try
        {
            // Quando um item é selecionado, navegamos para a tela de edição
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                // Passamos o produto como BindingContext para pre-preencher o formulário
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            // Refresh manual: limpa e recarrega do banco
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();
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
}