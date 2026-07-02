using System;
using Xunit;
using fhirclient_dotnet;
namespace fhirclient_dotnet_tests
{
    public class L03_2_GetClinicalDataMedication_Tests
    {
        //L03_2_T01: Patient Does Not Exist
        //L03_2_T02: Patient Exists, but has no Medication data
        //L03_2_T03: Patient Exists, with one med resource
        //L03_2_T04: Patient Exists, with several med resources
      
        [Fact]
        public string L03_2_T01_GetMedication_NonExistingPatient()

        {
            MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
              var IdentifierValue="L03_2_T01";
            var ExpMedications="Error:Patient_Not_Found";
            var fsh=new FetchMedication();
            var rm=fsh.GetMedications(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpMedications==rm,ExpMedications+"!="+rm);
            return rm;
        }
        [Fact]
         public string L03_2_T02_GetMedicationsPatientWithNoMedication()

        {
             MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
             var IdentifierValue="L03_2_T02";
            var ExpMedication="Error:No_Medications";
            var fsh=new FetchMedication();
            var rm=fsh.GetMedications(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpMedication==rm,ExpMedication+"!="+rm);
             return rm;
        }

     
 [Fact]
         public string L03_2_T03_GetMedicationsPatientWithOneMedication()

        {
             MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
             var IdentifierValue="L03_2_T03";
            var ExpMedication="Active|Order|2021-01-05|582620:Nizatidine 15 MG/ML Oral Solution [Axid]|John Requester, MD\n";
            var fsh=new FetchMedication();
            var rm=fsh.GetMedications(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpMedication==rm,ExpMedication+"!="+rm);
            return rm;
        }
[Fact]
         public string L03_2_T04_GetMedicationsPatientWithSeveralMedications()

        {
               MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
           var IdentifierValue="L03_2_T04";
            var ExpMedication="Active|Order|2021-01-05|582620:Nizatidine 15 MG/ML Oral Solution [Axid]|Mary Requesting, MD\n";
                ExpMedication+="Active|Order|2021-01-05|198436:Acetaminophen 325 MG Oral Capsule|Mary Requesting, MD\n";
            var fsh=new FetchMedication();
            var rm=fsh.GetMedications(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpMedication==rm,ExpMedication+"!="+rm);
            return rm;
        }

 
    }

}
