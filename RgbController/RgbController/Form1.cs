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
        String portName = "";
        ToolStripItem[] originalItemList = { };

        public void setPortName(String value) 
        {
            portName = value;
            settings.Write("portName", value);
            String initialColor = settings.Read("color");
            String initialEffect = settings.Read("effect");
            if (initialEffect.Length > 0)
            {
                setEffect(initialEffect);
            }
            if (initialColor.Length > 0)
            {
                setColor(initialColor);
            }
        }

        public RgbController()
        {
            InitializeComponent();
            setPortName(settings.Read("portName"));
            if( portName.Length <= 0)
            {
                setPortName(SerialPort.GetPortNames()[0]);
            }
            updatePorts();
        }

        private void updatePorts()
        {
            contextMenuStrip1.Items.Clear();

            ToolStripMenuItem changeColor = new ToolStripMenuItem();
            ToolStripMenuItem exit = new ToolStripMenuItem();
            ToolStripMenuItem update = new ToolStripMenuItem();
            ToolStripMenuItem fixedFX = new ToolStripMenuItem();
            ToolStripMenuItem breathFX = new ToolStripMenuItem();
            ToolStripMenuItem rainbowFX = new ToolStripMenuItem();

            changeColor.Name = "changeColor";
            changeColor.Text = "Cambiar color";

            update.Name = "update";
            update.Text = "Actualizar";

            fixedFX.Name = "fixedFX";
            fixedFX.Text = "Color fijo";

            breathFX.Name = "breathFX";
            breathFX.Text = "Respiración";

            rainbowFX.Name = "rainbowFX";
            rainbowFX.Text = "Arcoiris";

            exit.Name = "exit";
            exit.Text = "Salir y apagar";

            contextMenuStrip1.Items.AddRange(new ToolStripItem[] {changeColor,update,fixedFX,breathFX,rainbowFX});
            
            String[] ports = SerialPort.GetPortNames();
            ToolStripItemCollection newItems = new ToolStripItemCollection(contextMenuStrip1,originalItemList);
            for (int i = 0; i < ports.Length; i++)
            {
                String portName = ports[i];
                ToolStripItem item = new ToolStripMenuItem();
                item.Text = portName;
                item.Tag = "port";
                contextMenuStrip1.Items.Add(item);
            }
            contextMenuStrip1.Items.Add(exit);
        }

        private void setEffect(String effect, Boolean save = true)
        {
            try
            {
                if (save)
                {
                    settings.Write("effect", effect);
                }
                if (!serialPort.IsOpen)
                {
                    serialPort.BaudRate = 9600;
                    serialPort.PortName = portName;
                    serialPort.Open();
                }
                serialPort.WriteLine(effect);
            }
            catch
            {
                MessageBox.Show("No se pudo conectar :(");
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
                if (!serialPort.IsOpen)
                {
                    serialPort.BaudRate = 9600;
                    serialPort.PortName = portName;
                    serialPort.Open();
                }
                serialPort.WriteLine(colorStr);
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
            setEffect("0", false);
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
            } else if(e.ClickedItem.Tag != null && e.ClickedItem.Tag.Equals("port")) {
                setPortName(e.ClickedItem.Text);
            } else if (e.ClickedItem.Name != null && e.ClickedItem.Name.Equals("fixedFX"))
            {
                setEffect("0");
            } else if (e.ClickedItem.Name != null && e.ClickedItem.Name.Equals("breathFX"))
            {
                setEffect("1");
            } else if (e.ClickedItem.Name != null && e.ClickedItem.Name.Equals("rainbowFX"))
            {
                setEffect("2");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}
