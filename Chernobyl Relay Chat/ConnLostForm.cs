using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    public partial class ConnLostForm : Form
    {
        public static ConnLostForm staticVar = null;

        public ConnLostForm()
        {
            InitializeComponent();
            staticVar = this;
            Text = CRCStrings.Localize("crc_connlost");
            SignalLost.Text = CRCStrings.Localize("crc_connlost");
        }

        private void ConnLostForm_Load(object sender, EventArgs e)
        {

        }
    }
}
