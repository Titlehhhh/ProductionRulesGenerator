using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using AngleSharp.Html.Parser;
using Microsoft.Xml.Serialization.GeneratedAssembly;


namespace XmlModels;

public static class ParserDrawIO
{
    private static bool IsQuestion(MxCell cell)
    {
        if (string.IsNullOrWhiteSpace(cell.Style))
            return false;
        return cell.Style.Contains("ellipse");
    }

    public static bool IsResult(MxCell cell)
    {
        if (string.IsNullOrWhiteSpace(cell.Style))
            return false;
        if (cell.MxGeometry is null)
            return false;
        return cell.Vertex == 1 && cell.Parent == "1";
    }

    private static string ExtractValue(string value)
    {
        try
        {
            HtmlParser parser = new HtmlParser();
            using var document = parser.ParseDocument(value);
            return document.Body.TextContent.Trim().Replace("\u00a0", " ").Replace("\n", " ").Replace("\r", "")
                .Replace("  ", " ");
        }
        catch (Exception e)
        {
            return value;
        }
    }

    private static Question ToQuestion(MxCell cell)
    {
        Question question = new Question();
        question.Text = ExtractValue(cell.Value!);
        question.OriginalCell = cell;
        return question;
    }

    private static Result ToResult(MxCell cell)
    {
        Result result = new Result();
        result.Name = ExtractValue(cell.Value!);
        result.OriginalCell = cell;
        return result;
    }


    private static Dictionary<string, string> StyleToDict(this string? style)
    {
        if (string.IsNullOrWhiteSpace(style))
            return new Dictionary<string, string>();
        Dictionary<string, string> result = new Dictionary<string, string>();
        foreach (var se in style.Split(';'))
        {
            string[] splits = se.Split('=');
            if (splits.Length == 1)
            {
                result.Add(splits[0], "");
            }
            else if (splits.Length == 2)
            {
                result[splits[0]] = splits[1];
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        return result;
    }

    private static string DictToStyle(this Dictionary<string, string> styles)
    {
        List<string> result = new List<string>();
        foreach (var (key, val) in styles)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                result.Add($"{key}");
            }
            else
            {
                result.Add($"{key}={val}");
            }
        }

        return string.Join(';', result);
    }


    private static bool IsArrow(MxCell cell)
    {
        if (cell.MxGeometry?.MxPoints.Count == 0)
            return false;
        return !string.IsNullOrWhiteSpace(cell.Source) && !string.IsNullOrWhiteSpace(cell.Target);
    }


    private static readonly MxFileSerializer Serializer = new();

    public static MxFile DeserializeFile(Stream stream)
    {
        using StreamReader reader = new StreamReader(stream);
        return (MxFile)Serializer.Deserialize(reader)!;
    }

    [RequiresUnreferencedCode("Serialization may require types that cannot be statically analyzed")]
    public static Tree Parse(Diagram diagram, MxFile original)
    {
        Tree tree = new Tree();
        tree.XmlModel = original;
        var cells = diagram.MxGraphModel.Root.MxCells;


        int id = 1;
        List<MxCell> results = new();
        List<MxCell> questions = new();
        List<MxCell> arrows = new();
        foreach (var mxCell in cells)
        {
            if (IsQuestion(mxCell))
            {
                questions.Add(mxCell);
            }

            if (IsArrow(mxCell))
            {
                arrows.Add(mxCell);
            }
        }

        foreach (var mxCell in cells)
        {
            if (string.IsNullOrWhiteSpace(mxCell.Style))
                continue;

            if (questions.Any(x => ReferenceEquals(x, mxCell)) || arrows.Any(x => ReferenceEquals(x, mxCell)))
                continue;
            results.Add(mxCell);
        }

        questions.OrderBy(x => x.MxGeometry.Y)
            .ToList()
            .ForEach(x =>
            {
                var question = ToQuestion(x);
                question.Id = id++;
                var styles = x.Style.StyleToDict();
                styles["enumerate"] = "1";
                styles["enumerateValue"] = question.Id.ToString();

                x.Style = styles.DictToStyle();
                tree.Add(x.Id!, question);
            });
        results.OrderBy(x => x.MxGeometry.X)
            .ToList()
            .ForEach(x =>
            {
                var result = ToResult(x);
                result.Id = id++;
                var styles = x.Style.StyleToDict();
                styles["enumerate"] = "1";
                styles["enumerateValue"] = result.Id.ToString();
                x.Style = styles.DictToStyle();
                tree.Add(x.Id!, result);
            });


        foreach (var arrow in cells.ToList())
        {
            if (string.IsNullOrWhiteSpace(arrow.Value))
                continue;

            if (IsArrow(arrow))
            {
                string value = ExtractValue(arrow.Value);

                //Console.WriteLine(value);
                string from = arrow.Source;
                string to = arrow.Target;
                tree.ConnectFromTo(from, to, value);
            }
        }

        tree.SetRoot();
        return tree;
    }
}