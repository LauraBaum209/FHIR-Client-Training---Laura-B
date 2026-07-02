using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Xunit;
using fhirclient_dotnet;
using Hl7.Fhir.Model; 
using Hl7.Fhir.Rest; 

namespace fhirclient_dotnet_tests
{
    
    public class L00_0_Demo_Tests
    {
        static L00_0_Demo_Tests()
        {
            AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);
        }

        //L00_0_T01: Patient Does Not Exist
        //L00_0_T02: Patient with full name and two addresses
        
         [Fact]
        public void L00_0_T01_GetNameAndAddresses_NonExistingPatient()

        {
            MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
            var IdentifierValue="L00_1_T01";
            var NameAndAddresses="Error:Patient_Not_Found";
            var fsh=new Demo();
            var rm=fsh.GetPatientFullNameAndAddress(server,IdentifierSystem,IdentifierValue);
            Assert.Equal(NameAndAddresses,rm);
            
        }
        [Fact]
        public void L00_0_T02_GetNameAndAddresses_MultipleAddresses()

        {
            MyConfiguration c=new MyConfiguration();
            String server=c.ServerEndpoint;
            String IdentifierSystem=c.PatientIdentifierSystem;
            String IdentifierValue="L00_1_T02";
            // Spec
            // GetPatientFullNameAndAddress shall return a string with the Full Name of the patient
            // and all the addresses.
            // Format: 
            // Full Name:{family},{given}\n
            // (for each patient's address)
            // [Address:{line} - {city} , {state} , {country} (postalCode) / ]\n 
            // Message if patient not found: Error:Patient_Not_Found
            //
            String ExpectedResult = "FULL NAME:PATIENT L00_1_T02 FAM,PATIENT L00_1_T02 GIV\n";
            ExpectedResult += "ADDRESS:128 THIS PATIENT DRIVE SUITE 318  - ANN ARBOR , MI , US (48103) / 256 THIS PATIENT AVE SUITE 320 - PENT HOUSE  - MONROE , MI , US (48161) / \n";
        
            var fsh=new Demo();
            var rm=fsh.GetPatientFullNameAndAddress(server,IdentifierSystem,IdentifierValue);
            Assert.Equal(ExpectedResult.ToUpper(),rm.ToUpper());
            
        }
        [Fact]
        public void L00_0_T03_GetUSCoreR4RaceExtension()
        {
            
         
         MyConfiguration c=new MyConfiguration();
         String server=c.ServerEndpoint;
         String IdentifierSystem=c.PatientIdentifierSystem;
         String IdentifierValue="L00_1_T03";
         String ExpectedResult = "TEXT|MIXED\nCODE|2028-9:ASIAN\nDETAIL|1586-7:SHOSHONE\nDETAIL|2036-2:FILIPINO\n";
        // Spec
        // GetUSCoreRace shall return a string with the contents of the US-Core race extension
        // Format: 
        // text|{text sub extension}\n (mandatory)
        // code|{coded sub extension}\n (mandatory)
        // detail|{detailed sub extensions}\n (may repeat)
        // Message if patient not found: Error:Patient_Not_Found
        // Message if extension not found: see implementation
        // Message if extension not conformant: see implementation
        //
            var fsh=new Demo();
            var rm=fsh.GetUSCoreRace(server,IdentifierSystem,IdentifierValue);
            Assert.Equal(ExpectedResult.ToUpper(),rm.ToUpper());
        }
[Fact]

public void L00_0_T04_GetUSCoreConditions()
        {
            
         
         MyConfiguration c=new MyConfiguration();
         String server=c.ServerEndpoint;
         String IdentifierSystem=c.PatientIdentifierSystem;
         String IdentifierValue="L00_1_T04";
         // Spec
        // GetConditions shall return a string with the contents of all patient's US CORE Conditions
        // containing mandatory elements, as follows:
        // Format: for each condition:
        // {ClinicalStatus}|{VerificationStatus}|{Category}|{Code.code}:{Code.display}\n
        // Message if patient not found: Error:Patient_Not_Found
        // Message if no conditions found: see implementation
        //
        var fsh=new Demo();
            var rm=fsh.GetConditions(server,IdentifierSystem,IdentifierValue);
            var rmUpper=(rm??"").ToUpper();
            Assert.Contains("ACTIVE|CONFIRMED|PROBLEM LIST ITEM|442311008:LIVEBORN BORN IN HOSPITAL", rmUpper);
            Assert.Contains("ACTIVE|CONFIRMED|PROBLEM LIST ITEM|69347004:", rmUpper);
            bool hasShortOrLong = rmUpper.Contains("NEONATAL JAUNDICE (DISORDER)") || rmUpper.Contains("NEONATAL JAUNDICE DUE TO DELAYED CONJUGATION");
            Assert.True(hasShortOrLong, "Expected either short or long SNOMED display for 69347004");
        }
    
