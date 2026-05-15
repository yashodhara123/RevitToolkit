using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using Revit_ToolKit.Commands;
using System.Configuration.Assemblies;
using System.Reflection;

namespace Revit_ToolKit
{
   
    public class Application : ExternalApplication
    {
        private PushButton _duplicateViews;
        string tabName = "RevitToolKit";
        string panelName = "RevitModelCheck";
        public override void OnStartup()
        {
            RibbonPanel panel = CreateCustomRibbon(Application);
            _duplicateViews = panel.AddItem(new PushButtonData("Check_Duplicate_Views", "Check_Duplicate_Views", Assembly.GetExecutingAssembly().Location, typeof(CheckDuplicateViewsCommand).FullName)) as PushButton;

        }


        private RibbonPanel CreateCustomRibbon(UIControlledApplication app)
        {
            RibbonPanel _panel = null;
            RibbonPanel _pluginmanagerpanel = null;
            // Try to create the tab (will throw if it already exists)
            try
            {
                app.CreateRibbonTab(tabName);
            }
            catch
            {
                // Tab already exists, do nothing
            }

            // Check if panel already exists under this tab
            bool panelExists = false;
            bool PluginManagerpanelExists = false;
            try
            {
                IList<RibbonPanel> panels = app.GetRibbonPanels(tabName);
                foreach (RibbonPanel panel in panels)
                {
                    if (panel.Name.Equals(panelName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        _panel = panel;
                        panelExists = true;
                        //break;
                    }

                   
                }
            }
            catch
            {
                // Tab doesn't exist or some error – ignore since we handle tab creation above
            }

            // Create panel only if it doesn't exist
            if (!panelExists)
            {

                _panel = app.CreateRibbonPanel(tabName, panelName);
            }
          
            return _panel;
        }
    }
}