namespace UnoApp1.Presentation;

public sealed partial class ProductListPage : Page
{
    public ProductListViewModel? ViewModel => DataContext as ProductListViewModel;

    public ProductListPage()
    {
        this.InitializeComponent();
        this.Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Load products when page is loaded
        if (ViewModel?.LoadProductsCommand.CanExecute(null) == true)
        {
            await ViewModel.LoadProductsCommand.ExecuteAsync(null);
        }
    }
}

