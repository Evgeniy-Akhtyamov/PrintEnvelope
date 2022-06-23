using PrintEnvelope.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PrintEnvelope.Views
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private const double PIXEL_FACTOR = 3.793627;

        IMainViewModel _mainViewModel;
        public MainView(IMainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            InitializeComponent();
            DataContext = _mainViewModel;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
            base.OnClosing(e);
        }

        /// <summary>
        /// Печать конверта
        /// </summary>
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.PrintTicket.PageMediaSize = new PageMediaSize(ConvertDistance(EnvelopeHeightBox.Text), ConvertDistance(EnvelopeWidthBox.Text)); 
                printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;

                if (printDialog.ShowDialog() == true)
                {
                    // Создаем визуальный элемент для страницы
                    DrawingVisual visual = new DrawingVisual();

                    // Получаем контекст рисования
                    using (DrawingContext dc = visual.RenderOpen())
                    {
                        var whoFromText = CreateText(whoFromBox.Text, "Calibri", 18, ConvertDistance(whoFromBoxWidth.Text));
                        dc.DrawText(whoFromText, new Point(ConvertDistance(whoFromBoxSideSet.Text), ConvertDistance(whoFromBoxTopSet.Text))); 

                        var whereFromText = CreateText(whereFromBox.Text, "Calibri", 18, ConvertDistance(whereFromBoxWidth.Text));
                        dc.DrawText(whereFromText, new Point(ConvertDistance(whereFromBoxSideSet.Text), ConvertDistance(whereFromBoxTopSet.Text))); 

                        var indexFromText = CreateText(indexFromBox.Text, "Calibri", 18, 150);
                        dc.DrawText(indexFromText, new Point(ConvertDistance(indexFromBoxSideSet.Text), ConvertDistance(indexFromBoxTopSet.Text)));  

                        var whoText = CreateText(whoBox.Text, "Calibri", 18, ConvertDistance(whoBoxWidth.Text));
                        dc.DrawText(whoText, new Point(ConvertDistance(whoBoxSideSet.Text), ConvertDistance(whoBoxTopSet.Text)));                                                                                                                                   

                        var whereText = CreateText(whereBox.Text, "Calibri", 18, ConvertDistance(whereBoxWidth.Text));
                        dc.DrawText(whereText, new Point(ConvertDistance(whereBoxSideSet.Text), ConvertDistance(whereBoxTopSet.Text))); 

                        var indexText = CreateText(indexBox.Text, "Calibri", 18, 150);
                        dc.DrawText(indexText, new Point(ConvertDistance(indexBoxSideSet.Text), ConvertDistance(indexBoxTopSet.Text)));  

                        var indexTextBig = CreateText(indexBox.Text, "Pechkin", 45, 300);
                        indexTextBig.SetFontWeight(FontWeights.Bold);

                        indexBox.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#Pericles Light");

                        dc.DrawText(indexTextBig, new Point(ConvertDistance(bigIndexBoxSideSet.Text), ConvertDistance(bigIndexBoxTopSet.Text))); 
                    }
                    // Печатаем визуальный элемент. 
                    printDialog.PrintVisual(visual, "Печать с помощью классов визуального уровня");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Создание текста с необходимыми параметрами для печати
        /// </summary>
        private FormattedText CreateText(string textLine, string typefaceName, int size, double textWidth)
        {
            // Определяем текст, который необходимо печатать
            FormattedText text = new FormattedText(
                textLine,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(new FontFamily(new Uri("pack://application:,,,/"), $"./Fonts/#{typefaceName}"), FontStyle, FontWeight, FontStretch), 
                size, 
                Brushes.Black, 
                1.25);

            // Максимальная ширина, в пределах которой выполнять перенос текста, 
            text.MaxTextWidth = textWidth;
            return text;
        }

        /// <summary>
        /// Преобразование отступов и расстояний из мм в пиксели
        /// </summary>
        private double ConvertDistance(string distanceLine)
        {
            double distance = 0;
            try
            {
                distance = Convert.ToDouble(distanceLine) * PIXEL_FACTOR;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
            return distance;
        }
    }
}
