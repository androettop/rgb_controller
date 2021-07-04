using System.Windows.Forms;
using System.IO.Ports;
using System.Drawing;
using System;

namespace RgbController
{
    public partial class RgbController : Form
    {
        SerialPort serialPort = new SerialPort();
        IniFile settings = new IniFile();

        public RgbController()
        {
            InitializeComponent();
            String initialColor = settings.Read("color");
            if(initialColor.Length > 0)
            {
                setColor(initialColor);
            }
        }

        private void setColor(String colorStr, Boolean save = true)
        {
            try
            {
                if (save)
                {
                    settings.Write("color", colorStr);
                }
                serialPort.BaudRate = 9600;
                serialPort.PortName = SerialPort.GetPortNames()[0];
                serialPort.Open();
                serialPort.WriteLine(colorStr);
                serialPort.Close();
            }
            catch
            {
                MessageBox.Show("No se pudo conectar :(");
            }
        }
        private void setColor(Color color,Boolean save = true)
        {
            string colorStr = color.R.ToString().PadLeft(3, '0') +
                        color.G.ToString().PadLeft(3, '0') +
                        color.B.ToString().PadLeft(3, '0');
            setColor(colorStr,save);
        }


        private void RgbController_FormClosing(object sender, FormClosingEventArgs e)
        {
            setColor(Color.Black, false);
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem.Name == "changeColor") {
                ColorDialog dialog = new ColorDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    setColor(dialog.Color);
                }
            } else if (e.ClickedItem.Name == "exit") {
                Application.Exit();
            }
        }
    }
}
