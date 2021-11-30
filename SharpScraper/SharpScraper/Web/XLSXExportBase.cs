using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;
using X15 = DocumentFormat.OpenXml.Office2013.Excel;

namespace SharpScraper.Web
{
	/// <summary>
	/// Implements <see cref="IExportBase"/> that operates on .xlsx extension files.
	/// </summary>
	public class XLSXExportBase : IExportBase
	{
		/// <summary>
		/// String identifier for this <see cref="IExportBase"/>.
		/// </summary>
		public static readonly string Name = "XLSX";

		private static SheetData GenerateSheetdataForDetails(IDictionary<string, ICardTactic> cards)
		{
			var sheetData = new SheetData();

			sheetData.Append(new Row()
				.AppendCell("Name", 2)
				.AppendCell("Price", 2)
				.AppendCell("Rarity", 2)
				.AppendCell("Set Code", 2)
				.AppendCell("Set Name", 2)
				.AppendCell("URL", 2));

			foreach (var card in cards)
			{
				if (card.Value.IsNull)
				{
					continue;
				}

				sheetData.Append(new Row()
					.AppendCell(card.Value.Name)
					.AppendCell(card.Value.Price)
					.AppendCell(card.Value.Rarity)
					.AppendCell(card.Value.SetCode)
					.AppendCell(card.Value.SetName)
					.AppendCell(card.Key));
			}

			return sheetData;
		}

		private static void GenerateWorkbookPartContent(WorkbookPart workbookPart)
		{
			workbookPart.Workbook = new Workbook().AppendChild<Workbook, Sheets>(new Sheets().AppendChild<Sheets, Sheet>(new()
			{
				Name = "Card Database",
				Id = "BBB",
				SheetId = 1,
			}));
		}

