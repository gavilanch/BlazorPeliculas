using Azure.Storage.Blobs;

namespace BlazorPeliculas.Server.Helpers
{
    public class AlmacenadorArchivosAzStorage : IAlmacenadorArchivos
    {
        private string connectionString;

        public AlmacenadorArchivosAzStorage(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage")!;
        }

        public async Task EliminarArchivo(string ruta, string nombreContenedor)
        {
            var cliente = new BlobContainerClient(connectionString, nombreContenedor);
            await cliente.CreateIfNotExistsAsync();
            var nombreArchivo = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(nombreArchivo);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, 
            string nombreContenedor)
        {
            var cliente = new BlobContainerClient(connectionString, nombreContenedor);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var archivoNombre = $"{Guid.NewGuid()}{extension}";
            var blob = cliente.GetBlobClient(archivoNombre);
            using (var ms = new MemoryStream(contenido))
            {
                await blob.UploadAsync(ms);
            }

            return blob.Uri.ToString();
        }
    }
}
