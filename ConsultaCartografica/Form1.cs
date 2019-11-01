using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ConsultaCartografica{

    public partial class Form1 : Form{
        List<int> DistritoF = new List<int>();
        List<int> DistritoL = new List<int>();
        List<string> Municipio = new List<string>();
        List<int> ClaveLocalidad = new List<int>();
        List<string> localidades = new List<string>();
        List<int> tipo = new List<int>();
        List<int> Seccion = new List<int>();

        public Form1(){
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e){
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog()){
                openFileDialog.Filter = "txt files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK){
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();
                    cargador(filePath);
                }
            }
        }

        private void cargador(String filePath){          
            var reader = new StreamReader(File.OpenRead(filePath));
            string tit=reader.ReadLine();
            while (!reader.EndOfStream){
                try
                {
                    var line = reader.ReadLine();
                    string[] words = line.Split('|');
                    DistritoF.Add(Int32.Parse(words[1]));
                    DistritoL.Add(Int32.Parse(words[2]));
                    Municipio.Add(words[3]);
                    ClaveLocalidad.Add(Int32.Parse(words[4]));
                    localidades.Add(words[5]);
                    tipo.Add(words[6] == "U" ? 1 : 2);
                    Seccion.Add(Int32.Parse(words[7]));
                }
                catch (Exception e) {
                    MessageBox.Show("Asegurese de contener los campos: Entidad|Distrito F|Distrito L|Municipio|Clave Localidad|Nombre Localidad|Categoría|Sección \r" + reader.ReadLine(),
                   "Campos",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Exclamation,
                   MessageBoxDefaultButton.Button1);
                }
            }
            richTextBox1.AppendText(localidades.Count + " Localidades cargadas\r");
            DateTime creation = File.GetCreationTime(filePath);
            richTextBox1.AppendText( "Fecha de corte: " + creation);
        }

        private void Inicio(object sender, EventArgs e) {
            try{
                string[] docs = Directory.GetFiles(@"C:\AC-10-R", "*.csv");
                string ruta = docs[0];
                cargador(ruta);
            }
            catch(System.IO.DirectoryNotFoundException f) {
                MessageBox.Show("Coloce el AC-10-R en la carpeta C:\\AC-10-R");
                Directory.CreateDirectory("C:\\AC-10-R");
                System.Diagnostics.Process.Start("C:\\AC-10-R");
                Close();
            }
            catch(System.IndexOutOfRangeException o){
                MessageBox.Show("Coloce el AC-10-R en la carpeta C:\\AC-10-R");
                System.Diagnostics.Process.Start("C:\\AC-10-R");
                Close();
            }
            
        }

        private void Buscar(object sender, EventArgs e){
            string busc = textBox1.Text;
            string[] part = busc.Split(' ');
            switch (part[0]){
                case "sec":
                    int sec = Int32.Parse(part[1]);
                    richTextBox1.Text = "Seccion "+sec+"\n Contiene las localidades \n";
                    for(int i=0; i < Seccion.Count; i++){
                        if (Seccion[i] == sec) {
                            richTextBox1.AppendText(localidades[i] + "\n"
                                + ClaveLocalidad[i]
                                + (tipo[i] == 1 ? "Urbana" : "Rural")
                                + "\n");
                        }
                    }
                    break;

                case "tot":
                    break;

                case "loc":
                    break;

                default:

                    break;
            }
                

        }
    }
}
