using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WeiFos.Core.ExcelModule
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public class ExcelHelper
    {

        #region 由DataSet导出Excel

        private static Stream ExportDataSetToExcel(DataSet sourceDs, string[] sheetNames)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            for (int i = 0; i < sheetNames.Length; i++)
            {
                ISheet sheet = string.IsNullOrEmpty(sheetNames[i]) ? workbook.CreateSheet() : workbook.CreateSheet(sheetNames[i]);
                IRow headerRow = sheet.CreateRow(0);
                // handling header.
                foreach (DataColumn column in sourceDs.Tables[i].Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

                // handling value.
                int rowIndex = 1;

                foreach (DataRow row in sourceDs.Tables[i].Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in sourceDs.Tables[i].Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            workbook = null;
            return ms;
        }

        /// <summary>
        /// 由DataSet导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="sheetName">指定Excel工作表名称集合,不指定则设为空/null</param>
        public static void ExportDataSetToExcel(DataSet sourceDs, string fileName, string[] sheetNames)
        {
            MemoryStream ms = ExportDataSetToExcel(sourceDs, sheetNames) as MemoryStream;
             
            //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            //HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            //HttpContext.Current.Response.End();
            ms.Close();
            ms = null;
        }
        #endregion

        #region 由DataReader导出Excel

        public static MemoryStream ExportDataReaderToExcel(IDataReader reader, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = string.IsNullOrEmpty(sheetName) ? workbook.CreateSheet() : workbook.CreateSheet(sheetName);
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

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

        #endregion

        #region 由DataTable导出Excel

        private static Stream ExportDataTableToExcel(DataTable sourceTable, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = string.IsNullOrEmpty(sheetName) ? workbook.CreateSheet() : workbook.CreateSheet(sheetName);
            IRow headerRow = sheet.CreateRow(0);
            // handling header.
            foreach (DataColumn column in sourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            // handling value.
            int rowIndex = 1;

            foreach (DataRow row in sourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in sourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }
        /// <summary>
        /// 由DataTable导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="sheetName">指定Excel工作表名称,不指定则设为空/null</param>
        /// <returns>Excel工作表</returns>
        public static void ExportDataTableToExcel(DataTable sourceTable, string fileName, string sheetName)
        {
            MemoryStream ms = ExportDataTableToExcel(sourceTable, sheetName) as MemoryStream;
            //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            //HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            //HttpContext.Current.Response.End();
            ms.Close();
            ms = null;
        }

        #endregion

        #region Excel导入

        /// <summary>
        /// 由Excel导入DataTable
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="headerRowIndex">Excel表头行(标题行)索引,0为第一行</param>
        /// <returns></returns>
        private static DataTable ImportDataTableFromExcel(ISheet sheet, int headerRowIndex)
        {
            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum; //LastCellNum = PhysicalNumberOfCells, LastRowNum = PhysicalNumberOfRows - 1

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                    dataRow[j] = row.GetCell(j).ToString();
            }

            return table;
        }

        /// <summary>
        /// 由Excel导入DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件路径，为物理路径</param>
        /// <param name="sheetName">Excel工作表名称</param>
        /// <param name="headerRowIndex">Excel表头行(标题行)索引,0为第一行</param>
        /// <returns></returns>
        public static DataTable ImportDataTableFromExcel(string excelFilePath, string sheetName, int headerRowIndex)
        {
            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheet(sheetName);
                DataTable table = ImportDataTableFromExcel(sheet, headerRowIndex);
                workbook = null;
                sheet = null;
                return table;
            }
        }

        /// <summary>
        /// 由Excel导入DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件路径，为物理路径</param>
        /// <param name="sheetIndex">Excel工作表索引</param>
        /// <param name="headerRowIndex">Excel表头行(标题行)索引,0为第一行</param>
        /// <returns></returns>
        public static DataTable ImportDataTableFromExcel(string excelFilePath, int sheetIndex, int headerRowIndex)
        {
            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                DataTable table = ImportDataTableFromExcel(sheet, headerRowIndex);
                workbook = null;
                sheet = null;
                return table;
            }
        }

        /// <summary>
        /// 由Excel导入DataSet，如果有多个工作表，则包含多个DataTable
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径，为物理路径</param>
        /// <param name="headerRowIndex">Excel表头行索引,0为第一行</param>
        /// <returns></returns>
        public static DataSet ImportDataSetFromExcel(string excelFilePath, int headerRowIndex)
        {
            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                DataSet ds = new DataSet();
                HSSFWorkbook workbook = new HSSFWorkbook(stream);
                for (int index = 0, count = workbook.NumberOfSheets; index < count; index++)
                {
                    ISheet sheet = workbook.GetSheetAt(index);
                    DataTable table = ImportDataTableFromExcel(sheet, headerRowIndex);
                    ds.Tables.Add(table);
                    sheet = null;
                }
                workbook = null;
                return ds;
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 检测Excel是否有数据
        /// </summary>
        /// <param name="excelFileStream"></param>
        /// <returns></returns>
        public static bool HasData(Stream excelFileStream)
        {
            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);
                if (workbook.NumberOfSheets > 0)
                {
                    return workbook.GetSheetAt(0).PhysicalNumberOfRows > 0;
                }
            }
            return false;
        }

        /// <summary>
        /// 保存文件到硬盘
        /// </summary>
        /// <param name="ms">数据流</param>
        /// <param name="fileName">保存的文件名称</param>
        public static void SaveToFile(MemoryStream ms, string fileName)
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
        /// 将Excel的列索引转换为列名，列索引从0开始，列名从A开始。如第0列为A，第1列为B...
        /// </summary>
        /// <param name="index">列索引</param>
        /// <returns>列名，如第0列为A，第1列为B...</returns>
        public static string ConvertColumnIndexToColumnName(int index)
        {
            index = index + 1;
            int system = 26;
            char[] digArray = new char[100];
            int i = 0;
            while (index > 0)
            {
                int mod = index % system;
                if (mod == 0) mod = system;
                digArray[i++] = (char)(mod - 1 + 'A');
                index = (index - 1) / 26;
            }
            StringBuilder sb = new StringBuilder(i);
            for (int j = i - 1; j >= 0; j--)
            {
                sb.Append(digArray[j]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 转化日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static DateTime ConvertDate(string date)
        {
            DateTime dt = new DateTime();
            string[] time = date.Split('-');
            int year = Convert.ToInt32(time[2]);
            int month = Convert.ToInt32(time[0]);
            int day = Convert.ToInt32(time[1]);
            string years = Convert.ToString(year);
            string months = Convert.ToString(month);
            string days = Convert.ToString(day);
            if (months.Length == 4)
            {
                dt = Convert.ToDateTime(date);
            }
            else
            {
                string rq = "";
                if (years.Length == 1)
                {
                    years = "0" + years;
                }
                if (months.Length == 1)
                {
                    months = "0" + months;
                }
                if (days.Length == 1)
                {
                    days = "0" + days;
                }
                rq = "20" + years + "-" + months + "-" + days;
                dt = Convert.ToDateTime(rq);
            }
            return dt;
        }


        #endregion

    }
}
