// This module is a demo in C#/.NET core, created to understand different ways to query a FHIR Server
// and retrieve/create Patient Demographic Information and US Core/IPS conformant resources
// You are free to copy code from this demos to create the functions for your assignment submission.
// (copying from here is not mandatory, though)
// We will demo:
// 1) How to get a Patient's full name and address
// 2) How to get a US Core Race extension
// 3) How to read all US Core Condition resources for a patient
// 4) How to create a US Core Allergy conformant resource
// 5) How to read lab results from an IPS document for a patient
// 6) How to expand  a valueset using a terminology server
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace fhirclient_dotnet {
  public class Demo {

    private string GetGiven(Patient p) {
      HumanName name = p.Name[0];
      string first = "";
      foreach(var m in name.Given) {
        first += m + " ";
      }

      return first.TrimEnd();
    }

    private string GetFamily(Patient p) {
      string te = p.Name[0].Family;
      return te.TrimEnd();
    }

    public string GetPatientFullNameAndAddress(
      string server,
      string patientidentifiersystem,
      string patientidentifiervalue
    )

    {
      string auxA = "";
      string auxN = "";

      Patient pa = FHIR_SearchByIdentifier(server, patientidentifiersystem, patientidentifiervalue);
      string aux = "Error:Patient_Not_Found"; //Default Error
      if (pa != null) {
        auxN = GetFamily(pa) + "," + GetGiven(pa);
        var add = pa.Address;
        foreach(var xad in add) {
          string paddr = "";
          var lns = xad.Line;
          foreach(var l in lns) {
            paddr += l.ToString() + " ";
          }
          if (xad.City != null) {
            paddr = paddr + " - " + xad.City;
          }
          if (xad.State != null) {
            paddr = paddr + " , " + xad.State;
          }
          if (xad.Country != null) {
            paddr = paddr + " , " + xad.Country;
          }
          if (xad.PostalCode != null) {
            paddr = paddr + " (" + xad.PostalCode + ")";
          }
          auxA = auxA + paddr + " / ";
        }
        if (auxN == "") {
          auxN = "-";
        }
        if (auxA == "") {
          auxA = "-";
        }
        aux = "Full Name:" + auxN + "\n" + "Address:" + auxA + "\n";
      }

      return aux;
    }

    private Hl7.Fhir.Model.Patient FHIR_SearchByIdentifier(string ServerEndPoint, string IdentifierSystem, string IdentifierValue) {
      Hl7.Fhir.Model.Patient o = new Hl7.Fhir.Model.Patient();
      var client = new Hl7.Fhir.Rest.FhirClient(ServerEndPoint);
      Bundle bu = client.SearchAsync<Hl7.Fhir.Model.Patient>(new string[] {
        "identifier=" + IdentifierSystem + "|" + IdentifierValue
      }).GetAwaiter().GetResult();
      if (bu.Entry.Count > 0) {
        o = (Hl7.Fhir.Model.Patient) bu.Entry[0].Resource;
      } else {
        o = null;
      }
      return o;
    }

    private string GetRaceExtension(Hl7.Fhir.Model.Patient p) {
      string auxt = "";
      string auxo = "";
      string auxd = "";
      string raceExtensionUrl = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-race";
      string aux = "Error:No_us-core-race_Extension";
      System.Collections.Generic.List < Extension > e = p.Extension;
      foreach(Extension ef in e) {
        if (ef.Url == raceExtensionUrl) {
          aux = "";
          foreach(Extension efs in ef.Extension) {
            switch (efs.Url) 
            {
            case "text": {
              auxt = "text|" + efs.Value.ToString() + "\n";
              break;
            }
            case "ombCategory": {
              Coding c = (Coding) efs.Value;
              auxo = "code|" + c.Code + ":" + c.Display + "\n";
              break;
            }
            case "detailed": {
              Coding c = (Coding) efs.Value;
              auxd += "detail|" + c.Code + ":" + c.Display + "\n";
              break;
            }
            }
          }
        }
        aux += auxt;
        aux += auxo;
        aux += auxd;
        break;
      }

    
    if ((auxt == "") || (auxo == "")) {
      aux = "Error:Non_Conformant_us-core-race_Extension";
    }
    return aux;
  }

  public string GetUSCoreRace(
    string server,
    string patientidentifiersystem,
    string patientidentifiervalue
  ) {
    Patient p = FHIR_SearchByIdentifier(server, patientidentifiersystem, patientidentifiervalue);
    string aux = "Error:Patient_Not_Found"; //Default Error
    if (p != null) {
      aux = GetRaceExtension(p);
    }
    return aux;
  }

  public string GetConditions
    (string ServerEndPoint,
      string IdentifierSystem,
      string IdentifierValue
    ) {
      Patient patient = FHIR_SearchByIdentifier(ServerEndPoint, IdentifierSystem, IdentifierValue);
      string aux = "Error:Patient_Not_Found"; //Default Error

      if (patient != null) {
        aux = GetDetail(ServerEndPoint, patient);
      }
      return aux;
    }

  private string GetDetail(string server, Patient patient) {
    string aux = "Error:No_Conditions";
    Hl7.Fhir.Model.Condition p = new Hl7.Fhir.Model.Condition();
    var client = new Hl7.Fhir.Rest.FhirClient(server);
    Bundle bu = client.SearchAsync<Hl7.Fhir.Model.Condition>(new string[] {
      "patient=" + patient.Id
    }).GetAwaiter().GetResult();
    if (bu.Entry.Count > 0) {
      aux = "";
      foreach(Bundle.EntryComponent e in bu.Entry) {
        Hl7.Fhir.Model.Condition oneP = (Hl7.Fhir.Model.Condition) e.Resource;

        string cStatus = oneP.ClinicalStatus.Coding[0].Display;
        string cVerification = oneP.VerificationStatus.Coding[0].Display;
        string cCategory = oneP.Category[0].Coding[0].Display;
        string cCode = oneP.Code.Coding[0].Code + ":" + oneP.Code.Coding[0].Display;
        aux += cStatus + "|" + cVerification + "|" + cCategory + "|" + cCode + "\n";
      }

    }
    return aux;
  }
  public string CreateUSCoreAllergyIntolerance(
    string server,
    string patientidentifiersystem,
    string patientidentifiervalue,
    string ClinicalStatusCode,
    string VerificationStatusCode,
    string CategoryCode,
    string CriticalityCode,
    string AllergySnomedCode,
    string AllergySnomedDisplay,
    string ManifestationSnomedCode,
    string ManifestationSnomedDisplay,
    string ManifestationSeverityCode
  ) {

    Patient patient = FHIR_SearchByIdentifier(server, patientidentifiersystem, patientidentifiervalue);
    string aux = "Error:Patient_Not_Found"; //Default Error

    if (patient != null) {

      AllergyIntolerance Allergy = new AllergyIntolerance() {
        Meta = new Meta() {
            VersionId = "1",
              LastUpdated = DateTime.Today,
              Profile = new List < string > {
                "http://hl7.org/fhir/us/core/StructureDefinition/us-core-allergyintolerance"
              }
          },
          Text = new Narrative() {
            Status = Narrative.NarrativeStatus.Generated,
              Div = "<div xmlns=\"http://www.w3.org/1999/xhtml\"><p>Redacted Text</p></div>"
          },
          //  Type=AllergyIntoleranceType.Allergy,
          ClinicalStatus = new CodeableConcept() {
            Coding = new List < Coding > () {
              new Coding {
                System = "http://terminology.hl7.org/CodeSystem/allergyintolerance-clinical", Code = ClinicalStatusCode, Display = ClinicalStatusCode
              }
            },
          },

          VerificationStatus =
          new CodeableConcept() {
            Coding = new List < Coding > () {
                new Coding {
                  System = "http://terminology.hl7.org/CodeSystem/allergyintolerance-verification", Code = VerificationStatusCode, Display = VerificationStatusCode
                }
              },

          },
          Code = new CodeableConcept() {
            Coding = new List < Coding > () {
                new Coding() {
                  System = "http://snomed.info/sct", Code = AllergySnomedCode, Display = AllergySnomedDisplay
                }
              },
              Text = AllergySnomedDisplay
          },
          Patient = new ResourceReference() {
            Reference = $"Patient/{patient.Id}",
              Display = patient.Name[0].Family.ToString() + "," + GetGiven(patient)
          },
          Reaction = new List < AllergyIntolerance.ReactionComponent > () {

          }
      };

      AllergyIntolerance.ReactionComponent arc = new AllergyIntolerance.ReactionComponent();
      arc.Manifestation.Add(
        new CodeableConcept() {
          Coding = new List < Coding > () {
            new Coding {
              System = "http://snomed.info/sct", Code = ManifestationSnomedCode, Display = ManifestationSnomedDisplay
            }
          }
        }
      );
      Allergy.Reaction.Add(arc);

      aux = System.Text.Json.JsonSerializer.Serialize(Allergy, new System.Text.Json.JsonSerializerOptions().ForFhir());

    }
    return aux;

  }

  public string GetIPSLabResults
    (string ServerEndPoint,
      string IdentifierSystem,
      string IdentifierValue
    ) {
      Patient patient = FHIR_SearchByIdentifier(ServerEndPoint, IdentifierSystem, IdentifierValue);
      string aux = "Error:Patient_Not_Found"; //Default Error

      if (patient != null) {
        aux = GetIPSLabResultsDetail(ServerEndPoint, patient);
      }
      return aux;
    }

  private string GetIPSLabResultsDetail(string server, Patient patient) {
    string aux = "Error:No_IPS";

    var options = new System.Text.Json.JsonSerializerOptions().ForFhir().UsingMode(Hl7.Fhir.Serialization.DeserializationMode.Ostrich);
    Hl7.Fhir.Model.Bundle ReturnedResource = null;
    string ips = GetIPSSync(server, patient.Id);

    try {
      ReturnedResource = System.Text.Json.JsonSerializer.Deserialize<Bundle>(ips, options);
    } catch (Exception ex) {
      aux = ex.Message;
    }

    if (ReturnedResource != null) {
      Bundle bu = ReturnedResource;
      if (bu.Entry.Count > 0) {
        Bundle OneDoc = bu;
        aux = "";
        foreach(Bundle.EntryComponent e in OneDoc.Entry) {
          if (e.Resource.TypeName == "Observation") {
            Hl7.Fhir.Model.Observation oneP = (Hl7.Fhir.Model.Observation) e.Resource;
            string m_category = oneP.Category[0].Coding[0].Code.ToLower();
            if (m_category == "laboratory") {
              // Must support for IPS Laboratory
              string m_code = "";
              CodeableConcept c = oneP.Code;
              string u_code = "";
              if (c.Coding.Count > 0) {
                u_code = c.Coding[0].Code;
              }
              // This is null for the grouper
              // we only want the observations with results
              if (u_code != "") {
                m_code = c.Coding[0].Code + ":" + c.Coding[0].Display;
              }
              if (m_code != "") {
                string m_result = "";
                if (oneP.Value.TypeName == "Quantity") {
                  Quantity m_Q = (Quantity) oneP.Value;
                  m_result = m_Q.Value + " " + m_Q.Unit;
                }
                if (oneP.Value.TypeName == "String") {
                  m_result = oneP.Value.ToString();
                }
                if (oneP.Value.TypeName == "CodeableConcept") {
                  CodeableConcept m_C = (CodeableConcept) oneP.Value;
                  m_result = m_C.Coding[0].Code + ":" + m_C.Coding[0].Display;
                }
                string m_status = oneP.Status.ToString();
                string m_datefo = oneP.Effective.ToString();

                aux += m_code + "|" + m_datefo + "|" + m_status + "|" + m_result + "\n";
              }
            }
          }
        }
      }
    }
    if (aux == "") {
      aux = "Error:IPS_No_Lab_Results";
    }
    return aux;
  }

  private String GetIPSSync(String Server, String PatientId) {
    String aux = "";
    using(var client = new HttpClient()) {
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/fhir+json"));
      String CompleteUrl = Server + "/Patient/" + PatientId + "/$summary";

      var response = client.GetAsync(CompleteUrl).Result;

      if (response.IsSuccessStatusCode) {
        var responseContent = response.Content;
        string responseString = responseContent.ReadAsStringAsync().Result;
        aux = responseString;
      }
    }
    return aux;
  }

  public String ExpandValueSetForCombo(
    string EndPoint,
    string Url,
    string Filter

  ) {

    string aux = "";
    ValueSet Result = null;
    try {

      var parser = new Hl7.Fhir.Serialization.FhirJsonParser();
      String Response = GetDataSync(EndPoint, Url, Filter);
      try {
        Result = parser.Parse < ValueSet > (Response);
      } catch (Exception ex) {
        aux = ex.Message;
      }

      aux = "";
      if (Result != null) {
        foreach(ValueSet.ContainsComponent ec in Result.Expansion.Contains) {
          aux += ec.Code + "|" + ec.Display + "\n";
        }
        if (aux == "") {
          aux = "Error:ValueSet_Filter_Not_Found";
        }

      }
    } catch (Exception ex) {
      aux = ex.ToString();
    }
    return aux;
  }

  String GetDataSync(String Server, String Url, String Filter) {
    String aux = "";
    using(var client = new HttpClient()) {
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/fhir+json"));
      String CompleteUrl = Server + "/ValueSet/$expand?url=" + Url + "&filter=" + Filter;

      var response = client.GetAsync(CompleteUrl).Result;

      if (response.IsSuccessStatusCode) {
        var responseContent = response.Content;

        // by calling .Result you are synchronously reading the result
        string responseString = responseContent.ReadAsStringAsync().Result;

        aux = responseString;
      }
    }
    return aux;
  }

}
}
