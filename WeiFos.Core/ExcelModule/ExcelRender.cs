using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace WeiFos.Core.ExcelModule
{
    /// <summary>
    /// excel版本
    /// </summary>
    public enum ExcelVersion
    {
        /// <summary>
        /// 旧版excel, *.xls
        /// </summary>
        Excel_2003,

        /// <summary>
        /// 新版excel, *.xlsx
        /// </summary>
        Excel_2007,
    }

    /// <summary>
    /// 使用NPOI操作Excel，无需Office COM组件
    /// </summary>
    public class ExcelRender
    {
        /// <summary>
        /// 根据Excel列类型获取列的值
        /// </summary>
        /// <param name="cell">Excel列</param>
        /// <returns></returns>
        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }

        /// <summary>
        /// 自动设置Excel列宽
        /// </summary>
        /// <param name="sheet">Excel表</param>
        private static void AutoSizeColumns(ISheet sheet)
        {
            if (sheet.PhysicalNumberOfRows > 0)
            {
                IRow headerRow = sheet.GetRow(0);

                for (int i = 0, l = headerRow.LastCellNum; i < l; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
            }
        }

        /// <summary>
        /// 保存Excel文档流到文件
        /// </summary>
        /// <param name="ms">Excel文档流</param>
        /// <param name="fileName">文件名</param>
        private static void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();

                data = null;
            }
        }



        /// <summary>
        /// DataReader转换成Excel文档流
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static MemoryStream RenderToExcel(IDataReader reader)
        {
            MemoryStream ms = new MemoryStream();

            using (reader)
            {
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();

                IRow headerRow = sheet.CreateRow(0);
                int cellCount = reader.FieldCount;

                // handling header.
                for (int i = 0; i < cellCount; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(reader.GetName(i));
                }

                // handling value.
                int rowIndex = 1;
                while (reader.Read())
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    for (int i = 0; i < cellCount; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(reader[i].ToString());
                    }

                    rowIndex++;
                }

                AutoSizeColumns(sheet);

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;


            }
            return ms;
        }


        /// <summary>
        /// DataReader转换成Excel文档流，并保存到文件
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fileName">保存的路径</param>
        public static void RenderToExcel(IDataReader reader, string fileName)
        {
            using (MemoryStream ms = RenderToExcel(reader))
            {
                SaveToFile(ms, fileName);
            }
        }


        /// <summary>
        /// DataTable转换成Excel文档流
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static MemoryStream RenderToExcel(DataTable table)
        {
            MemoryStream ms = new MemoryStream();

            using (table)
            {
                IWorkbook workbook = new HSSFWorkbook();

                ISheet sheet = workbook.CreateSheet();
                IRow headerRow = sheet.CreateRow(0);

                // handling header.
                foreach (DataColumn column in table.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value

                // handling value.
                int rowIndex = 1;

                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }
                AutoSizeColumns(sheet);

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

            }
            return ms;
        }

        /// <summary>
        /// DataTable转换成Excel文档流，并保存到文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fileName">保存的路径</param>
        public static void RenderToExcel(DataTable table, string fileName)
        {
            using (MemoryStream ms = RenderToExcel(table))
            {
                SaveToFile(ms, fileName);
            }
        }


        /// <summary>
        /// Excel文档流是否有数据
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <returns></returns>
        public static bool HasData(Stream excelFileStream, ExcelVersion excelVer)
        {
            return HasData(excelFileStream, excelVer, 0);
        }

        /// <summary>
        /// Excel文档流是否有数据
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="excelVer">文档版本</param>
        /// <param name="sheetIndex">表索引号，如第一个表为0</param>
        /// <returns></returns>
        public static bool HasData(Stream excelFileStream, ExcelVersion excelVer, int sheetIndex)
        {
            using (excelFileStream)
            {
                IWorkbook workbook = null;
                switch (excelVer)
                {
                    case ExcelVersion.Excel_2007:
                        new XSSFWorkbook(excelFileStream);
                        break;
                    case ExcelVersion.Excel_2003:
                    default:
                        new HSSFWorkbook(excelFileStream);
                        break;
                }
                if (workbook.NumberOfSheets > 0)
                {
                    if (sheetIndex < workbook.NumberOfSheets)
                    {
                        ISheet sheet = workbook.GetSheetAt(sheetIndex);
                        return sheet.PhysicalNumberOfRows > 0;
                    }

                }
            }
            return false;
        }

        /// <summary>
        /// Excel文档流转换成DataTable
        /// 第一行必须为标题行
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="excelVer">文档版本</param>
        /// <param name="sheetName">表名称</param>
        /// <returns></returns>
        public static DataTable RenderFromExcelToDataTable(Stream excelFileStream, ExcelVersion excelVer, string sheetName)
        {
            return RenderFromExcelToDataTable(excelFileStream, excelVer, sheetName, 0);
        }

        /// <summary>
        /// Excel文档流转换成DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="excelVer">文档版本</param>
        /// <param name="sheetName">表名称</param>
        /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        /// <returns></returns>
        public static DataTable RenderFromExcelToDataTable(Stream excelFileStream, ExcelVersion excelVer, string sheetName, int headerRowIndex)
        {
            DataTable table = null;

            using (excelFileStream)
            {
                IWorkbook workbook = null;
                switch (excelVer)
                {
                    case ExcelVersion.Excel_2007:
                        new XSSFWorkbook(excelFileStream);
                        break;
                    case ExcelVersion.Excel_2003:
                    default:
                        new HSSFWorkbook(excelFileStream);
                        break;
                }
                ISheet sheet = workbook.GetSheet(sheetName);
                table = RenderFromSheetToDataTable(sheet, headerRowIndex);

            }
            return table;
        }

        /// <summary>
        /// Excel文档流转换成DataTable
        /// 默认转换Excel的第一个表
        /// 第一行必须为标题行
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="excelVer">文档版本</param>
        /// <returns></returns>
        public static DataTable RenderFromExcelToDataTable(Stream excelFileStream, ExcelVersion excelVer)
        {
            return RenderFromExcelToDataTable(excelFileStream, excelVer, 0, 0);
        }

        /// <summary>
        /// Excel文档流转换成DataTable
        /// 第一行必须为标题行
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="sheetIndex">表索引号，如第一个表为0</param>
        /// <returns></returns>
        public static DataTable RenderFromExcelToDataTable(Stream excelFileStream, ExcelVersion excelVer, int sheetIndex)
        {
            return RenderFromExcelToDataTable(excelFileStream, excelVer, sheetIndex, 0);
        }

        /// <summary>
        /// Excel文档流转换成DataSet
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <returns></returns>
        public static DataSet RenderFromExcelToDataSet(Stream excelFileStream, ExcelVersion excelVer)
        {
            DataSet ds = null;
            IWorkbook workbook = null;

            switch (excelVer)
            {
                case ExcelVersion.Excel_2007:
                    workbook = new XSSFWorkbook(excelFileStream);
                    break;
                case ExcelVersion.Excel_2003:
                default:
                    workbook = new HSSFWorkbook(excelFileStream);
                    break;
            }
            if (workbook != null)
            {
                ds = new DataSet();

                for (int sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
                {
                    ISheet sheet = workbook.GetSheetAt(sheetIndex);
                    DataTable dt = RenderFromSheetToDataTable(sheet, 0);
                    ds.Tables.Add(dt);
                }
            }

            return ds;
        }

        /// <summary>
        /// Excel文档流转换成DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="excelVer">文档版本</param>
        /// <param name="sheetIndex">表索引号，如第一个表为0</param>
        /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        /// <returns></returns>
        public static DataTable RenderFromExcelToDataTable(Stream excelFileStream, ExcelVersion excelVer, int sheetIndex, int headerRowIndex)
        {
            DataTable table = null;

            using (excelFileStream)
            {
                IWorkbook workbook = null;
                switch (excelVer)
                {
                    case ExcelVersion.Excel_2007:
                        new XSSFWorkbook(excelFileStream);
                        break;
                    case ExcelVersion.Excel_2003:
                    default:
                        new HSSFWorkbook(excelFileStream);
                        break;
                }
                ISheet sheet = workbook.GetSheetAt(sheetIndex);

                table = RenderFromSheetToDataTable(sheet, headerRowIndex);

            }
            return table;
        }

        /// <summary>
        /// Excel表格转换成DataTable
        /// </summary>
        /// <param name="sheet">表格</param>
        /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        /// <returns></returns>
        private static DataTable RenderFromSheetToDataTable(ISheet sheet, int headerRowIndex)
        {
            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = 0;
            int rowCount = 0;
            if (headerRow != null)
            {
                cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
                rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();

                    if (row != null)
                    {
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null) dataRow[j] = GetCellValue(row.GetCell(j));
                        }
                        table.Rows.Add(dataRow);
                    }

                }
            }
            //handling header.

            table.TableName = sheet.SheetName;

            return table;
        }

        /// <summary>
        /// Excel文档导入到数据库
        /// 默认取Excel的第一个表
        /// 第一行必须为标题行
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="excelVer">文档版本</param>
        /// <param name="insertSql">插入语句</param>
        /// <param name="dbAction">更新到数据库的方法</param>
        /// <returns></returns>
        public static int RenderToDb(Stream excelFileStream, ExcelVersion excelVer, string insertSql, DBAction dbAction)
        {
            return RenderToDb(excelFileStream, excelVer, insertSql, dbAction, 0, 0);
        }

        public delegate int DBAction(string sql, params IDataParameter[] parameters);

        /// <summary>
        /// Excel文档导入到数据库
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="excelVer">文档版本</param>
        /// <param name="insertSql">插入语句</param>
        /// <param name="dbAction">更新到数据库的方法</param>
        /// <param name="sheetIndex">表索引号，如第一个表为0</param>
        /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        /// <returns></returns>
        public static int RenderToDb(Stream excelFileStream, ExcelVersion excelVer, string insertSql, DBAction dbAction, int sheetIndex, int headerRowIndex)
        {
            int rowAffected = 0;
            using (excelFileStream)
            {
                IWorkbook workbook = null;
                switch (excelVer)
                {
                    case ExcelVersion.Excel_2007:
                        new XSSFWorkbook(excelFileStream);
                        break;
                    case ExcelVersion.Excel_2003:
                    default:
                        new HSSFWorkbook(excelFileStream);
                        break;
                }
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                StringBuilder builder = new StringBuilder();

                IRow headerRow = sheet.GetRow(headerRowIndex);
                int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
                int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

                for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        builder.Append(insertSql);
                        builder.Append(" values (");
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            builder.AppendFormat("'{0}',", GetCellValue(row.GetCell(j)).Replace("'", "''"));
                        }
                        builder.Length = builder.Length - 1;
                        builder.Append(");");
                    }

                    if ((i % 50 == 0 || i == rowCount) && builder.Length > 0)
                    {
                        //每50条记录一次批量插入到数据库
                        rowAffected += dbAction(builder.ToString());
                        builder.Length = 0;
                    }

                }
            }
            return rowAffected;
        }
    }
}