using System;
using Hl7.Fhir.Model;  
using Hl7.Fhir.Rest;

namespace fhirclient_dotnet
{
    public class CompareDemographics
    {
        public string GetDemographicComparison
        (string ServerEndPoint,
         string IdentifierSystem,
         string IdentifierValue,
         string myFamily,
         string myGiven,
         string myGender,
         string myBirthDate
         )
         {
            return "";
         }
    

     private Hl7.Fhir.Model.Patient FHIR_SearchByIdentifier(string ServerEndPoint, string IdentifierSystem,string IdentifierValue)
        {
             Hl7.Fhir.Model.Patient  o=new  Hl7.Fhir.Model.Patient() ;
            var client = new Hl7.Fhir.Rest.FhirClient(ServerEndPoint); 
            Bundle bu = client.SearchAsync<Hl7.Fhir.Model.Patient>(new string[]
             { "identifier=" + IdentifierSystem + "|" + IdentifierValue }).GetAwaiter().GetResult();  
            if (bu.Entry.Count>0)
                    {
                        o=(Hl7.Fhir.Model.Patient)bu.Entry[0].Resource;
                    }
            else
                {o=null;}
            return o;
        }
        private string GetFamily(Hl7.Fhir.Model.Patient p) {
            
            string te = p.Name[0].Family;
            return te;
        }
        private string GetGender(Hl7.Fhir.Model.Patient p) {
            
            string te = p.Gender.ToString();
            return te;
        }
        private string GetBirthDate(Hl7.Fhir.Model.Patient p) {
            
            string te = p.BirthDate.ToString();
            return te;
        }
        
        private string GetGiven(Hl7.Fhir.Model.Patient p) {
            string te="";
            foreach(var g in p.Name[0].Given)
                {
                    te+=g.ToString()+" ";
                }
            return te.TrimEnd();
        }
        
}
}
