using System.IO;
using System.Threading;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ExportPDF
    {
        public class Consulta : IRequest<Stream>{}

        public class Manejador : IRequestHandler<Consulta, Stream>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context){
                _context = context;
            }
            public async Task<Stream> Handle(Consulta request, CancellationToken cancellationToken)
            {
                Font fuenteTitulo = new Font(Font.HELVETICA, 8f, Font.BOLD, BaseColor.Blue);
                Font fuenteHeader = new Font(Font.HELVETICA, 7f, Font.BOLD, BaseColor.Black);
                Font fuenteData = new Font(Font.HELVETICA, 7f, Font.NORMAL, BaseColor.Black);

                var cursos = await _context.Curso.ToListAsync();

                MemoryStream workStream = new MemoryStream();
                Rectangle rect = new Rectangle(PageSize.A4);

                Document document = new Document(rect, 0, 0, 50, 100);
                PdfWriter writer = PdfWriter.GetInstance(document, workStream);
                writer.CloseStream = false;

                document.Open();
                document.AddTitle("Lista de Cursos en la Universidad");
                
                PdfPTable tabla = new PdfPTable(1);
                tabla.WidthPercentage = 90;
                PdfPCell celda = new PdfPCell(new Phrase("Lista de Cursos de SQL Server", fuenteTitulo));
                celda.Border = Rectangle.NO_BORDER;
                tabla.AddCell(celda);
                document.Add(tabla);

                PdfPTable tablaCursos = new PdfPTable(2);
                float[] widths = new float[]{40, 60};
                tablaCursos.SetWidthPercentage(widths, rect);

                PdfPCell celdaHeaderTitulo = new PdfPCell(new Phrase("Curso", fuenteHeader));
                tablaCursos.AddCell(celdaHeaderTitulo);

                PdfPCell celdaHeaderDescripcion = new PdfPCell(new Phrase("Descripcion", fuenteHeader));
                tablaCursos.AddCell(celdaHeaderDescripcion);

                tablaCursos.WidthPercentage = 90;

                foreach(var cursoElemento in cursos){
                    PdfPCell celdaDataTitulo = new PdfPCell(new Phrase(cursoElemento.Titulo, fuenteData));
                    tablaCursos.AddCell(celdaDataTitulo);

                    PdfPCell celdaDataDescripcion = new PdfPCell(new Phrase(cursoElemento.Descripcion, fuenteData));
                    tablaCursos.AddCell(celdaDataDescripcion);
                }

                document.Add(tablaCursos);

                document.Close();

                byte[] byteData = workStream.ToArray();
                workStream.Write(byteData, 0, byteData.Length);
                workStream.Position = 0;

                return workStream;
            }
        }
    }
}