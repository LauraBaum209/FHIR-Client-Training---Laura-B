using System;
using Hl7.Fhir.Model;  
using Hl7.Fhir.Rest;

namespace fhirclient_dotnet
{
    public class GetProvidersNearPatient
    {
        public string GetProvidersNearCity
        (string ServerEndPoint,
         string IdentifierSystem,
         string IdentifierValue
         )
         {
            return "";
         }
    
     private string GetProvidersByCity(string server,string city)
    {
        string aux="Error:No_Provider_In_Patient_City";
        Hl7.Fhir.Model.Practitioner  p=new  Hl7.Fhir.Model.Practitioner() ;
            var client = new Hl7.Fhir.Rest.FhirClient(server); 
            Bundle bu = client.SearchAsync<Hl7.Fhir.Model.Practitioner>(new string[]
             { "address-city=" + city }).GetAwaiter().GetResult();  
            if (bu.Entry.Count>0)
                    {
                        aux="";
                         foreach (Bundle.EntryComponent e in bu.Entry)
                            {
                              Hl7.Fhir.Model.Practitioner  oneP=(Hl7.Fhir.Model.Practitioner) e.Resource;
                              HumanName name=oneP.Name[0];
                              string first="";
                              foreach(var m in name.Given)
                              {
                                  first+=m+" ";
                              }
                              string paddr="";
                              string pname=oneP.Name[0].Family.ToString()+","+first;
                              string ptele=oneP.Telecom[0].System.ToString()+":"+oneP.Telecom[0].Value.ToString();
                              var a = oneP.Address[0];
                              if (a.City==city)
                              {   
                                 foreach(var l in a.Line)
                                 {
                                  paddr+=l+" ";
                                 }
                              }
                              paddr=paddr.TrimEnd();
                              pname=pname.TrimEnd();
                              string pqual=oneP.Qualification[0].Code.Coding[0].Display.ToString();
                              aux+=pname+"|"+ptele+"|"+paddr+"|"+pqual+"\n";
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
