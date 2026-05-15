using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Revit_ToolKit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class CheckDuplicateViewsCommand : ExternalCommand
    {
        public override void Execute()
        {
            string projectName = Path.GetFileNameWithoutExtension(Document.Title);

            var duplicateGroups = new FilteredElementCollector(Document)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => !v.IsTemplate)
                .GroupBy(v => v.Name)
                .Where(g => g.Count() > 1)
                .ToList();

            if (!duplicateGroups.Any())
            {
                TaskDialog.Show("Duplicate Views", "No duplicate views found in this project.");
                return;
            }

            string addinFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string outputFolder = Path.Combine(addinFolder, "output");
            Directory.CreateDirectory(outputFolder);

            string csvPath = Path.Combine(outputFolder, $"{projectName}.csv");

            var lines = new List<string> { "View Name,View Type,Element ID,Duplicate Count" };
            foreach (var group in duplicateGroups)
            {
                int count = group.Count();
                foreach (View view in group)
                    lines.Add($"\"{view.Name}\",\"{view.ViewType}\",{view.Id},{count}");
            }

            File.WriteAllLines(csvPath, lines);

            TaskDialog.Show("Duplicate Views",
                $"Found {duplicateGroups.Count} duplicate view name(s) across {duplicateGroups.Sum(g => g.Count())} views.\n\nReport saved to:\n{csvPath}");
        }
    }
}