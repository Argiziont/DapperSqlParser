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
using DapperSqlParser.StoredProcedureCodeGeneration;

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
            try
            {
                await AddDetailsWithCheckBoxesListToDataGrid();
            }
            catch (ArgumentException)
            {
                //Ignored
            }
        }


        private void AddDetailsToStoredProcedureGrid(StoredProcedureGridModel gridModel)
        {
            _gridModels.Add(gridModel);
            RefreshDataGridWithNewItems(_gridModels);
        }

        private void RefreshDataGridWithNewItems(IEnumerable<StoredProcedureGridModel> storedProcedureModels)
        {
            ClearDataGrid();
            foreach (StoredProcedureGridModel dataChunk in storedProcedureModels) StoredProceduresDataGrid.Items.Add(dataChunk);
        }

        private void ClearDataGrid()
        {
            StoredProceduresDataGrid.Items.Clear();
            StoredProceduresDataGrid.Items.Refresh();
        }

        private void UnCheckAllCheckboxesInDataGrid()
        {
            foreach (object storedProcedureDetails in StoredProceduresDataGrid.Items) ((StoredProcedureGridModel) storedProcedureDetails).IsChecked = false;
            RefreshDataGridWithNewItems(GetCurrentGridItemCollection());
        }

        private void CheckAllCheckboxesInDataGrid()
        {
            foreach (object storedProcedureDetails in StoredProceduresDataGrid.Items) ((StoredProcedureGridModel)storedProcedureDetails).IsChecked = true;
            RefreshDataGridWithNewItems(GetCurrentGridItemCollection());
        }

        private IEnumerable<StoredProcedureGridModel> GetCurrentGridItemCollection()
        {
            return StoredProceduresDataGrid.Items.OfType<StoredProcedureGridModel>().ToList();
        }

        private Task<string> GetConnectionStringFromTextBox()
        {
            if (ConnectionStringTextBlock.Text.Length == 0)
                throw new ArgumentException(nameof(ConnectionStringTextBlock) + " was empty");

            return Task.FromResult(ConnectionStringTextBlock.Text);
        }

        private Task<string> GetNameSpaceStringFromTextBox()
        {
            if (NamespaceNameTextBlock.Text.Length == 0)
                throw new ArgumentException(nameof(NamespaceNameTextBlock) + " was empty");

            return Task.FromResult(NamespaceNameTextBlock.Text);
        }

        private async Task<StoredProcedureService> GetStoreConnectedStoredProcedureService()
        {
            string connectionString;
            try
            {
                connectionString = await GetConnectionStringFromTextBox();
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Couldn't get stored procedures names!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw new ArgumentException("Couldn't get stored procedures names!");

            }

            StoredProcedureService storedProcedureService = new StoredProcedureService(connectionString);
            return storedProcedureService;
        }

        private async Task AddDetailsWithCheckBoxesListToDataGrid()
        {
            var storedProcedureService = GetStoreConnectedStoredProcedureService();
            var storedProceduresList = await (await storedProcedureService).GenerateModelsListAsync();

            _gridModels = new ObservableCollection<StoredProcedureGridModel>();

            foreach (StoredProcedureParameters storedProcedure in storedProceduresList)
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
            ShowProgressIndicator();
            try
            {
                await FillOutputTextBoxWithGeneratedCode();
            }
            catch (ArgumentException)
            {
                //Ignored
            }
        }

        private async Task FillOutputTextBoxWithGeneratedCode()
        {
            string generatedStoreProcedureOutput = await GenerateStoreProcedureOutput();

            GeneratedOutputRichTextBox.Document.Blocks.Clear();
            GeneratedOutputRichTextBox.Document.Blocks.Add(new Paragraph(new Run(generatedStoreProcedureOutput)));
        }

        private async Task<string> GenerateStoreProcedureOutput()
        {
            string nameSpaceName;

            try
            {
                nameSpaceName = await GetNameSpaceStringFromTextBox();
            }
            catch
            {
                MessageBox.Show("Namespace was not specified!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw new ArgumentException("Namespace was not specified!");
            }

            StoredProcedureService storedProcedureService = await GetStoreConnectedStoredProcedureService();
            
            var checkedStoredProceduresDetails = await GetStoreProcedureGeneratedPureCode(storedProcedureService);

            var progressIndicator = GetProgressIndicatorForStoreProcedureExtractor();

            string generatedStoreProcedureOutput =
                await StoredProceduresCodeGenerator.CreateSpClient(checkedStoredProceduresDetails, nameSpaceName, progressIndicator);
            return generatedStoreProcedureOutput;
        }

        private Progress<StoreProcedureGenerationProgress> GetProgressIndicatorForStoreProcedureExtractor()
        {
            var progressIndicator = new Progress<StoreProcedureGenerationProgress>(ReportProgress);
            progressIndicator.ProgressChanged += (sender, progress) =>
            {
                if (progress.CurrentProgressAmount== progress.TotalProgressAmount) HideProgressIndicator();
            };

            return progressIndicator;
        }

        private void HideProgressIndicator()
        {
            StoredProcedureParsingProgressBar.Visibility = Visibility.Hidden;
        }

        private void ShowProgressIndicator()
        {
            StoredProcedureParsingProgressBar.Visibility = Visibility.Visible;
        }
        private void ReportProgress(StoreProcedureGenerationProgress progress)
        {
            StoredProcedureParsingProgressBar.Maximum = progress.TotalProgressAmount;
            StoredProcedureParsingProgressBar.Value = progress.CurrentProgressAmount;
        }


        private async Task<List<StoredProcedureParameters>> GetStoreProcedureGeneratedPureCode(StoredProcedureService storedProcedureService)
        {
            string[] checkedStoredProceduresNames = GetCheckedStoreProcedureNames();
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


            SaveFileDialog saveFileDialog = CreateSaveDialogForGeneratedStoredProcedureCode();


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


        private void StoredProceduresDataGrid_OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            // Have to do this in the unusual case where the border of the cell gets selected.
            // and causes a crash 'EditItem is not allowed'

            if (e.Column is DataGridCheckBoxColumn)
            {
                EditCheckBoxColumn(e);
            }
        }

        private void EditCheckBoxColumn(DataGridBeginningEditEventArgs e)
        {
            StoredProcedureGridModel storedProcedureContext = e.Row.DataContext as StoredProcedureGridModel;
            InvertStoredProcedureCheckBox(GetCurrentGridItemCollection().FirstOrDefault(storedProcedure =>
                storedProcedure.Title == storedProcedureContext?.Title));

            RefreshDataGridWithNewItems(GetCurrentGridItemCollection());
            e.Cancel = true;
        }

        private void InvertStoredProcedureCheckBox(StoredProcedureGridModel model)
        {
            model.IsChecked = !model.IsChecked;
        }

        private void IsCheckedStoreProcedureSelector_OnChecked(object sender, RoutedEventArgs e)
        {
            CheckAllCheckboxesInDataGrid();
        }
        private void IsCheckedStoreProcedureSelector_OnUnchecked(object sender, RoutedEventArgs e)
        {
            UnCheckAllCheckboxesInDataGrid();
        }


        private async void LoadStoreProcedures_OnClick(object sender, RoutedEventArgs e)
        {
           await InitializeStoreProceduresGrid();
        }

        private void StoredProcedureDataGridSearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox searchTextBox)
            {
                RefreshDataGridWithNewItems(_gridModels.Where(storedProcedure => storedProcedure.Title.Contains(searchTextBox.Text)));
            }
        }
    }
}