using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTWPF.CustomControls
{
    public partial class ColorForegroundEvents
    {
        public static Action ColorPanelClosed;

        public void PopupClosed_Closed(object sender, EventArgs e)
        {
            if (ColorPanelClosed != null)
            {
                ColorPanelClosed();
                ColorPanelClosed = null;
            }
        }
    }
}
