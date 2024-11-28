using System.Globalization;
using FluentValidation;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Reporte;
public class ExportPDF
{
    public class Consulta : IRequest<Stream>
    {
        public Consulta(int transportistaID, DateTime fechaInicio, DateTime fechaFinal)
        {
            TransportistaID = transportistaID;
            FechaInicio = fechaInicio;
            FechaFinal = fechaFinal;
        }

        public int TransportistaID { get; }
        public DateTime FechaInicio { get; }
        public DateTime FechaFinal { get; }

    }

    public class ConsultaValidator : AbstractValidator<Consulta>
    {
        public ConsultaValidator()
        {
            RuleFor(x => x.TransportistaID).GreaterThan(0).WithMessage("El Campo Transportista debe ser mayor a 0 o campo no debe de estar en blanco.");
            RuleFor(x => x.FechaInicio)
                    .NotEmpty().WithMessage("La Fecha de Inicio no puede estar vacio.")
                    .LessThanOrEqualTo(DateTime.Now).WithMessage("La Fecha de Inicio no puede ser mayor que la fecha actual.");
            RuleFor(x => x.FechaFinal)
                .NotEmpty().WithMessage("La Fecha Final no puede estar vacía.")
                .GreaterThan(x => x.FechaInicio).WithMessage("La Fecha Final debe ser posterior a la Fecha de Inicio.");
                // .LessThanOrEqualTo(DateTime.Now).WithMessage("La Fecha Final no puede ser mayor que la fecha actual.");
        }
    }


    public class Manejador : IRequestHandler<Consulta, Stream>
    {

        private readonly BackendContext _context;

