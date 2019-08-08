using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AFC.WS.ModelView.ColorSettiing
{
    using AFC.WS.UI.Common;

    public class ModeInvalidColorSetting : IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            
           // throw new NotImplementedException();
        }

        #endregion
    }
}
