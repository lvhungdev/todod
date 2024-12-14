namespace ConsoleApp.UI;

public class TableUIFactory
{
    private readonly List<string> _header;
    private readonly List<List<string>> _content;

    public TableUIFactory(List<string> header, List<List<string>> content)
    {
        if (content.Any(row => row.Count != header.Count))
        {
            throw new Exception("header and content do not have the same size");
        }

        List<string> headerRow = [];
        List<List<string>> contentRow = content.Select(_ => new List<string>()).ToList();

        List<int> countEachCol = GetMaxCountEachColumn(content);

        for (int i = 0; i < countEachCol.Count; i++)
        {
            if (countEachCol[i] == 0) continue;

            headerRow.Add(header[i]);

            for (int j = 0; j < content.Count; j++)
            {
                contentRow[j].Add(content[j][i]);
            }
        }

        _header = headerRow;
        _content = contentRow;
    }

    public string Create()
    {
        string result = "";

        _content.Add(_header);
        List<int> countEachCol = GetMaxCountEachColumn(_content);
        _content.RemoveAt(_content.Count - 1);

        result += string.Join(" ", GetAlignedRow(_header, countEachCol)) + "\n";
        result += string.Join(" ", GetDividers('-', countEachCol)) + "\n";

        foreach (List<string> row in _content)
        {
            result += string.Join(" ", GetAlignedRow(row, countEachCol)) + "\n";
        }

        return result;
    }

    private static List<string> GetDividers(char divider, List<int> countEachCol)
    {
        return countEachCol.Select(m => string.Join("", Enumerable.Repeat(divider, m))).ToList();
    }

    private static List<string> GetAlignedRow(List<string> row, List<int> countEachCol)
    {
        IEnumerable<(string col, int size)> combined = row.Zip(countEachCol, (m1, m2) => (m1, m2));

        return combined
            .Select((m, i) => row[i] + string.Join("", Enumerable.Repeat(" ", m.size - m.col.Length)))
            .ToList();
    }

    private static List<int> GetMaxCountEachColumn(List<List<string>> rows)
    {
        if (rows.Count == 0) return [];

        List<int> result = Enumerable.Repeat(0, rows.First().Count).ToList();

        foreach (List<string> row in rows)
        {
            for (int i = 0; i < row.Count; i++)
            {
                result[i] = row[i].Length > result[i] ? row[i].Length : result[i];
            }
        }

        return result;
    }
}
