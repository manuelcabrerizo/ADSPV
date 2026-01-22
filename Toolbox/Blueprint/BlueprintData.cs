using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Rexar.Toolbox.Blueprint
{
    internal sealed class BlueprintData
    {
        internal string this[string blueprintID, string parameter] => rawContent[blueprintIDs.IndexOf(blueprintID) + 1, parameters.IndexOf(parameter) + 1];

        public string Get(string blueprintID, string parameter)
        {
            int indexBP = blueprintIDs.IndexOf(blueprintID) + 1;
            int indexPA = parameters.IndexOf(parameter) + 1;
            string result = rawContent[indexBP, indexPA];
            return result;
        }


        private readonly string[,] rawContent;
        private readonly List<string> blueprintIDs;
        private readonly List<string> parameters;
        public List<string> BlueprintIDs => blueprintIDs;
        public List<string> Parameters => parameters;

        public BlueprintData(ISheet sheet)
        {
            int maxRow = 0;
            int maxColumn = 0;

            for (int row = sheet.FirstRowNum; row <= sheet.LastRowNum; row++)
            {
                IRow sheetRow = sheet.GetRow(row);
                if (sheetRow == null)
                    continue;
                for (int column = sheetRow.FirstCellNum; column < sheetRow.LastCellNum; column++)
                { 
                    ICell cell = sheetRow.GetCell(column);
                    if (cell == null)
                        continue;

                    if (cell.CellType == CellType.Blank)
                        continue;

                    if (row + 1 > maxRow)
                        maxRow = row + 1;

                    if (column + 1 > maxColumn)
                        maxColumn = column + 1;
                }
            }

            rawContent = new string[ maxRow, maxColumn ];

            for (int row = sheet.FirstRowNum; row <= sheet.LastRowNum; row++)
            {
                IRow sheetRow = sheet.GetRow(row);
                if (sheetRow == null)
                    continue;
                for (int column = sheetRow.FirstCellNum; column < sheetRow.LastCellNum; column++)
                {
                    ICell cell = sheetRow.GetCell(column);
                    if (cell == null)
                        continue;

                    if (cell.CellType == CellType.Blank)
                        continue;

                    rawContent[row, column] = cell.ToString();
                }
            }

            blueprintIDs = new List<string>();
            for (int i = 1; i < rawContent.GetLength(0); i++)
            { 
                blueprintIDs.Add(rawContent[i, 0]);
            }

            parameters = new List<string>();
            for (int i = 1; i < rawContent.GetLength(1); i++)
            {
                parameters.Add(rawContent[0, i]);
            }
        }
    }

}