		private static void GenerateWorkbookStylesContent(WorkbookStylesPart workbookStyles)
		{
			var stylesheet = new Stylesheet()
			{
				MCAttributes = new MarkupCompatibilityAttributes()
				{
					Ignorable = "x14ac",
				}
			};

			stylesheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			stylesheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

			var fonts = new Fonts() { Count = 2, KnownFonts = true }
				.AppendChild<Fonts, Font>(new Font()
					.AppendChild<Font, FontSize>(new() { Val = 11.0 })
					.AppendChild<Font, Color>(new() { Theme = 1 })
					.AppendChild<Font, FontName>(new() { Val = "Calibri" })
					.AppendChild<Font, FontFamilyNumbering>(new() { Val = 2 })
					.AppendChild<Font, FontScheme>(new() { Val = FontSchemeValues.Minor }))
				.AppendChild<Fonts, Font>(new Font()
					.AppendChild<Font, Bold>(new() { Val = true })
					.AppendChild<Font, FontSize>(new() { Val = 12.0 })
					.AppendChild<Font, Color>(new() { Theme = 1 })
					.AppendChild<Font, FontName>(new() { Val = "Calibri" })
					.AppendChild<Font, FontFamilyNumbering>(new() { Val = 2 })
					.AppendChild<Font, FontScheme>(new() { Val = FontSchemeValues.Minor }));

			var fills = new Fills()
				.AppendChild<Fills, Fill>(new Fill()
					.AppendChild<Fill, PatternFill>(new()
					{
						PatternType = PatternValues.None,
					}))
				.AppendChild<Fills, Fill>(new Fill()
					.AppendChild<Fill, PatternFill>(new()
					{
						PatternType = PatternValues.Gray125,
					}));

			var borders = new Borders() { Count = 2 }
				.AppendChild<Borders, Border>(new Border()
					.AppendChild<Border, LeftBorder>(new())
					.AppendChild<Border, RightBorder>(new())
					.AppendChild<Border, TopBorder>(new())
					.AppendChild<Border, BottomBorder>(new())
					.AppendChild<Border, DiagonalBorder>(new()))
				.AppendChild<Borders, Border>(new Border()
					.AppendChild<Border, LeftBorder>(new LeftBorder() { Style = BorderStyleValues.Thin }
						.AppendChild<LeftBorder, Color>(new() { Indexed = 64 }))
					.AppendChild<Border, RightBorder>(new RightBorder() { Style = BorderStyleValues.Thin }
						.AppendChild<RightBorder, Color>(new() { Indexed = 64 }))
					.AppendChild<Border, TopBorder>(new TopBorder() { Style = BorderStyleValues.Thin }
						.AppendChild<TopBorder, Color>(new() { Indexed = 64 }))
					.AppendChild<Border, BottomBorder>(new BottomBorder() { Style = BorderStyleValues.Thin }
						.AppendChild<BottomBorder, Color>(new() { Indexed = 64 }))
					.AppendChild<Border, DiagonalBorder>(new()));

			var cellStyleFormats = new CellStyleFormats() { Count = 1 }.AppendChild<CellStyleFormats, CellFormat>(new()
			{
				NumberFormatId = 0,
				FontId = 0,
				FillId = 0,
				BorderId = 0,
			});

			var cellFormats = new CellFormats() { Count = 3 }
				.AppendChild<CellFormats, CellFormat>(new()
				{
					NumberFormatId = 0,
					FontId = 0,
					FillId = 0,
					BorderId = 0,
					FormatId = 0,
				})
				.AppendChild<CellFormats, CellFormat>(new()
				{
					NumberFormatId = 0,
					FontId = 0,
					FillId = 0,
					BorderId = 1,
					FormatId = 0,
					ApplyBorder = true,
				})
				.AppendChild<CellFormats, CellFormat>(new()
				{
					NumberFormatId = 0,
					FontId = 1,
					FillId = 0,
					BorderId = 1,
					FormatId = 0,
					ApplyFont = true,
					ApplyBorder = true,
				});

			var cellStyles = new CellStyles() { Count = 1 }
				.AppendChild<CellStyles, CellStyle>(new()
				{
					Name = "Normal",
					FormatId = 0,
					BuiltinId = 0,
				});

			var differentialFormats = new DifferentialFormats()
			{
				Count = 0
			};

			var tableStyles = new TableStyles()
			{
				Count = 0,
				DefaultTableStyle = "TableStyleMedium2",
				DefaultPivotStyle = "PivotStyleLight16"
			};

			var stylesheetExtension1 = new StylesheetExtension()
			{
				Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}",
			};

			var stylesheetExtension2 = new StylesheetExtension()
			{
				Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}",
			};

			stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
			stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");

			var stylesheetExtensionList = new StylesheetExtensionList()
				.AppendChild<StylesheetExtensionList, StylesheetExtension>(stylesheetExtension1
					.AppendChild<StylesheetExtension, X14.SlicerStyles>(new()
					{
						DefaultSlicerStyle = "SlicerStyleLight1",
					}))
				.AppendChild<StylesheetExtensionList, StylesheetExtension>(stylesheetExtension2
					.AppendChild<StylesheetExtension, X15.TimelineStyles>(new()
					{
						DefaultTimelineStyle = "TimeSlicerStyleLight1",
					}));

