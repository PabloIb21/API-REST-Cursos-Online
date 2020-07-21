using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Persistencia.DapperConexion
{
    public class FactoryConnection : IFactoryConnection
    {
        private IDbConnection _connection;
        private readonly IOptions<ConexionConfiguracion> _configs;
        public FactoryConnection(IOptions<ConexionConfiguracion> configs ){
            _configs = configs;
        }
        public void CloseConnection()
        {
            if(_connection != null && _connection.State == ConnectionState.Open){
                _connection.Close();
            } 
        }

        public IDbConnection GetConnection()
        {
            if(_connection == null){
                _connection = new SqlConnection(_configs.Value.DefaultConnection);
            }

            if(_connection.State != ConnectionState.Open){
                _connection.Open();
            }

            return _connection;
        }
    }
}