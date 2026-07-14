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
            Hl7.Fhir.Model.Patient patient = FHIR_SearchByIdentifier(ServerEndPoint, IdentifierSystem, IdentifierValue);
            if (patient == null)
            {
                return "Error:Patient_Not_Found";
            }

            string serverFamily = GetFamily(patient);
            string serverGiven = GetGiven(patient);
            string serverGender = GetGender(patient);
            string serverBirthDate = GetBirthDate(patient);

            string CompareValue(string expected, string actual)
            {
                return string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase) ? "{green}" : "{red}";
            }

            return "{family}|" + myFamily + "|" + serverFamily + "|" + CompareValue(myFamily, serverFamily) + "\n"
                 + "{given}|" + myGiven + "|" + serverGiven + "|" + CompareValue(myGiven, serverGiven) + "\n"
                 + "{gender}|" + myGender.ToUpperInvariant() + "|" + serverGender.ToUpperInvariant() + "|" + CompareValue(myGender, serverGender) + "\n"
                 + "{birthDate}|" + myBirthDate + "|" + serverBirthDate + "|" + CompareValue(myBirthDate, serverBirthDate) + "\n";
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
