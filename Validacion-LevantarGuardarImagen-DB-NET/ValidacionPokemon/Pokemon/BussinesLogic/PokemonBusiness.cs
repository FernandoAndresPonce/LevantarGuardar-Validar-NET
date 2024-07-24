using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLogic;
using Dominio;

namespace BusinessLogic
{
    public class PokemonBusiness
    {

        //Consulta a base de datos, y leerlo, traerlos.
        public List<Pokemon> Listar()
        {
            List<Pokemon> listaPokemones = new List<Pokemon>();
            DataAccess datos = new DataAccess();

            try
            {
                datos.setearConsulta("select P.Numero, P.Nombre, P.Descripcion, P.UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id  from POKEMONS P, ELEMENTOS E, ELEMENTOS D where P.IdTipo = E.Id And P.IdDebilidad = D.Id And Activo = 1");
                datos.ejecutarLectura();

                while(datos.Lector.Read())
                {
                    Pokemon auxPokemon = new Pokemon();
                    auxPokemon.Id = (int)datos.Lector["Id"];
                    auxPokemon.Numero = (int)datos.Lector["Numero"];
                    auxPokemon.Nombre = (string)datos.Lector["Nombre"];
                    auxPokemon.Descripcion = (string)datos.Lector["Descripcion"];
                    if(!(datos.Lector["UrlImagen"] is DBNull))    
                        auxPokemon.UrlImagen = (string)datos.Lector["UrlImagen"];
                    auxPokemon.Tipo = new Elemento();
                    auxPokemon.Tipo.Id = (int)datos.Lector["IdTipo"];
                    auxPokemon.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    auxPokemon.Debilidad = new Elemento();
                    auxPokemon.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                    auxPokemon.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    listaPokemones.Add(auxPokemon);
                }

                return listaPokemones;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            finally
            {
                datos.cerrarConexion();
            }
        }

        public void agregar(Pokemon newPokemon)
        {

            DataAccess data = new DataAccess();
            
            try
            {
                data.setearConsulta("insert into POKEMONS (Numero,Nombre, Descripcion, UrlImagen, idTipo, idDebilidad,Activo) values (@Numero ,@Nombre, @Descripcion, @UrlImagen, @Tipo, @Debilidad, 1)");
                data.setearParametros("@Numero", newPokemon.Numero);
                data.setearParametros("@Nombre", newPokemon.Nombre);
                data.setearParametros("@Descripcion", newPokemon.Descripcion);
                data.setearParametros("@UrlImagen", newPokemon.UrlImagen);
                data.setearParametros("@Tipo", newPokemon.Tipo.Id);
                data.setearParametros("@Debilidad", newPokemon.Debilidad.Id);

                data.ejecutarAccion();
            }
            catch (Exception ex)
            {

                
            }
            finally
            {
                data.cerrarConexion();
            }
        }


        public void modificar(Pokemon modificarPokemon)
        {
            DataAccess datos = new DataAccess();

            try
            {
                datos.setearConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @descripcion, UrlImagen = @urlimagen, IdTipo = @idtipo , IdDebilidad = @iddebilidad where Id = @id");
                datos.setearParametros("@numero",modificarPokemon.Numero );
                datos.setearParametros("@nombre", modificarPokemon.Nombre );
                datos.setearParametros("@descripcion", modificarPokemon.Descripcion);
                datos.setearParametros("@urlimagen", modificarPokemon.UrlImagen );
                datos.setearParametros("@idtipo", modificarPokemon.Tipo.Id );
                datos.setearParametros("@iddebilidad", modificarPokemon.Debilidad.Id);
                datos.setearParametros("@id", modificarPokemon.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

          
        }

        public void eliminarFisica(Pokemon eliminarFisicaPokemon)
        {
            DataAccess datos = new DataAccess();
            try
            {
                datos.setearConsulta("delete from POKEMONS where Id = @id");
                datos.setearParametros("@id", eliminarFisicaPokemon.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void eliminarLogica(Pokemon eliminarLogica)
        {

            DataAccess datos = new DataAccess();
            try
            {
                datos.setearConsulta("update POKEMONS set Activo = 0 where Id = @id");
                datos.setearParametros("@id", eliminarLogica.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            {
                datos.cerrarConexion();
            }
        }

        // paso 4 Filtrar avanzado
        public List<Pokemon> filtrar(string campo, string criterio, string filtro)
        {
            DataAccess datos = new DataAccess();
            List<Pokemon> listaPokemon = new List<Pokemon>();
            try
            {
                string consulta = "select P.Numero, P.Nombre, P.Descripcion, P.UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id  from POKEMONS P, ELEMENTOS E, ELEMENTOS D where P.IdTipo = E.Id And P.IdDebilidad = D.Id And Activo = 1 And";
                switch (campo)
                {
                    case "Numero":

                        switch(criterio)
                        {
                            case "Mayor a":
                                consulta += " Numero > " + filtro;
                                break;
                            case "Menor a":
                                consulta += " Numero < " + filtro;
                                break;
                            case "Igual a":
                                consulta += " Numero == " + filtro;
                                break;
                        }
                        break;
                    case "Nombre":
                        switch(criterio)
                        {
                            case "Comienza con":
                                consulta += " P.Nombre like " +"'" + filtro + "%'";
                                break;
                            case "Termina con":
                                consulta += " P.Nombre like " + "'%" + filtro + "'";
                                break;
                            case "Contiene":
                                consulta += " P.Nombre like " + "'%" + filtro + "%'";
                                break;
                        }


              
                        break;

                    case "Descripcion":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += " P.Descripcion like " + "'" + filtro + "%'";
                                break;
                            case "Termina con":
                                consulta += " P.Descripcion like " + "'%" + filtro + "'";
                                break;
                            default:
                                consulta +=  "P.Descripcion like " + "'%" + filtro + "%'";
                                break;
                        }
                        break;
                    default:
                        break;
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Pokemon auxPokemon = new Pokemon();
                    auxPokemon.Id = (int)datos.Lector["Id"];
                    auxPokemon.Numero = (int)datos.Lector["Numero"];
                    auxPokemon.Nombre = (string)datos.Lector["Nombre"];
                    auxPokemon.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["UrlImagen"] is DBNull))
                        auxPokemon.UrlImagen = (string)datos.Lector["UrlImagen"];
                    auxPokemon.Tipo = new Elemento();
                    auxPokemon.Tipo.Id = (int)datos.Lector["IdTipo"];
                    auxPokemon.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    auxPokemon.Debilidad = new Elemento();
                    auxPokemon.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                    auxPokemon.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    listaPokemon.Add(auxPokemon);

                }

                return listaPokemon;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            {
                datos.cerrarConexion();            
            }
        }
    }

       
}
                



                    
