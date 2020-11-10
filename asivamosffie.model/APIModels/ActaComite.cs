using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class HTMLContent
     {
         public string HTML { get; set; }
         public HTMLContent(string sHTML)
         {
             HTML = sHTML;
         }
     }

    public class ActaComite
    {

        public StringBuilder htmlContent { get; set; }
    }
}
