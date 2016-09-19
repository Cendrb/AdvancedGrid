using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VitSoft.Framework.BusinessObjects.Common;

namespace AdvancedGrid
{
    /// <summary>
    /// Interaction logic for SmartGrid.xaml
    /// </summary>
    public partial class SmartGrid : UserControl
    {
        public SmartGrid()
        {
            InitializeComponent();
        }

        public abstract class SmartGridInititializer
        {
            /// <summary>
            /// Gets the total number of columns
            /// </summary>
            /// <returns>Total number of columns</returns>
            public abstract int GetNumberOfColumns();
            /// <summary>
            /// Returns column header for a column specified by its ID
            /// </summary>
            /// <param name="columnID">ID of a column</param>
            /// <returns>Column header</returns>
            public abstract string GetColumnHeader(int columnID);
            /// <summary>
            /// Returns stringified
            /// </summary>
            /// <param name="columnID"></param>
            /// <param name="rowObject"></param>
            /// <returns></returns>
            public abstract FormattableSmartObject GetDataCellValue(int columnID, object rowObject);
            public abstract void ParseDataCellValue(int columnID, object rowObject, string columnValue);
        }

        public class FormattableSmartObject
        {
            public event Action<string> FormatedStringChanged = delegate { };

            private readonly ISmartObject smartObject;
            private readonly Func<object, string> formatFunc;
            private readonly Func<string, object, bool> parseFunc;

            public FormattableSmartObject(ISmartObject smartObject, Func<object, string> formatFunc, Func<string, object, bool> parseFunc)
            {
                this.smartObject = smartObject;
                this.formatFunc = formatFunc;
                this.parseFunc = parseFunc;

                smartObject.ValueChanged += smartObject_ValueChanged;
            }

            /// <summary>
            /// Tries to set the value into the data object
            /// </summary>
            /// <param name="changedText">Text after change</param>
            /// <returns>True if text is valid, false if revert to previous value is required</returns>
            public bool GuiValueChanged(string changedText)
            {
                if (parseFunc != null)
                    return parseFunc.Invoke(changedText, smartObject);
                else
                    return true;
            }

            void smartObject_ValueChanged(object sender, EventArgs e)
            {
                FormatedStringChanged.Invoke(formatFunc.Invoke(smartObject));
            }
        }

    }
}
