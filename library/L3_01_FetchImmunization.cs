using System;
using Hl7.Fhir.Model;  
using Hl7.Fhir.Rest;

namespace fhirclient_dotnet
{
    public class FetchImmunization
    {
        public string GetImmunizations
        (string ServerEndPoint,
         string IdentifierSystem,
         string IdentifierValue
         )
         {
            Hl7.Fhir.Model.Patient patient = FHIR_SearchByIdentifier(ServerEndPoint, IdentifierSystem, IdentifierValue);
            if (patient == null)
            {
                return "Error:Patient_Not_Found";
            }

            return GetDetail(ServerEndPoint, patient);
         }
    
    private string GetDetail(string server,Patient patient)
    {
        string aux="Error:No_Immunizations";
        Hl7.Fhir.Model.Immunization  p=new  Hl7.Fhir.Model.Immunization() ;
            var client = new Hl7.Fhir.Rest.FhirClient(server); 
            Bundle bu = client.SearchAsync<Hl7.Fhir.Model.Immunization>(new string[]
             { "patient=" + patient.Id }).GetAwaiter().GetResult();  
            if (bu.Entry.Count>0)
                    {
                        aux="";
                         foreach (Bundle.EntryComponent e in bu.Entry)
                            {
                              Hl7.Fhir.Model.Immunization  oneP=(Hl7.Fhir.Model.Immunization) e.Resource;
                              string istatus=oneP.Status.ToString();
                              string icode=oneP.VaccineCode.Coding[0].Code+":"+oneP.VaccineCode.Coding[0].Display;
                              string idate=oneP.Occurrence.ToString();
                              aux+=istatus+"|"+icode+"|"+idate+"\n";
                            }
                              
                    }
        return aux;
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
        
    }
}
