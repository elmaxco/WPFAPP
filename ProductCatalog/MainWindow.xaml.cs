using System.Windows;
using Resources.Enums;
using System.Windows.Controls;
using Resources.Services;
using Resources.Models;

namespace ProductCatalog
{
    public partial class MainWindow : Window
    {
        private readonly IProductService _productService;
        private Product? _selectedProduct;

        public MainWindow()
        {
            InitializeComponent();
            _productService = new ProductService();
            LoadProductList();
        }

        private void LoadProductList()
        {
            ProductListView.ItemsSource = _productService.GetAllProducts();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var product = new Product
            {
                Name = ProductNameTextBox.Text,
                Price = decimal.TryParse(PriceTextBox.Text, out var price) ? price : 0,
                Category = ((ComboBoxItem)CategoryTextBox.SelectedItem)?.Content.ToString() ?? string.Empty
            };

            var result = _productService.AddToList(product);
            UpdateStatus(result);

            LoadProductList();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProduct != null)
            {
                _selectedProduct.Name = ProductNameTextBox.Text;
                _selectedProduct.Price = decimal.TryParse(PriceTextBox.Text, out var price) ? price : 0;
                _selectedProduct.Category = ((ComboBoxItem)CategoryTextBox.SelectedItem)?.Content.ToString() ?? string.Empty;

                _productService.DeleteProduct(_selectedProduct);
                _productService.AddToList(_selectedProduct);

                LoadProductList();
                UpdateStatus(ResultStatus.Success);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProduct is not null)
            {
                var result = _productService.DeleteProduct(_selectedProduct);
                UpdateStatus(result);

                LoadProductList();
            }
        }

        private void ProductListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ProductListView.SelectedItem is Product selectedProduct)
            {
                _selectedProduct = selectedProduct;
                ProductNameTextBox.Text = selectedProduct.Name;
                PriceTextBox.Text = selectedProduct.Price.ToString();
                CategoryTextBox.SelectedItem = CategoryTextBox.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == selectedProduct.Category);
            }
        }

        private void UpdateStatus(ResultStatus status)
        {
            switch (status)
            {
                case ResultStatus.Success:
                    StatusTextBlock.Text = "Operation successful!";
                    break;
                case ResultStatus.Exists:
                    StatusTextBlock.Text = "Product already exists!";
                    break;
                case ResultStatus.Failed:
                    StatusTextBlock.Text = "Operation failed!";
                    break;
                case ResultStatus.NotFound:
                    StatusTextBlock.Text = "Product not found!";
                    break;
                default:
                    StatusTextBlock.Text = "Unexpected result!";
                    break;
            }
        }
    }
}
