using System.Collections.Generic;
using System.Linq;
using XmlModels;

namespace NotebookChoice.Models;

public class QuestionViewModel
{
    
    public QuestionViewModel(Question question)
    {
        this.Question = question.Text;
        Variants = question.Children.OrderBy(x => x.Key)
            .Select(x => new VariantViewModel(x))
            .ToList();

        Variants.First().IsChecked = true;
    }

    public VariantViewModel Selected => Variants.First(x => x.IsChecked);
    public string Question { get; set; }
    public List<VariantViewModel> Variants { get; set; }
}

public class VariantViewModel
{
    public string Text { get; set; }
    public bool IsChecked { get; set; }
    public Node Next { get; set; }

    public VariantViewModel(KeyValuePair<string, Node> kv)
    {
        Text = kv.Key.Trim();
        Next = kv.Value;
    }
}