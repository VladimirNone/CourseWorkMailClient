using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace CourseWorkMailClient.Domain
{
    public class LightRun
    {
        public Brush Foreground { get; set; }
        public Brush Background { get; set; }
        public FontWeight FontWeight { get; set; }
        public FontStyle FontStyle { get; set; }
        public TextDecorationCollection TextDecorations { get; set; }
        public double FontSize { get; set; }
        public string Text { get; set; }
    }
}
