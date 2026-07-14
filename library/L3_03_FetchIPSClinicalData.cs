using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace fhirclient_dotnet
{
    public class FetchIPS
    {
        public string GetIPSMedications(string ServerEndPoint, string IdentifierSystem, string IdentifierValue)
        {
            Patient patient = FHIR_SearchByIdentifier(ServerEndPoint, IdentifierSystem, IdentifierValue);
            if (patient == null)
            {
                return "Error:Patient_Not_Found";
            }

            return GetMedicationDetail(ServerEndPoint, patient);
        }

        private string GetMedicationDetail(string server, Patient patient)
        {
            string aux = "Error:No_IPS";
            string ips = GetIPSSync(server, patient.Id);

            if (string.IsNullOrWhiteSpace(ips))
            {
                return aux;
            }

            try
            {
                var entries = ParseBundleEntries(ips);
                var medicationsByReference = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (var entry in entries)
                {
                    if (entry.ResourceType != "Medication")
                    {
                        continue;
                    }

                    var code = GetMedicationResourceCode(entry.Resource);
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        if (!string.IsNullOrWhiteSpace(entry.FullUrl))
                        {
                            medicationsByReference[entry.FullUrl] = code;
                        }

                        var id = entry.Resource.TryGetProperty("id", out var idElement) ? idElement.GetString() : null;
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            medicationsByReference[id] = code;
                        }
                    }
                }

                aux = "";
                foreach (var entry in entries)
                {
                    if (entry.ResourceType != "MedicationStatement")
                    {
                        continue;
                    }

                    string status = NormalizeStatus(GetString(entry.Resource, "status"));
                    string date = GetMedicationDate(entry.Resource);
                    string code = GetMedicationCode(entry.Resource, medicationsByReference);
                    aux += status + "|" + date + "|" + code + "\n";
                }

                if (string.IsNullOrWhiteSpace(aux))
                {
                    aux = "Unknown||no-medication-info:No information about medications\n";
                }
            }
            catch (Exception)
            {
                aux = "Error:No_IPS";
            }

            return aux;
        }

        public string GetIPSImmunizations(string ServerEndPoint, string IdentifierSystem, string IdentifierValue)
        {
            Patient patient = FHIR_SearchByIdentifier(ServerEndPoint, IdentifierSystem, IdentifierValue);
            if (patient == null)
            {
                return "Error:Patient_Not_Found";
            }

            return GetImmunizationDetail(ServerEndPoint, patient);
        }

        private string GetImmunizationDetail(string server, Patient patient)
        {
            string aux = "Error:No_IPS";
            string ips = GetIPSSync(server, patient.Id);

            if (string.IsNullOrWhiteSpace(ips))
            {
                return aux;
            }

            try
            {
                var entries = ParseBundleEntries(ips);
                aux = "";

                foreach (var entry in entries)
                {
                    if (entry.ResourceType != "Immunization")
                    {
                        continue;
                    }

                    string status = NormalizeStatus(GetString(entry.Resource, "status"));
                    string date = GetImmunizationDate(entry.Resource);
                    string code = GetVaccineCode(entry.Resource);
                    aux += status + "|" + date + "|" + code + "\n";
                }

                if (string.IsNullOrWhiteSpace(aux))
                {
                    aux = "Error:IPS_No_Immunizations";
                }
            }
            catch (Exception)
            {
                aux = "Error:IPS_No_Immunizations";
            }

            return aux;
        }

        private Patient FHIR_SearchByIdentifier(string ServerEndPoint, string IdentifierSystem, string IdentifierValue)
        {
            Patient o = new Patient();
            var client = new FhirClient(ServerEndPoint);
            Bundle bu = client.SearchAsync<Patient>(new string[] { "identifier=" + IdentifierSystem + "|" + IdentifierValue }).GetAwaiter().GetResult();
            if (bu.Entry.Count > 0)
            {
                o = (Patient)bu.Entry[0].Resource;
            }
            else
            {
                o = null;
            }

            return o;
        }

        private string GetIPSSync(string Server, string PatientId)
        {
            string aux = string.Empty;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/fhir+json"));
                string CompleteUrl = Server + "/Patient/" + PatientId + "/$summary";

                var response = client.GetAsync(CompleteUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    aux = response.Content.ReadAsStringAsync().Result;
                }
            }

            return aux;
        }

        private static List<ParsedBundleEntry> ParseBundleEntries(string json)
        {
            using var document = JsonDocument.Parse(json);
            var entries = new List<ParsedBundleEntry>();

            if (document.RootElement.TryGetProperty("entry", out var entryArray))
            {
                foreach (var entry in entryArray.EnumerateArray())
                {
                    if (!entry.TryGetProperty("resource", out var resource))
                    {
                        continue;
                    }

                    string resourceType = string.Empty;
                    if (resource.TryGetProperty("resourceType", out var resourceTypeElement))
                    {
                        resourceType = resourceTypeElement.GetString() ?? string.Empty;
                    }

                    string fullUrl = string.Empty;
                    if (entry.TryGetProperty("fullUrl", out var fullUrlElement))
                    {
                        fullUrl = fullUrlElement.GetString() ?? string.Empty;
                    }

                    entries.Add(new ParsedBundleEntry(resourceType, fullUrl, resource));
                }
            }

            return entries;
        }

        private static string GetMedicationCode(JsonElement resource, Dictionary<string, string> medicationsByReference)
        {
            if (resource.TryGetProperty("medication", out var medication))
            {
                if (medication.ValueKind == JsonValueKind.Object)
                {
                    if (medication.TryGetProperty("coding", out var codingArray) && codingArray.ValueKind == JsonValueKind.Array && codingArray.GetArrayLength() > 0)
                    {
                        var code = GetString(codingArray[0], "code");
                        var display = GetString(codingArray[0], "display");
                        if (!string.IsNullOrWhiteSpace(code) || !string.IsNullOrWhiteSpace(display))
                        {
                            return (code ?? "") + ":" + (display ?? "");
                        }
                    }

                    if (medication.TryGetProperty("reference", out var referenceElement))
                    {
                        var reference = referenceElement.GetString() ?? string.Empty;
                        if (medicationsByReference.TryGetValue(reference, out var knownCode))
                        {
                            return knownCode;
                        }
                    }
                }
                else if (medication.ValueKind == JsonValueKind.String)
                {
                    return medication.GetString() ?? string.Empty;
                }
            }

            if (resource.TryGetProperty("medicationCodeableConcept", out var medCodeableConcept))
            {
                var code = GetString(medCodeableConcept, "code");
                var display = GetString(medCodeableConcept, "display");
                if (!string.IsNullOrWhiteSpace(code) || !string.IsNullOrWhiteSpace(display))
                {
                    return (code ?? "") + ":" + (display ?? "");
                }
            }

            return string.Empty;
        }

        private static string GetMedicationResourceCode(JsonElement resource)
        {
            if (resource.TryGetProperty("code", out var codeProperty))
            {
                var code = GetString(codeProperty, "code");
                var display = GetString(codeProperty, "display");
                if (!string.IsNullOrWhiteSpace(code) || !string.IsNullOrWhiteSpace(display))
                {
                    return (code ?? "") + ":" + (display ?? "");
                }
            }

            if (resource.TryGetProperty("coding", out var codingArray) && codingArray.ValueKind == JsonValueKind.Array && codingArray.GetArrayLength() > 0)
            {
                var code = GetString(codingArray[0], "code");
                var display = GetString(codingArray[0], "display");
                return (code ?? "") + ":" + (display ?? "");
            }

            return string.Empty;
        }

        private static string GetMedicationDate(JsonElement resource)
        {
            if (resource.TryGetProperty("effectiveDateTime", out var effectiveDateTime))
            {
                return NormalizeDate(effectiveDateTime.GetString());
            }

            if (resource.TryGetProperty("effectivePeriod", out var effectivePeriod) && effectivePeriod.ValueKind == JsonValueKind.Object)
            {
                return NormalizeDate(GetString(effectivePeriod, "start"));
            }

            return string.Empty;
        }

        private static string GetImmunizationDate(JsonElement resource)
        {
            if (resource.TryGetProperty("occurrenceDateTime", out var occurrenceDateTime))
            {
                return occurrenceDateTime.GetString() ?? string.Empty;
            }

            if (resource.TryGetProperty("occurrenceString", out var occurrenceString))
            {
                return occurrenceString.GetString() ?? string.Empty;
            }

            return string.Empty;
        }

        private static string GetVaccineCode(JsonElement resource)
        {
            if (resource.TryGetProperty("vaccineCode", out var vaccineCode) && vaccineCode.ValueKind == JsonValueKind.Object)
            {
                if (vaccineCode.TryGetProperty("coding", out var codingArray) && codingArray.ValueKind == JsonValueKind.Array && codingArray.GetArrayLength() > 0)
                {
                    var code = GetString(codingArray[0], "code");
                    var display = GetString(codingArray[0], "display");
                    return (code ?? "") + ":" + (display ?? "");
                }

                return GetString(vaccineCode, "text") ?? string.Empty;
            }

            return string.Empty;
        }

        private static string NormalizeDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            if (DateTime.TryParse(value, out var date))
            {
                return date.ToString("yyyy-MM");
            }

            return value;
        }

        private static string NormalizeStatus(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "Unknown";
            }

            return value.Trim().ToLowerInvariant() switch
            {
                "active" => "Active",
                "completed" => "Completed",
                "unknown" => "Unknown",
                _ => value
            };
        }

        private static string GetString(JsonElement element, string propertyName)
        {
            if (element.ValueKind != JsonValueKind.Object || !element.TryGetProperty(propertyName, out var property))
            {
                return null;
            }

            return property.ValueKind == JsonValueKind.String ? property.GetString() : property.ToString();
        }

        private sealed class ParsedBundleEntry
        {
            public ParsedBundleEntry(string resourceType, string fullUrl, JsonElement resource)
            {
                ResourceType = resourceType;
                FullUrl = fullUrl;
                Resource = resource;
            }

            public string ResourceType { get; }
            public string FullUrl { get; }
            public JsonElement Resource { get; }
        }
    }
}
