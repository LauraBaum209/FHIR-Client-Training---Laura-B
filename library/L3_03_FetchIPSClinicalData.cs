using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using System.Threading.Tasks;
using System.Net.Http; 
using System.Net.Http.Headers;
namespace fhirclient_dotnet
{
    public class FetchIPS
    {
        public string GetIPSMedications
        (
                string ServerEndPoint,
                string IdentifierSystem,
                string IdentifierValue
         )
         {
            return "";
         }

        private string GetMedicationDetail(string server,Patient patient)
        {
            String aux="Error:No_IPS";
            var options = new System.Text.Json.JsonSerializerOptions().ForFhir();
            var ReturnedResource= new Hl7.Fhir.Model.Bundle();
            String ips=  GetIPSSync(server,patient.Id);
            
            try
            {
                ReturnedResource = System.Text.Json.JsonSerializer.Deserialize<Bundle>(ips, options);
            }
            catch(Exception ex)
            {
                aux=ex.Message;
            }
        
            if (!(ReturnedResource is null))
            {
                
                Bundle bu = ReturnedResource;  
                if (bu.Entry.Count>0)
                {

                    Bundle OneDoc = bu;
                    if (OneDoc.Entry.Count>0)
                    {
                         aux="";
                         foreach (Bundle.EntryComponent e in OneDoc.Entry)
                            {
                              if (e.Resource.TypeName=="MedicationStatement")
                              {
                                Hl7.Fhir.Model.MedicationStatement  oneP=(Hl7.Fhir.Model.MedicationStatement) e.Resource;
                                string m_status=oneP.Status.ToString();
                                string m_datefo="";
                                if (oneP.Effective!=null)
                                {
                                    if (oneP.Effective.TypeName=="Period")
                                    {
                                        Period per=(Period)oneP.Effective;
                                        m_datefo=per.Start.ToString();
                                    }
                                    else
                                    {
                                        m_datefo=oneP.Effective.ToString();
                                    }
                                }
                                string m_code="";
                                if (oneP.Medication.TypeName=="CodeableConcept")
                                {
                                    CodeableConcept c;
                                    c=(CodeableConcept)oneP.Medication;
                                    m_code=c.Coding[0].Code+":"+c.Coding[0].Display;
                                }
                                else
                                {
                                   m_code=oneP.Medication.TypeName;
                                   if (oneP.Medication.TypeName=="Reference")
                                   {
                                       ResourceReference r=(ResourceReference) oneP.Medication;
                                       foreach(Bundle.EntryComponent em in OneDoc.Entry)
                                       {
                                        
                                           if (em.FullUrl==r.Reference)
                                           {
                                               Medication m = (Medication) em.Resource;
                                               CodeableConcept c=m.Code;
                                               m_code=c.Coding[0].Code+":"+c.Coding[0].Display;
                                               break;
                                           }
                                       }
                                       
                                   } 
                                }
                                  
                                aux+=m_status+"|"+m_datefo+"|"+m_code+"\n";
                              }
                            }
                        if (aux==""){aux="Error:IPS_No_Medications";}
                              
                    }
                }
            }
        
        return aux;
        
        
    }
     public string GetIPSImmunizations
        (string ServerEndPoint,
         string IdentifierSystem,
         string IdentifierValue
         )
         {
            return "";
         }
    
    
private string GetImmunizationDetail(string server,Patient patient)
    {
        string aux="Error:No_IPS";
        string ips= GetIPSSync(server,patient.Id);
        var options = new System.Text.Json.JsonSerializerOptions().ForFhir();
        Hl7.Fhir.Model.Bundle ReturnedResource = System.Text.Json.JsonSerializer.Deserialize<Bundle>(ips, options);
        Hl7.Fhir.Model.Bundle bu=ReturnedResource;
        if (bu.Entry.Count>0)
            {
                    Bundle OneDoc=bu;
            
                    if (OneDoc.Entry.Count>0)
                    {
                         aux="";
                         foreach (Bundle.EntryComponent e in OneDoc.Entry)
                            {
                              if (e.Resource.TypeName=="Immunization")
                              {
                                Hl7.Fhir.Model.Immunization oneP=(Hl7.Fhir.Model.Immunization) e.Resource;
                                string m_status=oneP.Status.ToString();
                                string m_datefo="";
                                if (oneP.Occurrence!=null)
                                {
                                        m_datefo=oneP.Occurrence.ToString();
                                    
                                }
                                string m_code="";
                                
                                CodeableConcept c;
                                c=oneP.VaccineCode;
                                m_code=c.Coding[0].Code+":"+c.Coding[0].Display;
                                  
                                aux+=m_status+"|"+m_datefo+"|"+m_code+"\n";
                              }
                            }
                        if (aux==""){aux="Error:IPS_No_Immunizations";}
                              
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
     private String GetIPSSync(String Server,String PatientId)
         {
             String aux="";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new  MediaTypeWithQualityHeaderValue("application/fhir+json"));
                String CompleteUrl=Server+"/Patient/"+PatientId+"/$summary";
                
                var response = client.GetAsync(CompleteUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content; 
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    aux=responseString;
                }
            }
            return aux;
        }
   }
}
