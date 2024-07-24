using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using DataAccessLogic;

namespace BussinesLogic
{
    public class ElementoBusi
    {
        public List<Elemento> ListarE()
        {
            List<Elemento> listaElementos = new List<Elemento>();
            DataAccess datos = new DataAccess();

            try
            {
                datos.setearConsulta("select id, Descripcion from ELEMENTOS");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Elemento auxElemento = new Elemento();

                    auxElemento.Id = (int)datos.Lector["id"];
                    auxElemento.Descripcion = (string)datos.Lector["Descripcion"];

                    listaElementos.Add(auxElemento);


                }

                return listaElementos;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
