﻿using System;
using System.Collections.Generic;
using System.Linq;
using GitlabCmd.Console.Utilities;

namespace GitlabCmd.Console.App
{
    public class OutputPresenter
    {
        public void Info(string text)
            => WriteLine(text);

        public void Error(string text)
            => WriteLine($"Error: {text}");

        public void SuccessResult(string header)
        {
            WriteLine("-------------------------");
            WriteLine(header);
        }

        public void FailureResult(string header, string error)
        {
            WriteLine("-------------------------");
            WriteLine(header);
            WriteLine($"Error: {error}");
        }

        public void GridResult(
            string header,
            string[] columnHeaders,
            IEnumerable<object[]> rows)
        {
            var inputRows = rows.ToList();
            if (inputRows.Select(r => r.Length).Any(l => l != columnHeaders.Length))
                throw new ArgumentOutOfRangeException();

            WriteLine("-------------------------");
            WriteLine(header);
            WriteLine("\r\n");

            string mainHeader = "";
            string underlineHeader = "";

            var columnWidths = new List<int>();
            for (int i = 0; i < columnHeaders.Length; i++)
            {
                int maxTextLength = inputRows.Select(row => row[i].SafeToString().Length).Max();
                int columnHeaderLength = columnHeaders[i].Length;
                int columnLength = Math.Max(maxTextLength, columnHeaderLength);

                mainHeader += "  " + EnsureLength(columnHeaders[i], columnLength);
                underlineHeader += "  " + '_'.Expand(columnLength);
                columnWidths.Add(columnLength);
            }

            var mainRows = new List<string>();
            foreach (var row in inputRows)
            {
                string mainRow = "";
                for (int i = 0; i < row.Length; i++)
                    mainRow += "  " + EnsureLength(row[i].SafeToString(), columnWidths[i]);

                mainRows.Add(mainRow);
            }

            WriteLine(mainHeader);
            WriteLine(underlineHeader);
            foreach (string mainRow in mainRows)
                WriteLine(mainRow);
        }

        private static string EnsureLength(string text, int length)
        {
            if (text.Length == length)
                return text;

            return text.Length < length ?
                text.PadRight(length) :
                text.Substring(0, length);
        }

        private void WriteLine(string text)
            => System.Console.WriteLine(text);
    }
}
