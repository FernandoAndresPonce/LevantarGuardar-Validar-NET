using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using BusinessLogic;
using BussinesLogic;
using System.IO;
using System.Configuration;

namespace UserInferfaz
{

    public partial class frmAltaPokemon : Form
    {
        private Pokemon pokemon = null;
        //Lenvantar imagen paso 3,
        private OpenFileDialog archivo = null;

        public frmAltaPokemon()
        {
            InitializeComponent();
        }

        public frmAltaPokemon(Pokemon modificarPokemon)
        {
            InitializeComponent();

            this.pokemon = modificarPokemon;
            lblAlta.Text = "Modificar Pokemon";
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            ElementoBusi elementoLista = new ElementoBusi();


            
            cmbTipo.SelectedIndex = -1;
            cmbTipo.DataSource = elementoLista.ListarE();
            cmbTipo.ValueMember = "Id";
            cmbTipo.DisplayMember = "Descripcion";


            
            cmbDebilidad.DataSource = elementoLista.ListarE();
            cmbDebilidad.SelectedIndex = -1;
            cmbDebilidad.ValueMember = "Id";
            cmbDebilidad.DisplayMember = "descripcion";

            if (pokemon != null)
            {
                txtNumero.Text = pokemon.Numero.ToString();
                txtNombre.Text = pokemon.Nombre;

                txtDescripcion.Text = pokemon.Descripcion;

                cargarImagenAP(pokemon.UrlImagen);
                txtUrlImagen.Text = pokemon.UrlImagen;

                cmbTipo.SelectedValue = pokemon.Tipo.Id;
                cmbDebilidad.SelectedValue = pokemon.Debilidad.Id;
            }
        }
            



        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            PokemonBusiness negocio = new PokemonBusiness();
            

            try
            {
                if (pokemon == null)
                    pokemon = new Pokemon();

                pokemon.Numero = int.Parse(txtNumero.Text);
                pokemon.Nombre = txtNombre.Text;
                pokemon.Descripcion = txtDescripcion.Text;
                pokemon.UrlImagen = txtUrlImagen.Text;


                pokemon.Tipo = (Elemento)cmbTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cmbDebilidad.SelectedItem;


                if (pokemon.Id != 0)
                {
                    negocio.modificar(pokemon);
                    MessageBox.Show("Modificado Exitosament");
                }
                else
                {
                    negocio.agregar(pokemon);
                    MessageBox.Show("Agregado Exitosamente");
                }
                //GUARDAR LEVANTAR ,Lenvantar imagen paso 4,
                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                
                    if (!(File.Exists(archivo.FileName))) 
                        File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
                       // EXISTE TAMBIEN OPEN.EXIST, SI EXISTE EL ARCHIVO, OPEN.DELETE, PARA ELIMINAR, EL CASO QUE EXISTA
                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
               
            }
            finally
            {
                Close();
            }
           

            
            
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagenAP(txtUrlImagen.Text);
        }

        public void cargarImagenAP( string imagen)
        {
            try
            {
                pictureBox1.Load(imagen);
            }
            catch (Exception ex)
            {
                pictureBox1.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSkq9bHJ3gt0lMcFAlhsCbumDb0fYgvpP0HNQ&s");


            }

        }

        private void txtNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            //Lenvantar imagen paso 1
            //openfiledialog, abre una ventana dialog, y me va a permitir elegir el arhivo.
            archivo = new OpenFileDialog();

            archivo.Filter = "jpg|*.jpg;|png|*.png"; // tipo de archivo que nos permite que traigan.
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName; // guarda la ruta completa del archivo que estoy seleccionando.
                cargarImagenAP(archivo.FileName);

                // Gaurdar imagen ,Lenvantar imagen paso 2, y nos vamos App.config >>> barra de herramientas de diagnostico

                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);

                // en aquel lado hacemos esto
                //             <? xml version = "1.0" encoding = "utf-8" ?>
                //< configuration >
                //    < startup >
                //        < supportedRuntime version = "v4.0" sku = ".NETFramework,Version=v4.7.2" />
                //       </ startup >
                //       < appSettings >
                //           < add key = "images-folder" value = "C:\Poke-App\"/>
                //         </ appSettings >
                //     </ configuration >>

                // luego agregamos en REFERENCIAS >>> ASSEMBLIES>> Y AGREGAR SYSTEM.CONFIGURATION, y agregamos a
                // using... system.configuration >>> y volvemos a file.copy completando...
   

      
      
            }
      
        

   
        }
    }
}
