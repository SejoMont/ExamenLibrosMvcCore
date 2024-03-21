using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;

namespace ExamenLibrosMvcCore.Helpers
{
    public enum Folders { Images = 0 }
    public class HelperPathProvider
    {
        private readonly IServer server;
        private readonly IWebHostEnvironment hostEnvironment;

        // Constructor que recibe el entorno de host web y el servidor
        public HelperPathProvider(IWebHostEnvironment hostEnvironment, IServer server)
        {
            this.server = server;
            this.hostEnvironment = hostEnvironment;
        }

        public string GetFolderPath(Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Images)
            {
                carpeta = "images";
            }

            return carpeta;
        }

        // Método para mapear la ruta completa del archivo en el sistema de archivos del servidor
        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = this.GetFolderPath(folder);
            string rootPath = this.hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath, carpeta, fileName);
            return path;
        }

        public string MapUrlPath(string fileName, Folders folder)
        {
            string carpeta = this.GetFolderPath(folder);
            var addresses = server.Features.Get<IServerAddressesFeature>().Addresses;
            string serverUrl = addresses.FirstOrDefault();
            string urlPath = serverUrl + "/" + carpeta + "/" + fileName;
            return urlPath;
        }

        public string MapUrlServerPath()
        {
            var addresses = server.Features.Get<IServerAddressesFeature>().Addresses;
            string serverUrl = addresses.FirstOrDefault();
            return serverUrl;
        }
    }
}