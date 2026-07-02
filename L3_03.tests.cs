using System;
using Xunit;
using fhirclient_dotnet;
namespace fhirclient_dotnet_tests
{
    public class L03_3_GetClinicalDataFromIPS_Tests
    {
        //L03_3_T01: Patient Does Not Exist
        //L03_3_T02: Patient Exists, has no IPS
        //L03_3_T03: Patient Exists, IPS with MEDS
        //L03_3_T04: Patient Exists, IPS with no MEDS
        //L03_3_T05: Patient Exists, IPS with IMM
        //L03_3_T06: Patient Exists, IPS with no IMM
        

   
        [Fact]
        public string L03_3_T01_GetMedication_NonExistingPatient()

        {
              MyConfiguration c=new MyConfiguration();
            var server=c.IPSServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
            var IdentifierValue="L03_3_T01";
            var ExpMedications="Error:Patient_Not_Found";
            var fsh=new FetchIPS();
            var rm=fsh.GetIPSMedications(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpMedications==rm,ExpMedications+"!="+rm);
            return rm;
        }
        [Fact]
         public string L03_3_T02_GetMedicationsPatientWithNoIPS()

        {
             MyConfiguration c=new MyConfiguration();
            var server=c.IPSServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
            var IdentifierValue="L03_3_T02";
            var fsh=new FetchIPS();
            var rm1=fsh.GetIPSMedications(server,IdentifierSystem,IdentifierValue);
            var rm2=fsh.GetIPSImmunizations(server,IdentifierSystem,IdentifierValue);
            var NoMeds = (rm1=="Unknown||no-medication-info:No information about medications\n");
            bool NoImms = (rm2=="Error:IPS_No_Immunizations");
            bool NoIPS = (NoMeds && NoImms);
            String rm="At least one IPS relevant section is populated";
            if (NoIPS) {rm="Error:No_IPS";}
            Assert.True(NoIPS,rm1+","+rm2);
            return rm; 
        }

     
 [Fact]
         public string L03_3_T03_GetMedicationsPatientWithIPSMeds()

        {
             MyConfiguration c=new MyConfiguration();
            var server=c.IPSServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
             var IdentifierValue="L03_3_T03";
            var ExpMedication="Active|2015-03|108774000:Product containing anastrozole (medicinal product)\n";
            ExpMedication+="Active|2016-01|412588001:Cimicifuga racemosa extract (substance)\n";
            var fsh=new FetchIPS();
            var rm=fsh.GetIPSMedications(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpMedication==rm,ExpMedication+"!="+rm);
            return rm;
        }
[Fact]
         public string L03_3_T04_GetMedicationsPatientWithIPSNoMeds()

        {
             MyConfiguration c=new MyConfiguration();
            var server=c.IPSServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
             var IdentifierValue="L03_3_T04";
            var ExpMedication="Unknown||no-medication-info:No information about medications\n";
            var fsh=new FetchIPS();
            var rm=fsh.GetIPSMedications(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpMedication==rm,ExpMedication+"!="+rm);
            return rm;
        }

 [Fact]
         public string L03_3_T05_GetMedicationsPatientWithIPSImmunization()

        {
              MyConfiguration c=new MyConfiguration();
            var server=c.IPSServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
            var IdentifierValue="L03_3_T04";
            var ExpImmunization="Completed|1998-06-04T00:00:00+02:00|414005006:Diphtheria + Pertussis + Poliomyelitis + Tetanus vaccine\n";
            var fsh=new FetchIPS();
            var rm=fsh.GetIPSImmunizations(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpImmunization==rm,ExpImmunization+"!="+rm);
            return rm;
        }
[Fact]
         public string L03_3_T06_GetMedicationsPatientWithIPSNoImm()

        {
              MyConfiguration c=new MyConfiguration();
            var server=c.IPSServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
            var IdentifierValue="L03_3_T03";
            var ExpImmunization="Error:IPS_No_Immunizations";
            var fsh=new FetchIPS();
            var rm=fsh.GetIPSImmunizations(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpImmunization==rm,ExpImmunization+"!="+rm);
            return rm;
        }

 
    }

}
