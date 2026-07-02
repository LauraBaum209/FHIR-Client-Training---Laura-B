using System;
using Hl7.Fhir.Model;  
using Hl7.Fhir.Rest;

namespace fhirclient_dotnet
{
    public class FetchDemographics
    {
        public string GetPatientPhoneAndEmail
        (string ServerEndPoint,
         string IdentifierSystem,
         string IdentifierValue)
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
        private string GetTelecom(Hl7.Fhir.Model.Patient p,string DesiredSystem) {
            string te = "";
            string sys;
            foreach (ContactPoint c in p.Telecom)
            {
                sys = c.System.Value.ToString().ToUpper();
                if (sys == DesiredSystem.ToUpper()) { te += (te==""? "": ",") + c.Value.ToString() + "(" + c.Use.ToString() + ")"; }
            }
            if (te==""){te="-";}
   
            return te;
        }
        
}
}
