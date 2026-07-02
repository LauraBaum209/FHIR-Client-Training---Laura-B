using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace fhirclient_dotnet
{
    public class CreateUSCoreImm
    {
        public string CreateUSCoreR4Immunization
        (string ServerEndpoint,
         string PatientIdentifierSystem,
         string PatientIdentifierValue,
         string ImmunizationStatusCode,
         string ImmunizationDateTime,
         string ProductCVXCode,
         string ProductCVXDisplay,
         string ReasonCode)
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
