using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelemetryUI_v1._0._1
{
    public partial class mainUI : Form
    {
        public mainUI()
        {
            InitializeComponent();
        }

        private void BPR_Click(object sender, EventArgs e)
        {

        }

        private void mainUI_Load(object sender, EventArgs e)
        {

        }

        private void Engine_Click(object sender, EventArgs e)
        {

        }

        private void mainUI_Load_1(object sender, EventArgs e)
        {

        }

        private void GearLabel_Click(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void wsRL_Click(object sender, EventArgs e)
        {

        }

        private void wsRR_Click(object sender, EventArgs e)
        {

        }

        private void BPR_Click_1(object sender, EventArgs e)
        {

        }

        private void BPF_Click(object sender, EventArgs e)
        {

        }

        private void wsFL_Click(object sender, EventArgs e)
        {

        }

        private void wsFR_Click(object sender, EventArgs e)
        {

        }

        private void genericLabel_Click(object sender, EventArgs e)
        {

        }

        private void LongGC_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void SpeedE_Load(object sender, EventArgs e)
        {

        }

        private void rpmE_Load(object sender, EventArgs e)
        {

        }

        private void O2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Chassis_Click(object sender, EventArgs e)
        {

        }

        private void mainUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            var window = MessageBox.Show(this, "Do you really want to quit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (window == DialogResult.No) e.Cancel = true;
            else e.Cancel = false;
        }
    }
}
