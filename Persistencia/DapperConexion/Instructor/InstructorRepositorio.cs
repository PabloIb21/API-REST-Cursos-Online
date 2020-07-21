using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepositorio : IInstructor
    {
        private readonly IFactoryConnection _factoryConnection;
        public InstructorRepositorio(IFactoryConnection factoryConnection){
            _factoryConnection = factoryConnection;
        }

        public async Task<int> Actualizar(Guid instructorId, string nombre, string apellidos, string grado)
        {
            var storeProcedure = "usp_Instructor_Editar";
            try{
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(
                    storeProcedure,
                    new {
                        InstructorId = instructorId,
                        Nombre = nombre,
                        Apellidos = apellidos,
                        Grado = grado
                    },
                    commandType: CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();
                return resultado;
            }catch(Exception e){
                throw new Exception("No se pudo editar el instructor", e);
            }
        }

        public async Task<int> Eliminar(Guid id)
        {
            var storeProcedure = "usp_Instructor_Eliminar";
            try{
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(
                    storeProcedure,
                    new {
                        InstructorId = id
                    },
                    commandType: CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();
                return resultado;

            }catch(Exception e){
                throw new Exception("No se pudo eliminar el instructor", e);
            }
        }

        public async Task<int> Nuevo(string nombre, string apellidos, string grado)
        {
            var storeProcedure = "usp_Instructor_Nuevo";
            try{
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(storeProcedure,
                new {
                    InstructorId = Guid.NewGuid(),
                    Nombre = nombre,
                    Apellidos = apellidos,
                    Grado = grado
                },
                commandType: CommandType.StoredProcedure);
                _factoryConnection.CloseConnection();

                return resultado;
            }catch(Exception e){
                throw new Exception("No se pudo guardar el nuevo instructor", e);
            }
        }

        public async Task<IEnumerable<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> instructorList = null;
            var storeProcedure = "usp_Obtener_Instructores";
            try{
                var connection = _factoryConnection.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storeProcedure, null, commandType : CommandType.StoredProcedure);
            }catch(Exception e){    
                throw new Exception("Error en la consulta de datos", e);
            }finally{
                _factoryConnection.CloseConnection();
            }

            return instructorList;
        }

        public async Task<InstructorModel> ObtenerPorId(Guid id)
        {
            var storeProcedure = "usp_Obtener_Instructor_Por_Id";
            InstructorModel instructor = null;
            try{
                var connection = _factoryConnection.GetConnection();
                instructor = await connection.QueryFirstAsync<InstructorModel>(
                    storeProcedure,
                    new{
                        InstructorId = id
                    },
                    commandType: CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();
                return instructor;
            }catch(Exception e){
                throw new Exception("No se pudo encontrar el instructor", e);
            }
        }
    }
}