			workbookStyles.Stylesheet = stylesheet
				.AppendChild<Stylesheet, Fonts>(fonts)
				.AppendChild<Stylesheet, Fills>(fills)
				.AppendChild<Stylesheet, Borders>(borders)
				.AppendChild<Stylesheet, CellStyleFormats>(cellStyleFormats)
				.AppendChild<Stylesheet, CellFormats>(cellFormats)
				.AppendChild<Stylesheet, CellStyles>(cellStyles)
				.AppendChild<Stylesheet, DifferentialFormats>(differentialFormats)
				.AppendChild<Stylesheet, TableStyles>(tableStyles)
				.AppendChild<Stylesheet, StylesheetExtensionList>(stylesheetExtensionList);
		}

		private static void GenerateWorksheetPartContent(WorksheetPart worksheetPart, IDictionary<string, ICardTactic> cards)
		{
			var worksheet = new Worksheet()
			{
				MCAttributes = new MarkupCompatibilityAttributes()
				{
					Ignorable = "x14ac",
				}
			};

			worksheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
			worksheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			worksheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

			var sheetDimension = new SheetDimension()
			{
				Reference = "A1",
			};

			var sheetViews = new SheetViews()
				.AppendChild<SheetViews, SheetView>(new SheetView()
				{
					TabSelected = true,
					WorkbookViewId = 0,
				}.AppendChild<SheetView, Selection>(new()
				{
					ActiveCell = "A1",
					SequenceOfReferences = new()
					{
						InnerText = "A1",
					},
				}));

			var sheetFormat = new SheetFormatProperties()
			{
				DefaultRowHeight = 15.0,
				DyDescent = 0.25,
			};

			var columns = new Columns()
				.AppendChild<Columns, Column>(new()
				{
					Min = 1,
					Max = 1,
					CustomWidth = true,
					Width = Math.Max(cards.Values.Max(_ => _.Name.Length), "Name".Length) * 10.0 / 7.0,
				})
				.AppendChild<Columns, Column>(new()
				{
					Min = 2,
					Max = 2,
					CustomWidth = true,
					Width = Math.Max(cards.Values.Max(_ => _.Price.ToString().Length), "Price".Length) * 10.0 / 7.0,
				})
				.AppendChild<Columns, Column>(new()
				{
					Min = 3,
					Max = 3,
					CustomWidth = true,
					Width = Math.Max(cards.Values.Max(_ => _.Rarity.Length), "Rarity".Length) * 10.0 / 7.0,
				})
				.AppendChild<Columns, Column>(new()
				{
					Min = 4,
					Max = 4,
					CustomWidth = true,
					Width = Math.Max(cards.Values.Max(_ => _.SetCode.Length), "Set Code".Length) * 10.0 / 7.0,
				})
				.AppendChild<Columns, Column>(new()
				{
					Min = 5,
					Max = 5,
					CustomWidth = true,
					Width = Math.Max(cards.Values.Max(_ => _.SetName.Length), "Set Name".Length) * 10.0 / 7.0,
				})
				.AppendChild<Columns, Column>(new()
				{
					Min = 6,
					Max = 6,
					CustomWidth = true,
					Width = Math.Max(cards.Keys.Max(_ => _.Length), "URL".Length) * 10.0 / 7.0,
				});

			var sheetData = XLSXExportBase.GenerateSheetdataForDetails(cards);

			var pageMargins = new PageMargins()
			{
				Top = 0.75,
				Left = 0.70,
				Right = 0.70,
				Bottom = 0.75,
				Header = 0.30,
				Footer = 0.30,
			};

			worksheetPart.Worksheet = worksheet
				.AppendChild<Worksheet, SheetDimension>(sheetDimension)
				.AppendChild<Worksheet, SheetViews>(sheetViews)
				.AppendChild<Worksheet, SheetFormatProperties>(sheetFormat)
				.AppendChild<Worksheet, Columns>(columns)
				.AppendChild<Worksheet, SheetData>(sheetData)
				.AppendChild<Worksheet, PageMargins>(pageMargins);
		}

		/// <inheritdoc/>
		public Task Export(Stream stream, IDictionary<string, ICardTactic> cards)
		{
			using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
			{
				var workbookPart = document.AddWorkbookPart();

				XLSXExportBase.GenerateWorkbookPartContent(workbookPart);

				var workbookStyles = workbookPart.AddNewPart<WorkbookStylesPart>("AAA");

				XLSXExportBase.GenerateWorkbookStylesContent(workbookStyles);

				var worksheetPart = workbookPart.AddNewPart<WorksheetPart>("BBB");

				XLSXExportBase.GenerateWorksheetPartContent(worksheetPart, cards);
			}

			return Task.CompletedTask;
		}
	}
}
