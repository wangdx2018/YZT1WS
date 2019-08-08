using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.ColorSettiing
{
    using AFC.WS.UI.Common;
    

    public class ParamsEditColorSetting:IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
           
                
            string editionType = dr["EDITON_TYPE"].ToString();
            
          
            try
            {
                switch (editionType)
                {
                    case "当前版本":
                        dgr.Background = System.Windows.Media.Brushes.Green;
                        dgr.ToolTip = "当前版本";
                        break;
                    case "将来版本":
                        dgr.Background = System.Windows.Media.Brushes.Yellow;
                        dgr.ToolTip = "将来版本";
                        break;
                    case "历史版本":
                        dgr.Background = System.Windows.Media.Brushes.Red;
                        dgr.ToolTip = "历史版本";
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            //throw new NotImplementedException();
        }

        #endregion
    }


    public class ParamsDevFullEditColorSetting : IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            

            string editionType = dr["EDITION_TYPE"].ToString();


            try
            {
                switch (editionType)
                {
                    case "当前版本":
                        dgr.Background = System.Windows.Media.Brushes.Green;
                        dgr.ToolTip = "当前版本";
                        break;
                    case "将来版本":
                        dgr.Background = System.Windows.Media.Brushes.Yellow;
                        dgr.ToolTip = "将来版本";
                        break;
                    case "历史版本":
                        dgr.Background = System.Windows.Media.Brushes.Red;
                        dgr.ToolTip = "历史版本";
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            //throw new NotImplementedException();
        }

        #endregion
    }
}
