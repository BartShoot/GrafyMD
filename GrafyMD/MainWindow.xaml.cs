using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Storage.Pickers;
using WinRT;

namespace GrafyMD
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            int vertexCount = (int)VertexCount.Value;
            double propability = ConnectionPropability.Value;
            int[,] connectionMatrix = new int[vertexCount, vertexCount];
            double[,] weightMatrix = new double[vertexCount, vertexCount];
            output.Text = $"Raport wygenerowany {DateTime.Now}\n\n";
            output.Text += $"Ilość wierzchołków grafu: {vertexCount}\n";
            output.Text += $"Prawdopodobieństwo istnienia krawędzi:\n{propability}\n\n";
            GenerateGraph(connectionMatrix, vertexCount, propability, weightMatrix);
        }

        private void GenerateGraph(int[,] connectionMatrix, int vertexCount, double propability, double[,] weightMatrix)
        {
            Cycles cycle = new();
            Random random = new();

            Functions.ResetMatrix(connectionMatrix);

            output.Text += "\n\nKrawędzie grafu:\n";
            var temp = Functions.CreateVertexesAndSetWeight(vertexCount, propability, random, connectionMatrix, weightMatrix);
            SendToOutput(temp.text);
            ConnectionStats.Text = "Ilość połączeń: " + temp.stat;

            output.Text += "\n\nMacierz grafu:\n";
            SendToOutput(Functions.DrawGraph(connectionMatrix));

            output.Text += "\n\nCykle długości 3:\n";
            temp = Functions.FindCycle3(connectionMatrix, vertexCount, weightMatrix, cycle);
            SendToOutput(temp.text);
            Cycles3Stats.Text = "Ilość cykli 3: " + temp.stat;

            output.Text += "\n\nCykle długości 4:\n";
            temp = Functions.FindCycle4(connectionMatrix, vertexCount, weightMatrix, cycle);
            SendToOutput(temp.text);
            Cycles4Stats.Text = "Ilość cykli 4: " + temp.stat;

            output.Text += "\n\nAutorzy raportu:" +
                "\nBartosz Dobija" +
                "\nKrzysztof Szczypka" +
                "\nTomasz Korzeniowski";
        }

        public void SendToOutput(string str)
        {
            output.Text += str;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new();
            savePicker.FileTypeChoices.Add("Plik tekstowy", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "Raport";
            var hwnd = this.As<IWindowNative>().WindowHandle;
            var initializeWithWindow = savePicker.As<IInitializeWithWindow>();
            initializeWithWindow.Initialize(hwnd);
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                await Windows.Storage.FileIO.WriteTextAsync(file, output.Text);
            }
        }

        [ComImport]
        [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IInitializeWithWindow
        {
            void Initialize(IntPtr hwnd);
        }
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
        internal interface IWindowNative
        {
            IntPtr WindowHandle { get; }
        }
    }
}
