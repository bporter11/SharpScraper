using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System;

namespace SharpScraper
{
	public static class OpenXMLUtils
	{
		public static Row AppendCell<T>(this Row row, T value, uint style = 1)
		{
			var type = Type.GetTypeCode(value?.GetType()) switch
			{
				TypeCode.Boolean => CellValues.Boolean,
				TypeCode.Byte => CellValues.Number,
				TypeCode.SByte => CellValues.Number,
				TypeCode.Int16 => CellValues.Number,
				TypeCode.UInt16 => CellValues.Number,
				TypeCode.Int32 => CellValues.Number,
				TypeCode.UInt32 => CellValues.Number,
				TypeCode.Int64 => CellValues.Number,
				TypeCode.UInt64 => CellValues.Number,
				TypeCode.Single => CellValues.Number,
				TypeCode.Double => CellValues.Number,
				TypeCode.Decimal => CellValues.Number,
				TypeCode.DateTime => CellValues.Date,
				TypeCode.Char => CellValues.String,
				TypeCode.String => CellValues.String,
				_ => CellValues.String,
			};

			row.Append(new Cell()
			{
				DataType = type,
				StyleIndex = style,
				CellValue = new CellValue(value?.ToString() ?? String.Empty),
			});

			return row;
		}

		public static T AppendChild<T, S>(this T element, S value)
			where T : OpenXmlCompositeElement
			where S : OpenXmlElement
		{
			element.Append(value);
			return element;
		}
	}
}
