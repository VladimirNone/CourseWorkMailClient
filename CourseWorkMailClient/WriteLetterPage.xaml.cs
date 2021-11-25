using CourseWorkMailClient.Domain;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TESTWPF.CustomControls;

namespace CourseWorkMailClient
{
    /// <summary>
    /// Interaction logic for WriteLetterPage.xaml
    /// </summary>
    public partial class WriteLetterPage : Page
    {
        private Page prevPage;
        private List<CustomMessage> curMessages { get; set; }
        private bool error { get; set; }
        private Action ChangeCaretPosition;

        public WriteLetterPage(Page previousPage)
        {
            prevPage = previousPage;
            InitializeComponent();

            colorOfBackgroundText.ColorChangedEvent += (newColor) =>
            {
                FormatText(TextElement.BackgroundProperty, newColor, new Run() { Background = newColor }, true, false);
            };

            colorOfForegroundText.ColorChangedEvent += (newColor) =>
            {
                FormatText(TextElement.ForegroundProperty, newColor, new Run() { Foreground = newColor }, false, true);
            };
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(prevPage);
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(prevPage);
        }

        private void ButtonAddAttachment_Click(object sender, RoutedEventArgs e)
        {
            var OPF = new OpenFileDialog();
            if ((bool)OPF.ShowDialog())
            {
                lbAttachments.Items.Add(new ListBoxItem() { Content = OPF.FileName });
            }
        }

        private void ButtonRemoveAttachment_Click(object sender, RoutedEventArgs e)
        {
            var selected = (ListBoxItem)lbAttachments.SelectedItem;
            lbAttachments.Items.Remove(selected);
        }

        private void bBold_Click(object sender, RoutedEventArgs e)
        {
            FormatText(TextElement.FontWeightProperty, FontWeights.Bold, new Run() { FontWeight = FontWeights.Bold }, isButton: true);
        }

        private void bItalic_Click(object sender, RoutedEventArgs e)
        {
            FormatText(TextElement.FontStyleProperty, FontStyles.Italic, new Run() { FontStyle = FontStyles.Italic }, isButton: true);
        }

        private void bUnderline_Click(object sender, RoutedEventArgs e)
        {
            FormatText(Inline.TextDecorationsProperty, TextDecorations.Underline, new Run() { TextDecorations = TextDecorations.Underline }, isButton: true);
        }


        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (ChangeCaretPosition != null)
            {
                ChangeCaretPosition();
                ChangeCaretPosition = null;
            }
        }

        private void FormatText(DependencyProperty property, object value, Run newRun, bool isBColor = false, bool isFColor = false, bool isButton = false)
        {
            if (rtbContent.Selection.Start != rtbContent.Selection.End)
            {
                TextRange searchRange = new TextRange(rtbContent.Selection.Start, rtbContent.Selection.End);
                searchRange.ApplyPropertyValue(property, value);

                if (isButton)
                {
                    ChangeCaretPosition = () =>
                    {
                        rtbContent.Focus();
                        rtbContent.CaretPosition = rtbContent.Selection.End;
                    };
                }
            }
            else if (rtbContent.CaretPosition == rtbContent.Selection.Start && rtbContent.CaretPosition == rtbContent.Selection.End)
            {
                ((Paragraph)rtbContent.Document.Blocks.LastBlock).Inlines.Add(newRun);

                if (isBColor)
                {
                    ColorBackgroundEvents.ColorPanelClosed = () =>
                    {
                        rtbContent.CaretPosition = newRun.ContentEnd;
                    };
                }

                if (isFColor)
                {
                    ColorBackgroundEvents.ColorPanelClosed = () =>
                    {
                        rtbContent.CaretPosition = newRun.ContentEnd;
                    };
                }

                if (isButton)
                {
                    ChangeCaretPosition = () =>
                    {
                        rtbContent.Focus();
                        rtbContent.CaretPosition = ((Paragraph)rtbContent.Document.Blocks.LastBlock).Inlines.LastInline.ContentEnd;
                    };
                }
            }
        }

    }
}