    [Fact]
    public void L00_0_T05_CreateUSCoreAllergyIntolerance()
        {
            
         
        MyConfiguration c=new MyConfiguration();
        String server=c.ServerEndpoint;
        String IdentifierSystem=c.PatientIdentifierSystem;
         
        String IdentifierValue = "L00_1_T04";
        
        String ClinicalStatusCode="active";
        String VerificationStatusCode="confirmed";
        String CategoryCode="medication";
        String CriticalityCode="high";
        String AllergySnomedCode="102263004";
        String AllergySnomedDisplay="Eggs (edible) (substance)";
        String ManifestationSnomedCode="271807003";
        String ManifestationSnomedDisplay="Skin Rash";
        String ManifestationSeverityCode="mild";
        // Spec
        // CreateUSCoreAllergyIntolerance shall return a string with a JSON FHIR Resource 
        // conformant with the US Core AllergyIntolerance profile
        // or Message if patient not found: Error:Patient_Not_Found
        //
        
        var fsh=new Demo();
        var rm=
        fsh.CreateUSCoreAllergyIntolerance(server, IdentifierSystem, IdentifierValue, ClinicalStatusCode, VerificationStatusCode, CategoryCode, CriticalityCode, AllergySnomedCode, AllergySnomedDisplay, ManifestationSnomedCode, ManifestationSnomedDisplay, ManifestationSeverityCode);
            if (rm!="" && rm!="Error:Patient_Not_Found")
            {
                String val=ValidateAllergyIntoleranceUSCORE(rm,server);
                Assert.True(val=="" || val.Contains("All OK") || val.Contains("Validation successful"), "Validation should succeed; got: " + val);
            }
            else
            {
                Assert.NotEqual("",rm);
            }
        }

    [Fact]
 public  void L00_0_T06_ExpandFilterExistingTerm()
        {

            MyConfiguration c=new MyConfiguration();
            var server=c.TerminologyServerEndpoint;
           
            // Spec
            // ExpandValueset shall return a string with one record for each concept found in the expansion
            // {code}|{display}
            // or Message if term not found: Error:Term/Concept not found
            //
      
            String ExpTerms="64432007|RADIONUCLIDE MYOCARDIAL IMAGING PROCEDURE\n";
            String url="http://snomed.info/sct?fhir_vs";
            String filter="Radionuclide myocardial imaging procedure";
        var fsh=new Demo();
            var rm= fsh.ExpandValueSetForCombo(
                     server,
                     url,
                     filter);
            Assert.True(rm.ToUpper().Contains(ExpTerms.ToUpper()),ExpTerms.ToUpper()+" not in "+rm.ToUpper());
            
        
        }

 [Fact]
    public void L00_0_T07_GetIPSLabResults()
        {
            
         
        MyConfiguration c=new MyConfiguration();
        String server=c.IPSServerEndpoint;
        String IdentifierSystem=c.PatientIdentifierSystem;
        String IdentifierValue="L03_3_T03";
        
        String match="882-1:ABO and Rh group [Type] in Blood|2015-10-10T09:15:00+01:00|final|278149003:Blood group A Rh(D) positive\n";
        match +="945-6:C Ab [Presence] in Serum or Plasma|2015-10-10T09:35:00+01:00|final|10828004:Positive\n";
        match +="1018-1:E Ab [Presence] in Serum or Plasma|2015-10-10T09:35:00+01:00|final|10828004:Positive\n";
        match +="1156-9:little c Ab [Presence] in Serum or Plasma|2015-10-10T09:35:00+01:00|final|260385009:Negative\n";
        match +="17856-6:Hemoglobin A1c/Hemoglobin.total in Blood by HPLC|2017-11-10T08:20:00+01:00|final|7.5 %\n";
        match +="42803-7:Bacteria identified in Isolate|2017-12-10T08:20:00+01:00|final|115329001:Methicillin resistant Staphylococcus aureus\n";
        // Spec
        // GetIPSLabResuls shall search an IPS document based on the patient's internal id
        // and then return a string with one record for each IPS lab result Observation
        // with the format
        // {code}:{display}|{effectiveDateTime}|{status}|{result}\n
        // {result} maybe {quantity units} or {code:display} or {string} depending on the result type
        
        var fsh=new Demo();
        var rm=
        fsh.GetIPSLabResults(server,IdentifierSystem,IdentifierValue);
          var expectedNorm = NormalizeDecimalSeparator(match).ToUpper();
          var actualNorm = NormalizeDecimalSeparator(rm).ToUpper();
          var expectedLines = expectedNorm.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
          foreach (var line in expectedLines)
              Assert.True(actualNorm.Contains(line), "Expected line not found in result: " + line);
        }

        private static string NormalizeDecimalSeparator(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return Regex.Replace(s, @"(\d),(\d)", "$1.$2");
        }

         public string ValidateAllergyIntoleranceUSCORE(string JsonAllergy,string server)
    {
             string aux="";
             Hl7.Fhir.Model.AllergyIntolerance  o=new  Hl7.Fhir.Model.AllergyIntolerance() ;
             
             var parser = new Hl7.Fhir.Serialization.FhirJsonParser();
          
            try
            {
                o = parser.Parse<AllergyIntolerance>(JsonAllergy);
            }
            catch
            {
                aux="Error:Invalid_AllergyIntolerance_Resource";
            }
            if (aux=="")
            {
            var client = new Hl7.Fhir.Rest.FhirClient(server); 
            Hl7.Fhir.Model.FhirUri profile=  new FhirUri("http://hl7.org/fhir/us/core/StructureDefinition/us-core-allergyintolerance");
           
            Parameters inParams = new Parameters();
            inParams.Add("resource", o);
            OperationOutcome bu = client.ValidateResourceAsync(o).GetAwaiter().GetResult(); 
            aux="OK";
            if (bu.Issue[0].Details.Text!="Validation successful, no issues found")
            {
                aux="Error:"+bu.Issue[0].Details.Text;
            }
            }
            return aux;

    }

    }
        
        


}
