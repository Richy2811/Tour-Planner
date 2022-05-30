using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using TourPlanner.Models;
using Document = iText.Layout.Document;

namespace TourPlanner.BL
{
    public class ExportPDF
    {
        private const string _targetFile = "TourSummary.pdf";

        public static int GeneratePdfSingle(TourData tour)
        {
            //check if tour is selected and has valid data
            if (tour == null)
            {
                return 1;
            }
            if (tour.TourName == null || tour.Start == null || tour.Destination == null || tour.Time == null || tour.TourDistance < 0)
            {
                return 2;
            }

            //create pdf document
            PdfWriter writer = new PdfWriter(_targetFile);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph title = new Paragraph(tour.TourName)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(30)
                .SetTextAlignment(TextAlignment.CENTER);
            document.Add(title);

            Paragraph tourDescriptionHeader = new Paragraph("Description:")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(18);
            document.Add(tourDescriptionHeader);

            Paragraph tourDescription = new Paragraph(string.IsNullOrEmpty(tour.TourDescription) ? "/" : tour.TourDescription)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(15);
            document.Add(tourDescription);

            Paragraph tourDetailsHeader = new Paragraph("Details:")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(18);
            document.Add(tourDetailsHeader);

            Table tourTable = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
            tourTable.AddHeaderCell(new Cell().Add(new Paragraph("Start")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            tourTable.AddHeaderCell(new Cell().Add(new Paragraph("Destination")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            tourTable.AddHeaderCell(new Cell().Add(new Paragraph("Transport Type")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            tourTable.AddHeaderCell(new Cell().Add(new Paragraph("Time")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            tourTable.AddHeaderCell(new Cell().Add(new Paragraph("Distance")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));

            tourTable.AddCell(tour.Start);
            tourTable.AddCell(tour.Destination);
            tourTable.AddCell(tour.TransportType);
            tourTable.AddCell(tour.Time);
            tourTable.AddCell($"{tour.TourDistance} km");

            tourTable.SetMarginBottom(10);

            document.Add(tourTable);

            Paragraph tourMapHeader = new Paragraph("Map Overview")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER);
            document.Add(tourMapHeader);

            ImageData tourImage = ImageDataFactory.Create($"../../../TourImages/{tour.ImageName}");
            document.Add(new Image(tourImage).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetBorder(new SolidBorder(2)));

            document.Close();

            return 0;
        }

        public static int GeneratePdfAll()
        {
            List<TourData> tourEntries = new List<TourData>();
            //retrieve all tours from the database
            Task<Dictionary<string, object> > dbTourCollectionReturn = Task.Run(() => GetData.GetAllTours());

            int index = 0;
            while (dbTourCollectionReturn.Result.ContainsKey("id" + index))
            {
                tourEntries.Add(new TourData((int)dbTourCollectionReturn.Result["id" + index], (string)dbTourCollectionReturn.Result["name" + index], (string)dbTourCollectionReturn.Result["description" + index], (string)dbTourCollectionReturn.Result["start" + index], (string)dbTourCollectionReturn.Result["destination" + index],
                    (string)dbTourCollectionReturn.Result["transport_type" + index], (int)dbTourCollectionReturn.Result["distance" + index], (string)dbTourCollectionReturn.Result["estimated_time" + index], (string)dbTourCollectionReturn.Result["image" + index], (string)dbTourCollectionReturn.Result["popularity" + index], (string)dbTourCollectionReturn.Result["childfriendlyness" + index], (string)dbTourCollectionReturn.Result["favourite" + index]));
                index++;
            }

            //create pdf document
            PdfWriter writer = new PdfWriter(_targetFile);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            //add all tours from database into document
            bool firstIteration = true;
            foreach (TourData tour in tourEntries)
            {
                if (!firstIteration)
                {
                    document.Add(new AreaBreak());
                }

                Paragraph title = new Paragraph(tour.TourName)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(30)
                .SetTextAlignment(TextAlignment.CENTER);
                document.Add(title);

                Paragraph tourDescriptionHeader = new Paragraph("Description:")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(18);
                document.Add(tourDescriptionHeader);

                Paragraph tourDescription = new Paragraph(string.IsNullOrEmpty(tour.TourDescription) ? "/" : tour.TourDescription)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(15);
                document.Add(tourDescription);

                Paragraph tourDetailsHeader = new Paragraph("Details:")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(18);
                document.Add(tourDetailsHeader);

                Table tourTable = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
                tourTable.AddHeaderCell(new Cell().Add(new Paragraph("Start")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                tourTable.AddHeaderCell(new Cell().Add(new Paragraph("Destination")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                tourTable.AddHeaderCell(new Cell().Add(new Paragraph("Transport Type")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                tourTable.AddHeaderCell(new Cell().Add(new Paragraph("Time")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                tourTable.AddHeaderCell(new Cell().Add(new Paragraph("Distance")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                tourTable.AddCell(tour.Start);
                tourTable.AddCell(tour.Destination);
                tourTable.AddCell(tour.TransportType);
                tourTable.AddCell(tour.Time);
                tourTable.AddCell($"{tour.TourDistance} km");

                tourTable.SetMarginBottom(10);

                document.Add(tourTable);

                Paragraph tourMapHeader = new Paragraph("Map Overview")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER);
                document.Add(tourMapHeader);

                ImageData tourImage = ImageDataFactory.Create($"../../../TourImages/{tour.ImageName}");
                document.Add(new Image(tourImage).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetBorder(new SolidBorder(2)));

                firstIteration = false;
            }

            document.Close();

            return 0;
        }
    }
}
