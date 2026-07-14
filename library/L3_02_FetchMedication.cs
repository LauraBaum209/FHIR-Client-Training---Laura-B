using System;
using Hl7.Fhir.Model;  
using Hl7.Fhir.Rest;

namespace fhirclient_dotnet
{
    public class FetchMedication
    {
        public string GetMedications
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
        string aux="Error:No_Medications";
        Hl7.Fhir.Model.MedicationRequest  p=new  Hl7.Fhir.Model.MedicationRequest() ;
            var client = new Hl7.Fhir.Rest.FhirClient(server); 
            Bundle bu = client.SearchAsync<Hl7.Fhir.Model.MedicationRequest>(new string[]
             { "patient=" + patient.Id }).GetAwaiter().GetResult();  
            if (bu.Entry.Count>0)
                    {
                        aux="";
                         foreach (Bundle.EntryComponent e in bu.Entry)
                            {
                              Hl7.Fhir.Model.MedicationRequest  oneP=(Hl7.Fhir.Model.MedicationRequest) e.Resource;
                              string m_status=oneP.Status.ToString();
                              string m_intent=oneP.Intent.ToString();
                              string m_datefo=oneP.AuthoredOn.ToString();
                              CodeableConcept c;
                              c=(CodeableConcept)oneP.Medication;
                              
                              string m_code=c.Coding[0].Code+":"+c.Coding[0].Display;
                              string m_pres=oneP.Requester.Display.ToString();
                              if (m_pres=="")
                              {
                                  m_pres=oneP.Requester.Reference.ToString();
                              }

                              aux+=m_status+"|"+m_intent+"|"+m_datefo+"|"+m_code+"|"+m_pres+"\n";
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
