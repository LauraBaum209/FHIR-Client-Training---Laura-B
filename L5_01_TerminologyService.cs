using Hl7.Fhir.Model; 
using Hl7.Fhir.Rest; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http; 
using System.Net.Http.Headers;

namespace fhirclient_dotnet
{
    public class TerminologyService
    {
        public String ExpandValueSetForCombo(
            string EndPoint,
            string Url,
            string Filter

        )
        {
            return "";
        }
         String GetDataSync(String Server,String Url, String Filter)
         {
             String aux="";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new  MediaTypeWithQualityHeaderValue("application/fhir+json"));
                String CompleteUrl=Server+"/ValueSet/$expand?url="+Url+"&filter="+Filter;
                
                var response = client.GetAsync(CompleteUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content; 

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;

                    aux=responseString;
                }
            }
            return aux;
        }

    }
}
