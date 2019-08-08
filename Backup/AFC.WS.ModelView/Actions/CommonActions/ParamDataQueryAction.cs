using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.CommonActions
{

    using AFC.WS.UI.Common;

    public class ParamDataQueryAction:IAction
    {

        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            return true;
            //throw newr NotImplementedException();
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            /*
             *  1. from params data 
             *  
             * 2. Create new control
             * 
             * 3. Load RuleFile
             * 
             * 4. Init
             * 
             * 5. Create Entity
             * 
             * 6. Load Data
             * 
             * 7.Read Guize
             * 
             * ShowAction 
             * 
             * */

            return null;
            //throw new NotImplementedException();
        }

        #endregion
    }
}
