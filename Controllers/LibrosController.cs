using ExamenLibrosMvcCore.Models;
using ExamenLibrosMvcCore.Filters;
using ExamenLibrosMvcCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using ExamenLibrosMvcCore.Extensions;
using System;

namespace ExamenLibrosMvcCore.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;
        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }


        public async Task<IActionResult> Index()
        {
            List<Libro> libros = await this.repo.GetAllLibrosAsync();
            return View(libros);
        }


        public async Task<IActionResult> DetallesLibro(int idLibro)
        {
            Libro libro = await this.repo.FindLibroAsync(idLibro);
            return View(libro);
        }


        public async Task<IActionResult> LibrosGenero(int idgenero)
        {
            Genero genero = await this.repo.FindGeneroAsync(idgenero);

            ViewData["GENERO"] = genero;

            List<Libro> libros = await this.repo.GetLibrosGeneroAsync(idgenero);
            return View(libros);
        }

        public IActionResult GuardarLibroCarrito(int idLibro, int? idGenero)
        {
            if (idLibro != null)
            //Guardamos el producto en el carrito
            {
                List<int> carrito;
                if (HttpContext.Session.GetObject<List<int>>("CARRITO") == null)
                {
                    carrito = new List<int>();
                }
                else
                {
                    carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
                }
                carrito.Add(idLibro);
                HttpContext.Session.SetObject("CARRITO", carrito);
            }
            if (idGenero != null)
            {
                return RedirectToAction("LibrosGenero", "Libros", new { idgenero = idGenero });

            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult Carrito(int? idLibroEliminar)
        {
            //Le pasamos el carrito
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");

            //Tienes que crear para añadir datos al carrito
            if (carrito == null)
            {
                return View();
            }
            else
            {
                if (idLibroEliminar != null)
                {
                    carrito.Remove(idLibroEliminar.Value);
                    HttpContext.Session.SetObject("CARRITO", carrito);
                }
                List<Libro> peliculas = this.repo.GetLibrosCarrito(carrito);
                return View(peliculas);
            }
        }

        [AuthorizeUsuarios]
        [HttpPost]
        public async Task<IActionResult> Compra(List<int> idlibros, List<int> cantidades, int iduser)
        {
            int idpedido = await repo.GetUltimoIdPedido();
            int idfactura = await repo.GetUltimaFactura() + 1;

            for (int i = 0; i < idlibros.Count; i++)
            {
                int idlibro = idlibros[i];
                int cantidad = cantidades[i];

                idpedido++;
                Pedido nuevoPedido = new Pedido()
                {
                    IdPedido = idpedido,
                    IdFactura = idfactura,
                    Fecha = DateTime.Now,
                    IdLibro = idlibro,
                    IdUsuario = iduser,
                    Cantidad = cantidad
                };

                await repo.ComprarProducto(nuevoPedido);

            }
            HttpContext.Session.Remove("CARRITO");
            return RedirectToAction("ComprasUsuario", "Libros", new { iduser = iduser });
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> ComprasUsuario(int iduser)
        {
            List<VistaPedidos> pedidos = await this.repo.GetPedidosUsuario(iduser);

            return View(pedidos);
        }

    }
}
