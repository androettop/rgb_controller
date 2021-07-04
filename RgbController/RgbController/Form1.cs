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
        String _portName = "";
        ToolStripItem[] originalItemList = { };

        String portName
        {
            get {
                return _portName;
            }
            set
            {
                _portName = value;
                settings.Write("portName", value);
                String initialColor = settings.Read("color");
                if (initialColor.Length > 0)
                {
                    setColor(initialColor);
                }
            }
        }

        public RgbController()
        {
            InitializeComponent();
            portName = settings.Read("portName");
            if( portName.Length <= 0)
            {
                portName = SerialPort.GetPortNames()[0];
            }
            updatePorts();
        }

        private void updatePorts()
        {
            this.contextMenuStrip1.Items.Clear();

            ToolStripMenuItem changeColor = new ToolStripMenuItem();
            ToolStripMenuItem exit = new ToolStripMenuItem();
            ToolStripMenuItem update = new ToolStripMenuItem();

            changeColor.Name = "changeColor";
            changeColor.Text = "Cambiar color";

            update.Name = "update";
            update.Text = "Actualizar";

            exit.Name = "exit";
            exit.Text = "Salir y apagar";

            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] {
            changeColor,update});
            
            String[] ports = SerialPort.GetPortNames();
            ToolStripItemCollection newItems = new ToolStripItemCollection(contextMenuStrip1,originalItemList);
            for (int i = 0; i < ports.Length; i++)
            {
                String portName = ports[i];
                ToolStripItem item = new ToolStripMenuItem();
                item.Text = portName;
                item.Tag = "port";
                this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] {
                item});
            }
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] {
            exit});
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
            }
            else if (e.ClickedItem.Name == "exit")
            {
                Application.Exit();
            } else if (e.ClickedItem.Name == "update") {
                updatePorts();
            }else if(e.ClickedItem.Tag != null && e.ClickedItem.Equals("port")) {
                portName = e.ClickedItem.Text;
            }
        }
    }
}
