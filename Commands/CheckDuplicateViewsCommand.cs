using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;

namespace Revit_ToolKit.Commands
{
    /// <summary>
    ///     External command entry point
    /// </summary>
   
    [Transaction(TransactionMode.Manual)]
    public class CheckDuplicateViewsCommand : ExternalCommand
    {
        public override void Execute()
        {
        }
    }
}