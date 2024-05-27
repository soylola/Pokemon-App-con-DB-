using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace winform_app
{
    public partial class frmPokemons : Form
    {
        private List<Pokemon> listaPokemon;
        public frmPokemons()
        {
            InitializeComponent();
            this.Size = new Size(1946,1106);
        }

        private void frmPokemons_Load(object sender, EventArgs e)
        {
            cargar(); 
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.UrlImagen);
        }

        private void cargarImagen (string imagen)
        {
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch(Exception)
            {
                pbxPokemon.Load("https://cdn.pixabay.com/photo/2017/01/25/17/35/picture-2008484_1280.png");
            }

        }
        private void cargar()
        {
            //INVOCACIÓN DE LA LECTURA
            PokemonNegocio negocio = new PokemonNegocio();

            try
            {
                listaPokemon = negocio.listar();

                //A LA GRILLA DE DATOS LE ASIGNO NEGOCIO.LISTAR()
                dgvPokemons.DataSource = listaPokemon;

                //OCULTO LA COLUMNA URL IMAGEN y ID
                dgvPokemons.Columns["UrlImagen"].Visible = false;
                dgvPokemons.Columns["Id"].Visible = false;


                cargarImagen(listaPokemon[0].UrlImagen);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaPokemon alta = new frmAltaPokemon();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;  //tomo el pokemon seleccionado
            frmAltaPokemon modificar = new frmAltaPokemon(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminarF_Click(object sender, EventArgs e)
        {
            eliminar();
          
        }

        private void btnEliminarL_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }



        private void eliminar(bool logico=false)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            Pokemon seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Querés elimnar lógicamente este Pokemon?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                    if(logico)
                    {
                        negocio.eliminarLogico(seleccionado.Id);
                    }
                    else
                    { 
                        negocio.eliminar(seleccionado.Id);
                    }
                   
                    cargar();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