        public Manejador(BackendContext context)
        {
            _context = context;
        }
        public async Task<Stream> Handle(Consulta request, CancellationToken cancellationToken)
        {
            Font fuenteTitulo = new Font(Font.HELVETICA, 10f, Font.BOLD, BaseColor.Blue);
            Font fuenteHeader = new Font(Font.HELVETICA, 7f, Font.BOLD, BaseColor.Black);
            Font fuenteData = new Font(Font.HELVETICA, 7f, Font.NORMAL, BaseColor.Black);

            BaseColor colorPar = new BaseColor(240, 240, 240);
            BaseColor colorImpar = BaseColor.White;

            var viajes = await _context.Viajes
                            .Where(v => v.TransportistaID == request.TransportistaID)
                            .Where(v => v.Fecha.Date >= request.FechaInicio.Date && v.Fecha.Date <= request.FechaFinal.Date)
                            .Include(v => v.Sucursal)
                            .Include(v => v.Transportista)
                            .Include(v => v.ViajesDetalles)
                                .ThenInclude(vd => vd.Colaborador)
                                .ThenInclude(c => c.SucursalesColaboradores)
                        .ToListAsync();

            var dataConsulta = viajes
                                .OrderBy(v => v.Fecha)
                                    .ThenBy(v => v.SucursalID)
                                    .ThenBy(v => v.ViajesDetalles.First().Colaborador.Nombre)
                                .SelectMany(v => v.ViajesDetalles.Select(vd => new
                                {
                                    Fecha = v.Fecha.Date,
                                    Sucursal = v.Sucursal.Descripcion,
                                    Transportista = v.Transportista.Descripcion,
                                    Tarifa = v.Transportista.Tarifa,
                                    Colaborador = vd.Colaborador.Nombre,
                                    Distancia = vd.Colaborador.SucursalesColaboradores.Single(
                                                    sc => sc.SucursalID == v.SucursalID
                                                ).Distancia,
                                    Total = vd.Colaborador.SucursalesColaboradores
                                                .Single(sc => sc.SucursalID == v.SucursalID).Distancia * v.Transportista.Tarifa
                                })).ToList();

            var dataConsulta2 = viajes
                                .GroupBy(v => v.Transportista)
                                .Select(a => new
                                {
                                    Transportista = a.Key.Descripcion,
                                    Total = a.Sum(b => b.ViajesDetalles
                                                .Sum(
                                                    vd => vd.Colaborador.SucursalesColaboradores.Where(sc => sc.SucursalID == b.SucursalID)
                                                    .Sum(sc => sc.Distancia * a.Key.Tarifa)))
                                }).ToList();

            MemoryStream workStream = new MemoryStream();
            Rectangle rect = new Rectangle(PageSize.A4);

            Document document = new Document(rect, 0, 0, 50, 100);
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;

            document.Open();
            document.AddTitle("Reporte de Transportistas");

            PdfPTable tabla = new PdfPTable(1);
            tabla.WidthPercentage = 90;
            PdfPCell celda = new PdfPCell(new Phrase("Reporte de Transportistas", fuenteTitulo));
            celda.Border = Rectangle.NO_BORDER;
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla.AddCell(celda);
            document.Add(tabla);


            PdfPTable tablaEspacio = new PdfPTable(1);
            tabla.WidthPercentage = 90;
            PdfPCell celdaEspacio = new PdfPCell(new Phrase(" "));
            celdaEspacio.Border = Rectangle.NO_BORDER;
            tablaEspacio.AddCell(celdaEspacio);
            document.Add(tablaEspacio);


            PdfPTable tablaReporte = new PdfPTable(7);
            float[] widths = new float[] { 8f, 15f, 20f, 5f, 20f, 8f, 10f };
            tablaReporte.SetWidthPercentage(widths, rect);

            PdfPCell celdaHeaderFecha = new PdfPCell(new Phrase("Fecha", fuenteHeader));
            celdaHeaderFecha.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaHeaderFecha.Border = Rectangle.NO_BORDER;
            tablaReporte.AddCell(celdaHeaderFecha);

            PdfPCell celdaHeaderSucursal = new PdfPCell(new Phrase("Sucursal", fuenteHeader));
            celdaHeaderSucursal.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaHeaderSucursal.Border = Rectangle.NO_BORDER;
            tablaReporte.AddCell(celdaHeaderSucursal);

            PdfPCell celdaHeaderTransportista = new PdfPCell(new Phrase("Transportista", fuenteHeader));
            celdaHeaderTransportista.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaHeaderTransportista.Border = Rectangle.NO_BORDER;
            tablaReporte.AddCell(celdaHeaderTransportista);

            PdfPCell celdaHeaderTarifa = new PdfPCell(new Phrase("Tarifa", fuenteHeader));
            celdaHeaderTarifa.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaHeaderTarifa.Border = Rectangle.NO_BORDER;
            tablaReporte.AddCell(celdaHeaderTarifa);

            PdfPCell celdaHeaderColaborador = new PdfPCell(new Phrase("Colaborador", fuenteHeader));
            celdaHeaderColaborador.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaHeaderColaborador.Border = Rectangle.NO_BORDER;
            tablaReporte.AddCell(celdaHeaderColaborador);

            PdfPCell celdaHeaderDistancia = new PdfPCell(new Phrase("Distancia", fuenteHeader));
            celdaHeaderDistancia.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaHeaderDistancia.Border = Rectangle.NO_BORDER;
            tablaReporte.AddCell(celdaHeaderDistancia);

            PdfPCell celdaHeaderTotal = new PdfPCell(new Phrase("Total", fuenteHeader));
            celdaHeaderTotal.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaHeaderTotal.Border = Rectangle.NO_BORDER;
            tablaReporte.AddCell(celdaHeaderTotal);

            tablaReporte.WidthPercentage = 90;
            int i = 0;
            foreach (var reporteElemento in dataConsulta)
            {
                BaseColor colorFondo = (i % 2 == 0) ? colorPar : colorImpar;
                
                PdfPCell celdaDataFecha = new PdfPCell(new Phrase(reporteElemento.Fecha.ToString("dd/MM/yyyy"), fuenteData));
                celdaDataFecha.HorizontalAlignment = Element.ALIGN_CENTER;
                celdaDataFecha.Border = Rectangle.NO_BORDER;
                celdaDataFecha.BackgroundColor = colorFondo;
                tablaReporte.AddCell(celdaDataFecha);

                PdfPCell celdaDataSucursal = new PdfPCell(new Phrase(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reporteElemento.Sucursal.ToLower()), fuenteData));
                celdaDataSucursal.HorizontalAlignment = Element.ALIGN_CENTER;
                celdaDataSucursal.Border = Rectangle.NO_BORDER;
                celdaDataSucursal.BackgroundColor = colorFondo;
                tablaReporte.AddCell(celdaDataSucursal);

                PdfPCell celdaDataTransportista = new PdfPCell(new Phrase(
                    string.IsNullOrEmpty(reporteElemento.Transportista) ? "Desconocido" :
                    CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reporteElemento.Transportista.ToLower())
                    , fuenteData));
                celdaDataTransportista.HorizontalAlignment = Element.ALIGN_CENTER;
                celdaDataTransportista.Border = Rectangle.NO_BORDER;
                celdaDataTransportista.BackgroundColor = colorFondo;
                tablaReporte.AddCell(celdaDataTransportista);

