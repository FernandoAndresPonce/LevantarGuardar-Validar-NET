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

namespace UserInferfaz
{
    public partial class Form1 : Form
    {
        private List<Pokemon> listaPokemones;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // paso 1 Filtro Avanzando DB
           

            cargar();
            cmbCampo.Items.Add("Numero");
            cmbCampo.Items.Add("Nombre");
            cmbCampo.Items.Add("Descripcion");
        }

        private void cargar()
        {
            PokemonBusiness pokeBusiness = new PokemonBusiness();
            listaPokemones = pokeBusiness.Listar();

            dataGridView1.DataSource = listaPokemones;
            ocultarColumnas();
            pictureBox1.Load(listaPokemones[0].UrlImagen);

        }
        



        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                Pokemon seleccion = (Pokemon)dataGridView1.CurrentRow.DataBoundItem;
                cargarImagen(seleccion.UrlImagen);
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pictureBox1.Load(imagen);
            }
            catch (Exception ex)
            {
                pictureBox1.Load("https://static.vecteezy.com/system/resources/previews/004/141/669/non_2x/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg");
            }

        }
        // AGRGAR A BASE DE DATO
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaPokemon formularioAltaPokemon = new frmAltaPokemon();
            formularioAltaPokemon.ShowDialog();
            cargar();



        }
        // MODIFICACION 
        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon seleccionModificar = (Pokemon)dataGridView1.CurrentRow.DataBoundItem;

            frmAltaPokemon modificarPokemon = new frmAltaPokemon(seleccionModificar);
            modificarPokemon.ShowDialog();
            cargar();
        }

        //ELIMINACION FISICA
        private void button1_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        // ELIMINACION LOGICA
        private void btnEliminacionLogica_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }

        private void eliminar(bool logica = false)
        {
            PokemonBusiness negocio = new PokemonBusiness();
            Pokemon eliminacion = (Pokemon)dataGridView1.CurrentRow.DataBoundItem;

            try
            {
                DialogResult resultado = MessageBox.Show("Esta seguro  de Eliminar el Registro?", "Atencion...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    if (logica)
                    {
                        negocio.eliminarLogica(eliminacion);
                        cargar();
                    }
                    else
                    {
                        negocio.eliminarFisica(eliminacion);
                        cargar();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void ocultarColumnas()
        {

            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Columns["UrlImagen"].Visible = false;

        }

        // FILTRO RAPIDO
        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            // paso 1 Busqueda
            // aplicamos un filtro sobre la lista creada.
            List<Pokemon> listaFiltrada;
            string filtro = txtFiltro.Text;


            // paso 3 Busqueda
            if (filtro != string.Empty) // or (filtro.Lenght > 3)
            {

                // esto esa como foreach, que itera la coleccion, buscando cada
                // vuelta el nombre en este caso,
                // se llama lambda expresion.
                //El CONTAINS, ME  analiza un cadena con otra cadena si tiene datos relacionado, me la trae.
                listaFiltrada = listaPokemones.FindAll(x => x.Nombre.ToLower().Contains(filtro.ToLower()) || x.Tipo.Descripcion.ToLower().Contains(filtro.ToLower()));
            }
            else
            {
                listaFiltrada = listaPokemones;
            }

            //paso 2 Busqueda
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaFiltrada;
            ocultarColumnas();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // paso 2 Filtro Avanzando DB
            string opcion = cmbCampo.SelectedItem.ToString();// me guarda el elemento seleccionado

            try
            {   
                
                switch (opcion)
                {
                    case "Numero":
                        cmbCriterio.Items.Clear();
                        cmbCriterio.Items.Add("Mayor a");
                        cmbCriterio.Items.Add("Menor a");
                        cmbCriterio.Items.Add("Igual a");

                        break;

                    case "Nombre":
                        cmbCriterio.Items.Clear();
                        cmbCriterio.Items.Add("Comienza con");
                        cmbCriterio.Items.Add("Termina con");
                        cmbCriterio.Items.Add("Contiene");

                        break;

                    case "Descripcion":
                        cmbCriterio.Items.Clear();
                        cmbCriterio.Items.Add("Comienza con");
                        cmbCriterio.Items.Add("Termina con");
                        cmbCriterio.Items.Add("Contiene");

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
        }
        // Requeridos y Sólo Nros - MessageBox PASO 3.
        private bool soloNumeros(string cadenaValidar)
        {
            foreach (char caracter in cadenaValidar)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }

        // Requeridos y Sólo Nros - MessageBox PASO 1.
        private bool validarFiltro()
        {
            if (cmbCampo.SelectedIndex < 0)// Con esto decimos que tiene algo, que no esta VACIO, si es mayor a cero.
            {
                MessageBox.Show("Por favor seleccione el campo para filtrar.");
                return true;
            }
            else if (cmbCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el Criterio para filtrar");
                return true;
            }
            else if (cmbCampo.SelectedItem.ToString() == "Numero")
            {
                // Requeridos y Sólo Nros - MessageBox PASO 4.
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Deber cargar el filtro... Esta vacio");
                    return true;
                }
                if(!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Solo numeros para filtrar");
                    return true;
                }

            }
            return false;
        }
        private void btnFiltro_Click(object sender, EventArgs e)
        {
            PokemonBusiness negocio = new PokemonBusiness();

            // Requeridos y Sólo Nros - MessageBox PASO 2.
            if (validarFiltro())
                return;

            string campo = cmbCampo.SelectedItem.ToString();
            string criterio = cmbCriterio.SelectedItem.ToString();
            string filtro = txtFiltroAvanzado.Text;

            dataGridView1.DataSource = negocio.filtrar(campo, criterio, filtro);
        
            


        }
    }
}
