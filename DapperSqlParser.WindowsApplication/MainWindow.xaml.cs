using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using DapperSqlParser.Models;
using DapperSqlParser.Services;
using DapperSqlParser.WindowsApplication.Models;
using Microsoft.Win32;


namespace DapperSqlParser.WindowsApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private List<StoredProcedureGridModel> _gridModels= new List<StoredProcedureGridModel>();

        public MainWindow()
        {
            InitializeComponent();

        }


        private void AddDetailsToStoredProcedureGrid(StoredProcedureGridModel gridModel)
        {
            _gridModels.Add(gridModel);
            StoredProceduresDataGrid.ItemsSource = _gridModels;
            StoredProceduresDataGrid.Items.Refresh();
        }

        private void RefreshDataGrid()
        {
            StoredProceduresDataGrid.ItemsSource = _gridModels;
            StoredProceduresDataGrid.Items.Refresh();
        }


        private void UnCheckAllCheckboxesInDataGrid()
        {
            foreach (var storedProcedureDetails in _gridModels)
            {
                storedProcedureDetails.IsChecked = false;
            }
            RefreshDataGrid();
        }
        private void CheckAllCheckboxesInDataGrid()
        {
            foreach (var storedProcedureDetails in _gridModels)
            {
                storedProcedureDetails.IsChecked = true;
            }
            RefreshDataGrid();
        }

        private string GetConnectionStringFromTextBox()
        {
            if (ConnectionStringTextBlock.Text.Length==0)
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
            catch
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

            _gridModels = new List<StoredProcedureGridModel>();

            foreach (var storedProcedure in storedProceduresList)
            {
                AddDetailsToStoredProcedureGrid(new StoredProcedureGridModel()
                {
                    IsChecked = false,
                    Title = storedProcedure.StoredProcedureInfo.Name,
                    InputCount = storedProcedure.InputParametersDataModels != null ? storedProcedure.InputParametersDataModels.Length.ToString():"0",
                    OutputCount = storedProcedure.OutputParametersDataModels != null ? storedProcedure.OutputParametersDataModels.Length.ToString() : "0",
                });
            }

            CreateAndSetupEventsForStoredProcedureDataGrid();
        }
        private async void AllStoredProceduresRadioButton_Checked(object sender, RoutedEventArgs e)
        {

            await AddDetailsWithCheckBoxesListToDataGrid();
            CheckAllCheckboxesInDataGrid();
        }

        private async void OnlyCheckedStoredProceduresRadioButton_Checked(object sender, RoutedEventArgs e)
        {

            await AddDetailsWithCheckBoxesListToDataGrid();
            UnCheckAllCheckboxesInDataGrid();

        }

        private void CreateAndSetupEventsForStoredProcedureDataGrid()
        {
            var cellStyle = new Style(typeof(DataGridCell));
            cellStyle.Setters.Add(new EventSetter(DataGridRow.MouseEnterEvent,
                new MouseEventHandler(Row_MouseEnter)));
            StoredProceduresDataGrid.CellStyle = cellStyle;
        }

        private async void Row_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));

            var cell = e.Source as DataGridCell;

            if (!(cell.DataContext is StoredProcedureGridModel cellDataContext))
                return;

            switch (cell.Column.Header.ToString())
            {
                case nameof(StoredProcedureGridModel.InputCount):
                {
                    var storedProcedureService = GetStoreConnectedStoredProcedureService();
                    var storeProcedureParameters= await storedProcedureService.GetSpDataAsync(cellDataContext.Title);
                    var inputTip = FormatInputStoredProcedureParameters(storeProcedureParameters.InputParametersDataModels);

                    cell.ToolTip = inputTip;
                    break;
                }
                case nameof(StoredProcedureGridModel.OutputCount):
                {
                    var storedProcedureService = GetStoreConnectedStoredProcedureService();
                    var storeProcedureParameters = await storedProcedureService.GetSpDataAsync(cellDataContext.Title);
                    var outputTip = FormatOutputStoredProcedureParameters(storeProcedureParameters.OutputParametersDataModels);

                    cell.ToolTip = outputTip;
                    break;
                }
            }
        }

        private string FormatInputStoredProcedureParameters(InputParametersDataModel[] inputParameters)
        {
            return inputParameters == null
                ? "Input parameters are empty"
                : inputParameters.Aggregate("",
                    (current, inputParameter) =>
                        current + (inputParameter.ParameterName + " " + inputParameter.TypeName + " \n"));
        }

        private string FormatOutputStoredProcedureParameters(OutputParametersDataModel[] outputParameters)
        {
            return outputParameters == null
                ? "Output parameters are empty"
                : outputParameters.Aggregate("",
                    (current, outputParameter) =>
                        current + (outputParameter.ParameterName + " " + outputParameter.TypeName + " \n"));
        }

        private async void GenerateOutputButton_Click(object sender, RoutedEventArgs e)
        {
            await GetGeneratedStoreProcedureData();
        }

        private async Task GetGeneratedStoreProcedureData()
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

            var checkedStoredProceduresNames = _gridModels.Where(storedProcedure => storedProcedure.IsChecked)
                .Select(checkedStoredProcedure => checkedStoredProcedure.Title).ToArray();
            var checkedStoredProceduresDetails =
                await storedProcedureService.GenerateModelsListAsync(checkedStoredProceduresNames);

            var generatedStoreProcedureOutput =
                await StoredProceduresExtractor.CreateSpClient(checkedStoredProceduresDetails, nameSpaceName);
            return generatedStoreProcedureOutput;
        }

        private async void SaveOutputToFileButton_Click(object sender, RoutedEventArgs e)
        {
            var resultStoreProcedureOutput = await GenerateStoreProcedureOutput();
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = "StoreProcedureClientOutput";
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";

            if (saveFileDialog.ShowDialog() == true)
                await File.WriteAllTextAsync(saveFileDialog.FileName, resultStoreProcedureOutput);
        }
    }
}