                PdfPCell celdaDataTarifa = new PdfPCell(new Phrase(reporteElemento.Tarifa.ToString("0.00", CultureInfo.InvariantCulture), fuenteData));
                celdaDataTarifa.HorizontalAlignment = Element.ALIGN_CENTER;
                celdaDataTarifa.Border = Rectangle.NO_BORDER;
                celdaDataTarifa.BackgroundColor = colorFondo;
                tablaReporte.AddCell(celdaDataTarifa);

                PdfPCell celdaDataColaborador = new PdfPCell(new Phrase(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reporteElemento.Colaborador.ToLower()), fuenteData));
                celdaDataColaborador.HorizontalAlignment = Element.ALIGN_CENTER;
                celdaDataColaborador.Border = Rectangle.NO_BORDER;
                celdaDataColaborador.BackgroundColor = colorFondo;
                tablaReporte.AddCell(celdaDataColaborador);

                PdfPCell celdaDataDistancia = new PdfPCell(new Phrase(reporteElemento.Distancia.ToString(), fuenteData));
                celdaDataDistancia.HorizontalAlignment = Element.ALIGN_CENTER;
                celdaDataDistancia.Border = Rectangle.NO_BORDER;
                celdaDataDistancia.BackgroundColor = colorFondo;
                tablaReporte.AddCell(celdaDataDistancia);

                PdfPCell celdaDataTotal = new PdfPCell(new Phrase(reporteElemento.Total.ToString("0.00", CultureInfo.InvariantCulture), fuenteData));
                celdaDataTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
                celdaDataTotal.Border = Rectangle.NO_BORDER;
                celdaDataTotal.BackgroundColor = colorFondo;
                tablaReporte.AddCell(celdaDataTotal);
                i++;
            }

            document.Add(tablaReporte);
            document.Add(tablaEspacio);
            document.Add(tablaEspacio);

            PdfPTable tablaReporteTotal = new PdfPTable(2);
            float[] widths1 = new float[] { 1f, 20f };
            tablaReporteTotal.SetWidthPercentage(widths1, rect);
            tablaReporteTotal.WidthPercentage = 90;


            foreach (var reporteElemento2 in dataConsulta2)
            {
                PdfPCell celdaDataLeyenda = new PdfPCell(new Phrase("Total: ", fuenteHeader));
                celdaDataLeyenda.HorizontalAlignment = Element.ALIGN_LEFT;
                celdaDataLeyenda.Border = Rectangle.NO_BORDER;
                tablaReporteTotal.AddCell(celdaDataLeyenda);

                PdfPCell celdaDataLeyendaTotal = new PdfPCell(new Phrase(reporteElemento2.Total.ToString("0.00", CultureInfo.InvariantCulture), fuenteData));
                celdaDataLeyendaTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                celdaDataLeyendaTotal.Border = Rectangle.NO_BORDER;
                tablaReporteTotal.AddCell(celdaDataLeyendaTotal);
            }
            document.Add(tablaReporteTotal);
            document.Close();

            byte[] byteData = workStream.ToArray();
            workStream.Write(byteData, 0, byteData.Length);
            workStream.Position = 0;

            return workStream;
        }
    }




}
