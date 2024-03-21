using ExamenLibrosMvcCore.Data;
using Microsoft.EntityFrameworkCore;
using ExamenLibrosMvcCore.Models;

namespace ExamenLibrosMvcCore.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;

        public RepositoryLibros(LibrosContext context)
        {
            this.context = context;
        }

        public async Task<List<Libro>> GetAllLibrosAsync()
        {
            return await this.context.Libros.ToListAsync();
        }

        public async Task<Libro> FindLibroAsync(int idLibro)
        {
            return await this.context.Libros.FirstOrDefaultAsync(x => x.IdLibro == idLibro);
        }

        public async Task<List<Libro>> GetLibrosGeneroAsync(int idgenero)
        {
            return await this.context.Libros.Where(p => p.IdGenero == idgenero).ToListAsync();
        }

        public async Task<List<Genero>> GetAllGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }

        public async Task<Genero> FindGeneroAsync(int idgenero)
        {
            return await this.context.Generos.FirstOrDefaultAsync(x => x.IdGenero == idgenero);
        }


        public List<Libro> GetLibrosCarrito(List<int> idLibros)
        {
            return context.Libros.Where(p => idLibros.Contains(p.IdLibro)).ToList();
        }

        public async Task<int> GetUltimoIdPedido()
        {
            var ultimoId = await this.context.Pedidos
                                            .MaxAsync(p => (int?)p.IdPedido);

            return ultimoId ?? 1;
        }

        public async Task<int> GetUltimaFactura()
        {
            var ultimaFactura = await this.context.Pedidos
                                            .MaxAsync(f => (int?)f.IdFactura);

            return ultimaFactura ?? 1;
        }

        public async Task ComprarProducto(Pedido pedido)
        {
            context.Pedidos.Add(pedido);
            await context.SaveChangesAsync();
        }

        public async Task<List<VistaPedidos>> GetPedidosUsuario(int iduser)
        {

            return context.VistasPedidos.Where(c => c.IdUsuario == iduser).ToList();
        }
    }
}
