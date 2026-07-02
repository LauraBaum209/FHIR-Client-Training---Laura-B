using System;
using Xunit;
using fhirclient_dotnet;
namespace fhirclient_dotnet_tests
{
    
    public class L01_1_FetchDemographicData_Tests
    {
        //L01_1_T01: Patient Does Not Exist
        //L01_1_T02: Patient Exists, but has no telecom element
        //L01_1_T03: Patient Exists, only with one phone number
        //L01_1_T04: Patient Exists, only with one email address
        //L01_1_T05: Patient Exists, with more than one phone number and email addresses
        //L01_1_T06: Patient Exists, with one telecom which is not phone or email
         [Fact]
        public string L01_1_T01_GetMailAndTelecom_NonExistingPatient()

        {
            MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
            var IdentifierValue="L01_1_T01";
            var MailAndTelecom="Error:Patient_Not_Found";
            var fsh=new FetchDemographics();
            var rm=fsh.GetPatientPhoneAndEmail(server,IdentifierSystem,IdentifierValue);
            Assert.Equal(MailAndTelecom,rm);
            return rm;
        }
        [Fact]
         public string L01_1_T02_GetMailAndTelecom_PatientWithNoTelecomElement()

        {
            MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
            var IdentifierValue="L01_1_T02";
            var MailAndTelecom="Emails:-\nPhones:-\n";
            var fsh=new FetchDemographics();
            var rm=fsh.GetPatientPhoneAndEmail(server,IdentifierSystem,IdentifierValue);
            Assert.Equal(MailAndTelecom,rm);            
            return rm;
        
        }

        [Fact]
         public string L01_1_T03_GetMailAndTelecom_PatientWithPhoneNoMail()

        {
            MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
            var IdentifierValue="L01_1_T03";
            var MailAndTelecom="Emails:-\nPhones:+15555555555(Home)\n";
            var fsh=new FetchDemographics();
            var rm=fsh.GetPatientPhoneAndEmail(server,IdentifierSystem,IdentifierValue);
            Assert.Equal(MailAndTelecom,rm);            
            return rm;
        }
 [Fact]
         public string L01_1_T04_GetMailAndTelecom_PatientWithMailNoPhone()

        {
             MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
           var IdentifierValue="L01_1_T04";
            var MailAndTelecom="Emails:mymail@patientt04.com(Home)\nPhones:-\n";
            var fsh=new FetchDemographics();
            var rm=fsh.GetPatientPhoneAndEmail(server,IdentifierSystem,IdentifierValue);
            Assert.Equal(MailAndTelecom,rm);            
            return rm;
        
        }
[Fact]
         public string L01_1_T05_GetMailAndTelecom_PatientWithSeveralMailPhones()

        {
             MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
           var IdentifierValue="L01_1_T05";
            var MailAndTelecom="Emails:mymail@patientt05job.com(Work),mymail@patientt05.com(Home)\nPhones:+15555555555(Work),+16666666666(Home)\n";
            var fsh=new FetchDemographics();
            var rm=fsh.GetPatientPhoneAndEmail(server,IdentifierSystem,IdentifierValue);
            Assert.Equal(MailAndTelecom,rm);            
            return rm;
        
        }

[Fact]
         public string L01_1_T06_GetMailAndTelecom_PatientWithTelecomNotMailPhone()

        {
            MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
            var IdentifierValue="L01_1_T06";
            var MailAndTelecom="Emails:-\nPhones:-\n";
            var fsh=new FetchDemographics();
            var rm=fsh.GetPatientPhoneAndEmail(server,IdentifierSystem,IdentifierValue);
            Assert.Equal(MailAndTelecom,rm);            
            return rm;
        }

 
    }

    

}
