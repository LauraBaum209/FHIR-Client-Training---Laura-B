using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

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
            var patient = FHIR_SearchByIdentifier(ServerEndpoint, PatientIdentifierSystem, PatientIdentifierValue);
            if (patient == null)
            {
                return "Error:Patient_Not_Found";
            }

            var body = new Dictionary<string, object>
            {
                ["resourceType"] = "Immunization",
                ["meta"] = new Dictionary<string, object>
                {
                    ["profile"] = new[] { "http://hl7.org/fhir/us/core/StructureDefinition/us-core-immunization" }
                },
                ["status"] = string.IsNullOrWhiteSpace(ImmunizationStatusCode) ? "completed" : ImmunizationStatusCode,
                ["patient"] = new Dictionary<string, object>
                {
                    ["reference"] = "Patient/" + patient.Id
                },
                ["occurrenceDateTime"] = ImmunizationDateTime,
                ["vaccineCode"] = new Dictionary<string, object>
                {
                    ["coding"] = new[]
                    {
                        new Dictionary<string, object>
                        {
                            ["system"] = "http://hl7.org/fhir/sid/cvx",
                            ["code"] = ProductCVXCode,
                            ["display"] = ProductCVXDisplay
                        }
                    },
                    ["text"] = ProductCVXDisplay
                }
            };

            if (!string.IsNullOrWhiteSpace(ReasonCode))
            {
                body["reasonCode"] = new[]
                {
                    new Dictionary<string, object>
                    {
                        ["coding"] = new[]
                        {
                            new Dictionary<string, object>
                            {
                                ["system"] = "http://snomed.info/sct",
                                ["code"] = ReasonCode,
                                ["display"] = ReasonCode
                            }
                        },
                        ["text"] = ReasonCode
                    }
                };
            }

            return JsonSerializer.Serialize(body, new JsonSerializerOptions { WriteIndented = false });
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
