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

        private async Task InitializeStoreProceduresGrid()
        {
            //Uncheck the header checkbox
            try
            {
                await AddDetailsWithCheckBoxesListToDataGrid();
            }
            catch (ArgumentException)
            {
                return;
            }
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
                        : "0",
                    InputTooltip = StoredProcedureParametersStringFormatter.FormatInputStoredProcedureParameters(storedProcedure.InputParametersDataModels),
                    OutputTooltip = StoredProcedureParametersStringFormatter.FormatOutputStoredProcedureParameters(storedProcedure.OutputParametersDataModels),
                    GeneralDetails = StoredProcedureParametersStringFormatter.FormatStoreProcedureInfo(storedProcedure.StoredProcedureInfo)

                });
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
                Filter = "C# file (*.cs)|*.cs|Text file (*.txt)|*.txt"
            };
        }

        private void StoredProceduresDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Ignored
            //Created to prevent the unauthorized crash

        }

        private void StoredProceduresDataGrid_OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            // Have to do this in the unusual case where the border of the cell gets selected.
            // and causes a crash 'EditItem is not allowed'

            var storedProcedureContext = e.Row.DataContext as StoredProcedureGridModel;
            InvertStoredProcedureCheckBox(_gridModels.FirstOrDefault(storedProcedure=>storedProcedure.Title== storedProcedureContext?.Title));
            RefreshDataGridWithNewItems();
            e.Cancel = true;
        }

        private void InvertStoredProcedureCheckBox(StoredProcedureGridModel model)
        {
            model.IsChecked = !model.IsChecked;
        }

        private void IsCheckedStoreProcedureSelector_OnChecked(object sender, RoutedEventArgs e)
        {
            CheckAllCheckboxesInDataGrid();
            RefreshDataGridWithNewItems();
        }
        private void IsCheckedStoreProcedureSelector_OnUnchecked(object sender, RoutedEventArgs e)
        {
            UnCheckAllCheckboxesInDataGrid();
            RefreshDataGridWithNewItems();
        }


        private async void LoadStoreProcedures_OnClick(object sender, RoutedEventArgs e)
        {
           await InitializeStoreProceduresGrid();
        }
    }
}