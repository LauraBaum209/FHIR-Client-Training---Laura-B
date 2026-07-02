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
            Hl7.Fhir.Model.Patient patient = FHIR_SearchByIdentifier(ServerEndPoint, IdentifierSystem, IdentifierValue);
            if (patient == null)
            {
                return "Error:Patient_Not_Found";
            }

            var emails = new List<string>();
            var phones = new List<string>();

            if (patient.Telecom != null)
            {
                foreach (ContactPoint telecom in patient.Telecom)
                {
                    if (telecom == null || telecom.System == null || telecom.Value == null)
                    {
                        continue;
                    }

                    string system = telecom.System.ToString()?.ToLowerInvariant();
                    string use = telecom.Use != null ? telecom.Use.ToString() : string.Empty;
                    string entry = telecom.Value + (string.IsNullOrEmpty(use) ? string.Empty : "(" + use + ")");

                    if (system == "email")
                    {
                        emails.Add(entry);
                    }
                    else if (system == "phone")
                    {
                        phones.Add(entry);
                    }
                }
            }

            string emailText = emails.Count > 0 ? string.Join(",", emails) : "-";
            string phoneText = phones.Count > 0 ? string.Join(",", phones) : "-";

            return "Emails:" + emailText + "\nPhones:" + phoneText + "\n";
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
