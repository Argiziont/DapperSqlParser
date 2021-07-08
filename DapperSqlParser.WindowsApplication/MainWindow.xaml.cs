using DapperSqlParser.Models;
using DapperSqlParser.Services;
using DapperSqlParser.WindowsApplication.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace DapperSqlParser.WindowsApplication
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ObservableCollection<StoredProcedureGridModel> _gridModels = new ObservableCollection<StoredProcedureGridModel>();

        public MainWindow()
        {
            InitializeComponent();
        }


        private void AddDetailsToStoredProcedureGrid(StoredProcedureGridModel gridModel)
        {
            _gridModels.Add(gridModel);
            RefreshDataGridWithNewItems();
        }

        private void RefreshDataGridWithNewItems()
        {
            ClearDataGrid();
            foreach (var dataChunk in _gridModels) StoredProceduresDataGrid.Items.Add(dataChunk);
            StoredProceduresDataGrid.Items.Refresh();
        }

        private void ClearDataGrid()
        {
            StoredProceduresDataGrid.Items.Clear();
            StoredProceduresDataGrid.Items.Refresh();
        }

        private void UnCheckAllCheckboxesInDataGrid()
        {
            foreach (var storedProcedureDetails in _gridModels) storedProcedureDetails.IsChecked = false;
            RefreshDataGridWithNewItems();
        }

        private void CheckAllCheckboxesInDataGrid()
        {
            foreach (var storedProcedureDetails in _gridModels) storedProcedureDetails.IsChecked = true;
            RefreshDataGridWithNewItems();
        }

        private string GetConnectionStringFromTextBox()
        {
            if (ConnectionStringTextBlock.Text.Length == 0)
                throw new ArgumentException(nameof(ConnectionStringTextBlock) + " was empty");

            return ConnectionStringTextBlock.Text;
        }

        private string GetNameSpaceStringFromTextBox()
        {
            if (NamespaceNameTextBlock.Text.Length == 0)
                throw new ArgumentException(nameof(NamespaceNameTextBlock) + " was empty");

            return NamespaceNameTextBlock.Text;
        }

        private StoredProcedureService GetStoreConnectedStoredProcedureService()
        {
            string connectionString;
            try
            {
                connectionString = GetConnectionStringFromTextBox();
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Couldn't get stored procedures names!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw new ArgumentException("Couldn't get stored procedures names!");

            }

            var storedProcedureService = new StoredProcedureService(connectionString);
            return storedProcedureService;
        }

        private async Task AddDetailsWithCheckBoxesListToDataGrid()
        {
            var storedProcedureService = GetStoreConnectedStoredProcedureService();
            var storedProceduresList = await storedProcedureService.GenerateModelsListAsync();

            _gridModels = new ObservableCollection<StoredProcedureGridModel>();

            foreach (var storedProcedure in storedProceduresList)
                AddDetailsToStoredProcedureGrid(new StoredProcedureGridModel
                {
                    IsChecked = false,
                    Title = storedProcedure.StoredProcedureInfo.Name,
                    InputCount = storedProcedure.InputParametersDataModels != null
                        ? storedProcedure.InputParametersDataModels.Length.ToString()
                        : "0",
                    OutputCount = storedProcedure.OutputParametersDataModels != null
                        ? storedProcedure.OutputParametersDataModels.Length.ToString()
                        : "0"
                });

            CreateAndSetupEventsForStoredProcedureDataGrid();
        }

        private async void AllStoredProceduresRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                await AddDetailsWithCheckBoxesListToDataGrid();
            }
            catch (ArgumentException) { return; }

            CheckAllCheckboxesInDataGrid();
        }

        private async void OnlyCheckedStoredProceduresRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                await AddDetailsWithCheckBoxesListToDataGrid();
            }
            catch (ArgumentException) { return; }

            UnCheckAllCheckboxesInDataGrid();
        }

        private void CreateAndSetupEventsForStoredProcedureDataGrid()
        {
            var cellStyle = new Style(typeof(DataGridCell));
            cellStyle.Setters.Add(new EventSetter(MouseEnterEvent,
                new MouseEventHandler(Cell_MouseEnter)));
            StoredProceduresDataGrid.CellStyle = cellStyle;
        }

        private async void Cell_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));

            var cell = e.Source as DataGridCell;

            if (!(cell?.DataContext is StoredProcedureGridModel cellDataContext))
                return;

            switch (cell.Column.Header.ToString())
            {
                case nameof(StoredProcedureGridModel.InputCount):
                    {
                        StoredProcedureService storedProcedureService;
                        try
                        {
                            storedProcedureService = GetStoreConnectedStoredProcedureService();
                        }
                        catch { return; }

                        var storeProcedureParameters = await storedProcedureService.GetSpDataAsync(cellDataContext.Title);
                        var inputTip = StoredProcedureParametersStringFormatter.FormatInputStoredProcedureParameters(storeProcedureParameters.InputParametersDataModels);

                        cell.ToolTip = inputTip;
                        break;
                    }
                case nameof(StoredProcedureGridModel.OutputCount):
                    {
                        StoredProcedureService storedProcedureService;
                        try
                        {
                            storedProcedureService = GetStoreConnectedStoredProcedureService();
                        }
                        catch { return; }

                        var storeProcedureParameters = await storedProcedureService.GetSpDataAsync(cellDataContext.Title);
                        var outputTip = StoredProcedureParametersStringFormatter.FormatOutputStoredProcedureParameters(storeProcedureParameters.OutputParametersDataModels);

                        cell.ToolTip = outputTip;
                        break;
                    }
            }
        }

        private async void GenerateOutputButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await FillOutputTextBoxWithGeneratedCode();
            }
            catch (ArgumentException)
            {
            }
        }

        private async Task FillOutputTextBoxWithGeneratedCode()
        {
            var generatedStoreProcedureOutput = await GenerateStoreProcedureOutput();

            GeneratedOutputRichTextBox.Document.Blocks.Clear();
            GeneratedOutputRichTextBox.Document.Blocks.Add(new Paragraph(new Run(generatedStoreProcedureOutput)));
        }

        private async Task<string> GenerateStoreProcedureOutput()
        {
            string nameSpaceName;

            try
            {
                nameSpaceName = GetNameSpaceStringFromTextBox();
            }
            catch
            {
                MessageBox.Show("Namespace was not specified!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw new ArgumentException("Namespace was not specified!");
            }

            var storedProcedureService = GetStoreConnectedStoredProcedureService();
            
            var checkedStoredProceduresDetails = await GetStoreProcedureGeneratedPureCode(storedProcedureService);

            var generatedStoreProcedureOutput =
                await StoredProceduresExtractor.CreateSpClient(checkedStoredProceduresDetails, nameSpaceName);
            return generatedStoreProcedureOutput;
        }

        private async Task<List<StoredProcedureParameters>> GetStoreProcedureGeneratedPureCode(StoredProcedureService storedProcedureService)
        {
            var checkedStoredProceduresNames = GetCheckedStoreProcedureNames();
            return await storedProcedureService.GenerateModelsListAsync(checkedStoredProceduresNames);
        }

        private string[] GetCheckedStoreProcedureNames()
        {
            return _gridModels.Where(storedProcedure => storedProcedure.IsChecked)
                .Select(checkedStoredProcedure => checkedStoredProcedure.Title).ToArray();
        }

        private async void SaveOutputToFileButton_Click(object sender, RoutedEventArgs e)
        {
            string resultStoreProcedureOutput;
            try
            {
                resultStoreProcedureOutput = await GenerateStoreProcedureOutput();
            }
            catch (ArgumentException)
            {
                return;
            }


            var saveFileDialog = CreateSaveDialogForGeneratedStoredProcedureCode();


            if (saveFileDialog.ShowDialog() == true)
                await File.WriteAllTextAsync(saveFileDialog.FileName, resultStoreProcedureOutput);
        }

        private static SaveFileDialog CreateSaveDialogForGeneratedStoredProcedureCode()
        {
            return new SaveFileDialog
            {
                FileName = "StoreProcedureClientOutput",
                Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs"
            };
        }
    }
}