using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DapperSqlParser.Models;
using DapperSqlParser.Services;
using DapperSqlParser.WindowsApplication.Models;


namespace DapperSqlParser.WindowsApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Thickness _defaultStoredProceduresCheckBoxMargin = new Thickness(0, 10, 0, 0);
        private List<CheckBox> StoredProcedureCheckBoxes;
        private readonly int _defaultStoredProceduresCheckBoxWidth = 209;
        private readonly int _defaultStoredProceduresCheckBoxMarginSpacing = 20;
        private readonly List<StoredProcedureGridModel> _gridModels= new List<StoredProcedureGridModel>();

        public MainWindow()
        {
            InitializeComponent();
        }

        void StoredProcedureCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox storedProcedureCheckBox)
            {
                AddDetailsToStoredProcedureGrid(new StoredProcedureGridModel(){Title = storedProcedureCheckBox.Content.ToString()});
            }
            
        }
        void StoredProcedureCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox storedProcedureCheckBox)
            {
                RemoveDetailsToStoredProcedureGrid(new StoredProcedureGridModel() { Title = storedProcedureCheckBox.Content.ToString() });
            }

        }

        private void AddDetailsToStoredProcedureGrid(StoredProcedureGridModel gridModel)
        {
            _gridModels.Add(gridModel);
            StoredProceduresDataGrid.ItemsSource = _gridModels;
            StoredProceduresDataGrid.Items.Refresh();
        }

        private void RemoveDetailsToStoredProcedureGrid(StoredProcedureGridModel gridModel)
        {
            var itemToRemove = _gridModels.SingleOrDefault(r => r.Title == gridModel.Title);
            _gridModels.Remove(itemToRemove);
            StoredProceduresDataGrid.ItemsSource = _gridModels;
            StoredProceduresDataGrid.Items.Refresh();
        }

        private void MakeCheckBoxListScrollViewHidden()
        {
            if (CheckBoxListGrid!=null) CheckBoxListScrollView.Visibility = Visibility.Hidden;
        }
        private void MakeCheckBoxListScrollViewVisible()
        {
            if (CheckBoxListGrid != null) CheckBoxListScrollView.Visibility = Visibility.Visible;
        }
        private List<CheckBox> CreateStoredProceduresCheckBoxesList(params string[] storedProceduresNames)
        {
            return storedProceduresNames.Select((storedProceduresName, checkBoxIndex) => new CheckBox()
                {
                    Content = storedProceduresName,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness()
                    {
                         Top = _defaultStoredProceduresCheckBoxMargin.Top+ _defaultStoredProceduresCheckBoxMarginSpacing* checkBoxIndex,
                         Bottom = _defaultStoredProceduresCheckBoxMargin.Bottom,
                         Left = _defaultStoredProceduresCheckBoxMargin.Left,
                         Right = _defaultStoredProceduresCheckBoxMargin.Right
                    },
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = _defaultStoredProceduresCheckBoxWidth
                })
                .ToList();
        }

        private void DisableCheckBoxList(List<CheckBox> checkBoxes)
        {
            foreach (var checkBox in checkBoxes)
            {
                checkBox.IsEnabled = false;
            }
        }
        private void EnableCheckBoxList(List<CheckBox> checkBoxes)
        {
            foreach (var checkBox in checkBoxes)
            {
                checkBox.IsEnabled = true;
            }
        }
        private void CheckAllCheckBoxList(List<CheckBox> checkBoxes)
        {
            foreach (var checkBox in checkBoxes)
            {
                checkBox.IsChecked = true;
            }
        }
        private void UnCheckAllCheckBoxList(List<CheckBox> checkBoxes)
        {
            foreach (var checkBox in checkBoxes)
            {
                checkBox.IsChecked = false;
            }
        }

        private string GetConnectionStringFromTextBox()
        {
            if (ConnectionStringTextBlock.Text.Length==0)
                throw new ArgumentException(nameof(ConnectionStringTextBlock) + " was empty");

            return ConnectionStringTextBlock.Text;
        }

        private async Task<string[]> GetStoredProceduresViaConnectionString()
        {
            string connectionString;
            try
            {
                connectionString= GetConnectionStringFromTextBox();
            }
            catch
            {
                MessageBox.Show("Couldn't get stored procedures names!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw new ArgumentException("Couldn't get stored procedures names!");
            }

            var storedProcedureService = new StoredProcedureService(connectionString);
            var storedProceduresList = await storedProcedureService.GetSpListAsync();

            return storedProceduresList.Select(sp=> sp.Name).ToArray();

        }
        private void AddCheckBoxesListToGrid(Panel grid, IEnumerable<CheckBox> checkBoxes)
        {
            if (grid.Children.Count!=0) ClearGridChildren(grid);

            foreach (var checkBox in checkBoxes) grid.Children.Add(checkBox);
        }

        private void ClearGridChildren(Panel grid)
        {
            grid.Children.Clear();
        }
        private async void AllStoredProceduresRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            await CreateAndSetupCheckBoxesForStoredProcedures();
            DisableCheckBoxList(StoredProcedureCheckBoxes);
            CheckAllCheckBoxList(StoredProcedureCheckBoxes);

            AddCheckBoxesListToGrid(CheckBoxListGrid, StoredProcedureCheckBoxes);

            MakeCheckBoxListScrollViewVisible();
        }

        private async void OnlyCheckedStoredProceduresRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            await CreateAndSetupCheckBoxesForStoredProcedures();
            EnableCheckBoxList(StoredProcedureCheckBoxes);
            UnCheckAllCheckBoxList(StoredProcedureCheckBoxes);

            AddCheckBoxesListToGrid(CheckBoxListGrid, StoredProcedureCheckBoxes);

            MakeCheckBoxListScrollViewVisible();
        }

        private async Task CreateAndSetupCheckBoxesForStoredProcedures()
        {
            var storedProceduresNames = await GetStoredProceduresViaConnectionString();
            StoredProcedureCheckBoxes = CreateStoredProceduresCheckBoxesList(storedProceduresNames);
            AddCheckAndUncheckEventHandlersToStoredProceduresCheckBoxes();
        }

        private void AddCheckAndUncheckEventHandlersToStoredProceduresCheckBoxes()
        {
            foreach (var storedProcedureCheckBox in StoredProcedureCheckBoxes)
            {
                storedProcedureCheckBox.AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(StoredProcedureCheckBox_Checked));
                storedProcedureCheckBox.AddHandler(ToggleButton.UncheckedEvent, new RoutedEventHandler(StoredProcedureCheckBox_UnChecked));
            }
        }
    }
}
