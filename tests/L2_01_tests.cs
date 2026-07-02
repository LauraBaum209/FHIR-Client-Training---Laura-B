using System;
using Xunit;
using fhirclient_dotnet;
namespace fhirclient_dotnet_tests
{
    public class L02_1_GetEthnicityData_Tests
    {
        //L02_1_T01: Patient Does Not Exist
        //L02_1_T02: Patient Exists, but has no extension for ethnicity
        //L01_1_T03: Patient Exists, has a non-compliant extension for ethnicity
        //L01_1_T04: Patient Exists, with a minimum compliant ethnicity extension
        //L01_1_T05: Patient Exists, with a full ethnicity extension
        
        [Fact]
        public string L02_1_T01_GetEthnicity_NonExistingPatient()

        {
          MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
                var IdentifierValue="L02_1_T01";
            var ExpEthnicity="Error:Patient_Not_Found";
            var fsh=new FetchEthnicity();
            var rm=fsh.GetEthnicity(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpEthnicity==rm,ExpEthnicity+"!="+rm);
            return rm;
        }
        [Fact]
         public  string L02_1_T02_GetEthnicityPatientWithNoEthnicityExtension()

        {
            MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
              var IdentifierValue="L02_1_T02";
            var ExpEthnicity="Error:No_us-core-ethnicity_Extension";
            var fsh=new FetchEthnicity();
            var rm=fsh.GetEthnicity(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpEthnicity==rm,ExpEthnicity+"!="+rm);
            return rm;  
        }

        [Fact]
         public  string L02_1_T03_GetEthnicityPatientWithNonCompliantEthnicityExtension()

        {
             MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
             var IdentifierValue="L02_1_T03";
            var ExpEthnicity="Error:Non_Conformant_us-core-ethnicity_Extension";
            var fsh=new FetchEthnicity();
            var rm=fsh.GetEthnicity(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpEthnicity==rm,ExpEthnicity+"!="+rm);
            return rm;
        }
 [Fact]
         public  string L02_1_T04_GetEthnicityPatientWithMinimumEthnicityExtension()

        {
              MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
            var IdentifierValue="L02_1_T04";
            var ExpEthnicity="text|Hispanic or Latino\n";
            ExpEthnicity+="code|2135-2:Hispanic or Latino\n";
            var fsh=new FetchEthnicity();
            var rm=fsh.GetEthnicity(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpEthnicity==rm,ExpEthnicity+"!="+rm);
            return rm; 
        }
[Fact]
         public  string L02_1_T05_GetEthnicityPatientWithFullEthnicityExtension()

        {
              MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
            var IdentifierValue="L02_1_T05";
            var ExpEthnicity="text|Hispanic or Latino\n";
            ExpEthnicity+="code|2135-2:Hispanic or Latino\n";
            ExpEthnicity+="detail|2184-0:Dominican\n";
            ExpEthnicity+="detail|2148-5:Mexican\n";
            ExpEthnicity+="detail|2151-9:Chicano\n";
            var fsh=new FetchEthnicity();
            var rm=fsh.GetEthnicity(server,IdentifierSystem,IdentifierValue);
            Assert.True(ExpEthnicity==rm,ExpEthnicity+"!="+rm);
            return rm;
        }

 
    }

}
