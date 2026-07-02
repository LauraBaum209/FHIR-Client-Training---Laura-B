using System;
using Xunit;
using Xunit.Abstractions;

using fhirclient_dotnet;
namespace fhirclient_dotnet_tests
{
    public class L01_3_GetProvidersNearPatient_Tests
    {

        //L01_3_T01: Patient Does Not Exist
        //L01_3_T02: Patient Exists, No City element
        //L01_3_T03: Patient Exists, No Provider in the City
        //L01_3_T04: Patient Exists, One Provider in the City
        //L01_3_T05: Patient Exists, More than One Provider in the City[Fact]
        public string L01_3_T01_GetProviderNearCity_NonExistingPatient()

        {
              MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
          var IdentifierValue="L01_3_T01";
            var Providers="Error:Patient_Not_Found";
            var fsh=new GetProvidersNearPatient();
            var rm=fsh.GetProvidersNearCity(server,IdentifierSystem,IdentifierValue);
            Assert.Equal(Providers,rm);
            return rm;
        }
        [Fact]
         public string L01_3_T02_GetProviderNearCity_PatientWithNoCityElement()

        {
              MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
          var IdentifierValue="L01_3_T02";
            var Providers="Error:Patient_w/o_City";
            var fsh=new GetProvidersNearPatient();
            var rm=fsh.GetProvidersNearCity(server,IdentifierSystem,IdentifierValue);
            Assert.Equal(Providers,rm);            
            return rm;
        }

        [Fact]
         public string L01_3_T03_GetProviderNearCity_NoProviderInTheCity()

        {
               MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
         var IdentifierValue="L01_3_T03";
            var Providers="Error:No_Provider_In_Patient_City";
            var fsh=new GetProvidersNearPatient();
            var rm=fsh.GetProvidersNearCity(server,IdentifierSystem,IdentifierValue);
            Assert.Equal(Providers,rm);            
            return rm;
        }
 [Fact]
         public string L01_3_T04_GetProviderNearCity_OneProvider()

        {
              MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
          var IdentifierValue="L01_3_T04";
            var Providers="OnlyPhysician,InTown|Phone:+402-772-7777|2000 ONE PROVIDER DRIVE|Certified Nurse Practitioner\n";
            var fsh=new GetProvidersNearPatient();
            var rm=fsh.GetProvidersNearCity(server,IdentifierSystem,IdentifierValue);
            Assert.True(Providers==rm,Providers+"!="+rm);
            return rm;            
        
        }
[Fact]
         public string L01_3_T05_GetProviderNearCity_SeveralProviders()

        {
             MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
           var IdentifierValue="L01_3_T05";
            var Providers="OnePhysician,First|Phone:+402-772-7777|2000 ONE PROVIDER DRIVE|OB/GYN\nTwoPhysician,Second|Phone:+403-772-7777|3000 TWO PROVIDER DRIVE|FAMILY MEDICINE\n";
            var fsh=new GetProvidersNearPatient();
            var rm=fsh.GetProvidersNearCity(server,IdentifierSystem,IdentifierValue);
            Assert.True(Providers==rm,Providers+"!="+rm);
            return rm;
        }
    }

}
