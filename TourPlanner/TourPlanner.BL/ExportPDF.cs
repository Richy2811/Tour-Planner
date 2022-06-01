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

        public static int GeneratePdfSingle(TourData tour, ObservableCollection<LogEntry> logs)
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

            #region Description

            Paragraph tourDescriptionHeader = new Paragraph("Description:")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(18);
            document.Add(tourDescriptionHeader);

            Paragraph tourDescription = new Paragraph(string.IsNullOrEmpty(tour.TourDescription) ? "/" : tour.TourDescription)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(15);
            document.Add(tourDescription);

            #endregion

            #region Tour details

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

            tourTable.SetMarginBottom(1);

            document.Add(tourTable);

            //more details
            Table tourTableExtra = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
            tourTableExtra.AddHeaderCell(new Cell().Add(new Paragraph("Popularity")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            tourTableExtra.AddHeaderCell(new Cell().Add(new Paragraph("Child Friendliness")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));

            tourTableExtra.AddCell(tour.Popularity);
            tourTableExtra.AddCell(tour.ChildFriendliness);

            tourTableExtra.SetMarginBottom(10);

            document.Add(tourTableExtra);

            #endregion

            #region Map image

            Paragraph tourMapHeader = new Paragraph("Map Overview")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER);
            document.Add(tourMapHeader);

            ImageData tourImage = ImageDataFactory.Create($"../../../TourImages/{tour.ImageName}");
            document.Add(new Image(tourImage).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetBorder(new SolidBorder(2)).SetMarginBottom(10));

            #endregion

            #region Logs

            //skip section if no logs have been created yet
            if (logs.Count > 0)
            {
                Paragraph logHeader = new Paragraph("Logs")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER);
                document.Add(logHeader);

                Table logTable = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
                logTable.AddHeaderCell(new Cell().Add(new Paragraph("Date")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                logTable.AddHeaderCell(new Cell().Add(new Paragraph("Duration")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                logTable.AddHeaderCell(new Cell().Add(new Paragraph("Comment")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                logTable.AddHeaderCell(new Cell().Add(new Paragraph("Difficulty")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                logTable.AddHeaderCell(new Cell().Add(new Paragraph("Rating")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                foreach (LogEntry entry in logs)
                {
                    logTable.AddCell(entry.Date);
                    logTable.AddCell(entry.Duration);
                    logTable.AddCell(entry.Comment);
                    logTable.AddCell(entry.Difficulty);
                    logTable.AddCell(entry.Rating);
                }

                document.Add(logTable);
            }

            #endregion

            document.Close();

            return 0;
        }

        public static int GeneratePdfAll()
        {
            List<TourData> tourEntries = new List<TourData>();
            List<LogEntry> tourLogEntries = new List<LogEntry>();
            //retrieve all tours from the database
            Dictionary<string, object> dbTourCollectionReturn = Task.Run(async () => await GetData.GetAllTours()).Result;

            int tourResultIndex = 0;
            while (dbTourCollectionReturn.ContainsKey("id" + tourResultIndex))
            {
                tourEntries.Add(new TourData((int)dbTourCollectionReturn["id" + tourResultIndex],
                    (string)dbTourCollectionReturn["name" + tourResultIndex],
                    (string)dbTourCollectionReturn["description" + tourResultIndex],
                    (string)dbTourCollectionReturn["start" + tourResultIndex],
                    (string)dbTourCollectionReturn["destination" + tourResultIndex],
                    (string)dbTourCollectionReturn["transport_type" + tourResultIndex],
                    (int)dbTourCollectionReturn["distance" + tourResultIndex],
                    (string)dbTourCollectionReturn["estimated_time" + tourResultIndex],
                    (string)dbTourCollectionReturn["image" + tourResultIndex],
                    (string)dbTourCollectionReturn["popularity" + tourResultIndex],
                    (string)dbTourCollectionReturn["childfriendlyness" + tourResultIndex],
                    (string)dbTourCollectionReturn["favourite" + tourResultIndex]));
                tourResultIndex++;
            }

            //create pdf document
            PdfWriter writer = new PdfWriter(_targetFile);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            //add all tours from database into document
            bool firstIteration = true;
            foreach (TourData tour in tourEntries)
            {
                tourLogEntries.Clear();

                if (!firstIteration)
                {
                    document.Add(new AreaBreak());
                }

                Paragraph title = new Paragraph(tour.TourName)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(30)
                .SetTextAlignment(TextAlignment.CENTER);
                document.Add(title);

                #region Description

                Paragraph tourDescriptionHeader = new Paragraph("Description:")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(18);
                document.Add(tourDescriptionHeader);

                Paragraph tourDescription = new Paragraph(string.IsNullOrEmpty(tour.TourDescription) ? "/" : tour.TourDescription)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(15);
                document.Add(tourDescription);

                #endregion

                #region Tour Details

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

                tourTable.SetMarginBottom(1);

                document.Add(tourTable);

                //more details
                Table tourTableExtra = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
                tourTableExtra.AddHeaderCell(new Cell().Add(new Paragraph("Popularity")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                tourTableExtra.AddHeaderCell(new Cell().Add(new Paragraph("Child Friendliness")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                tourTableExtra.AddCell(tour.Popularity);
                tourTableExtra.AddCell(tour.ChildFriendliness);

                tourTableExtra.SetMarginBottom(10);

                document.Add(tourTableExtra);

                #endregion

                #region Map image

                Paragraph tourMapHeader = new Paragraph("Map Overview")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER);
                document.Add(tourMapHeader);

                ImageData tourImage = ImageDataFactory.Create($"../../../TourImages/{tour.ImageName}");
                document.Add(new Image(tourImage).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetBorder(new SolidBorder(2)).SetMarginBottom(10));

                #endregion

                #region Logs

                //retrieve current tour log entries
                Dictionary<string, object> dbTourLogReturn = Task.Run(async () => await GetData.GetAllTourLogData(tour.ID)).Result;
                int logResultIndex = 0;
                if (dbTourLogReturn != null)
                {
                    while (dbTourLogReturn.ContainsKey("id" + logResultIndex))
                    {
                        tourLogEntries.Add(new LogEntry((int)dbTourLogReturn["id" + logResultIndex],
                            (string)dbTourLogReturn["date_time" + logResultIndex],
                            (string)dbTourLogReturn["comment" + logResultIndex],
                            (string)dbTourLogReturn["difficulty" + logResultIndex],
                            (string)dbTourLogReturn["total_time" + logResultIndex],
                            (string)dbTourLogReturn["rating" + logResultIndex]));
                        logResultIndex++;
                    }
                }

                //skip section if no logs have been created yet
                if (tourLogEntries.Count > 0)
                {
                    Paragraph logHeader = new Paragraph("Logs")
                        .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER);
                    document.Add(logHeader);

                    Table logTable = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
                    logTable.AddHeaderCell(new Cell().Add(new Paragraph("Date")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                    logTable.AddHeaderCell(new Cell().Add(new Paragraph("Duration")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                    logTable.AddHeaderCell(new Cell().Add(new Paragraph("Comment")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                    logTable.AddHeaderCell(new Cell().Add(new Paragraph("Difficulty")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                    logTable.AddHeaderCell(new Cell().Add(new Paragraph("Rating")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                    foreach (LogEntry entry in tourLogEntries)
                    {
                        logTable.AddCell(entry.Date);
                        logTable.AddCell(entry.Duration);
                        logTable.AddCell(entry.Comment);
                        logTable.AddCell(entry.Difficulty);
                        logTable.AddCell(entry.Rating);
                    }

                    document.Add(logTable);
                }

                #endregion

                firstIteration = false;
            }

            document.Close();

            return 0;
        }
    }
}
