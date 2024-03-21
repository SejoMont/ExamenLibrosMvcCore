using ExamenLibrosMvcCore.Data;
using ExamenLibrosMvcCore.Models;

namespace ExamenLibrosMvcCore.Repositories
{
    public class RepositoryUsuarios
    {
        private LibrosContext context;

        public RepositoryUsuarios(LibrosContext context)
        {
            this.context = context;
        }
        public async Task<Usuario> GetUserByEmailPasswordAsync(string email, string password)
        {
            return this.context.Usuarios.Where(x => x.Email == email && x.Pass == password).AsEnumerable().FirstOrDefault();
        }
    }
}