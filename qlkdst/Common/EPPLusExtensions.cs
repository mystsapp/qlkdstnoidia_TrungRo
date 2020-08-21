using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace qlkdst.Common
{
    [AttributeUsage(AttributeTargets.All)]
    public class Column : System.Attribute
    {
        public int ColumnIndex { get; set; }


        public Column(int column)
        {
            ColumnIndex = column;
        }
    }

    public static class EPPLusExtensions
    {
        public static DataSet ExcelToDataTableMultiSheet(this ExcelPackage package)
        {
            DataSet ds = new DataSet();
            ExcelWorksheets workSheets = package.Workbook.Worksheets;

            foreach (ExcelWorksheet workSheet in workSheets)
            {
                try
                {
                    DataTable dt = new DataTable();

                    foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                    {
                        dt.Columns.Add(firstRowCell.Text);

                    }
                    for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                    {
                        var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                        var newRow = dt.NewRow();
                        foreach (var cell in row)
                        {
                            if (cell.Value != null)
                            {
                                newRow[cell.Start.Column - 1] = cell.Value.ToString();
                                //newRow[cell.Start.Column - 1] = cell.Text;
                            }
                            else
                            {
                                newRow[cell.Start.Column - 1] = "";
                            }


                        }
                        dt.Rows.Add(newRow);
                    }

                    ds.Tables.Add(dt);
                }
                catch (Exception ex)
                {

                }
            }


            return ds;
        }
        public static DataTable ExcelToDataTableRichText(this ExcelPackage package)
        {
            DataTable dt = new DataTable();
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                dt.Columns.Add(firstRowCell.Text);

            }
            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = dt.NewRow();
                foreach (var cell in row)
                {
                    if (cell.Value != null)
                    {
                        newRow[cell.Start.Column - 1] = cell.Value.ToString();
                    }
                    else
                    {
                        newRow[cell.Start.Column - 1] = "";
                    }


                }
                dt.Rows.Add(newRow);
            }

            return dt;
        }
        public static DataTable ExcelToDataTable(this ExcelPackage package)
        {
            DataTable dt = new DataTable();
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                dt.Columns.Add(firstRowCell.Text);

            }
            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = dt.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                dt.Rows.Add(newRow);
            }

            return dt;
        }
        public static IEnumerable<T> ConvertSheetToObjects<T>(this ExcelWorksheet worksheet) where T : new()
        {

            Func<CustomAttributeData, bool> columnOnly = y => y.AttributeType == typeof(Column);

            var columns = typeof(T)
                    .GetProperties()
                    .Where(x => x.CustomAttributes.Any(columnOnly))
            .Select(p => new
            {
                Property = p,
                Column = p.GetCustomAttributes<Column>().First().ColumnIndex //safe because if where above
            }).ToList();


            var rows = worksheet.Cells
                .Select(cell => cell.Start.Row)
                .Distinct()
                .OrderBy(x => x);


            //Create the collection container
            var collection = rows.Skip(1)
                .Select(row =>
                {
                    var tnew = new T();
                    columns.ForEach(col =>
                    {
                        //This is the real wrinkle to using reflection - Excel stores all numbers as double including int
                        var val = worksheet.Cells[row, col.Column];
                        //If it is numeric it is a double since that is how excel stores all numbers
                        if (val.Value == null)
                        {
                            col.Property.SetValue(tnew, null);
                            return;
                        }
                        if (col.Property.PropertyType == typeof(Int32))
                        {
                            col.Property.SetValue(tnew, val.GetValue<int>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(double))
                        {
                            col.Property.SetValue(tnew, val.GetValue<double>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(DateTime))
                        {
                            col.Property.SetValue(tnew, val.GetValue<DateTime>());
                            return;
                        }
                        //Its a string
                        col.Property.SetValue(tnew, val.GetValue<string>());
                    });

                    return tnew;
                });


            //Send it back
            return collection;
        }
    }
}