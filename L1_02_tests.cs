using System;
using Xunit;
using fhirclient_dotnet;
namespace fhirclient_dotnet_tests
{
    
  public class L01_2_CompareDemographicData_Tests
    {
        //L01_2_T01: Patient Does Not Exist
        //L01_2_T02: Patient Exists, different family name 
        //L01_2_T03: Patient Exists, different given name 
        //L01_2_T04: Patient Exists, different birth date
        //L01_2_T05: Patient Exists, different gender
        //L01_2_T06: Patient Exists, everything is OK

        [Fact]
        public string L01_2_T01_CompareDemographics_NonExistingPatient()
        {
              MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
              var IdentifierValue="L01_2_T01";
            var result="Error:Patient_Not_Found";
            var fsh=new CompareDemographics();
            //Nothing to compare since the patient does not exist
            var myFamily="";
            var myGiven="";
            var myGender="";
            var myBirth="";
            var comparison=fsh.GetDemographicComparison(server,IdentifierSystem,IdentifierValue,myFamily,myGiven,myGender,myBirth);
            Assert.True(result==comparison,result+"!="+comparison);
            return comparison;
        }
         [Fact]
        public string L01_2_T02_CompareDemographics_DifferentFamilyName()
        {
              MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
          var IdentifierValue="L01_2_T02";
            var result ="{family}|Dougras|Douglas|{red}\n";
                result+="{given}|Jamieson Harris|Jamieson Harris|{green}\n";
                result+="{gender}|MALE|MALE|{green}\n";
                result+="{birthDate}|1968-07-23|1968-07-23|{green}\n";
            var fsh=new CompareDemographics();
            //family is different
            var myFamily="Dougras";
            var myGiven="Jamieson Harris";
            var myGender="male";
            var myBirth="1968-07-23";
            var comparison=fsh.GetDemographicComparison(server,IdentifierSystem,IdentifierValue,myFamily,myGiven,myGender,myBirth);
            Assert.True(result==comparison,result+"!="+comparison);
            return comparison;
        }
         [Fact]
        public string L01_2_T03_CompareDemographics_DifferentGivenName()
        {
              MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
          var IdentifierValue="L01_2_T02";
            var result ="{family}|Douglas|Douglas|{green}\n";
                result+="{given}|Jamieson|Jamieson Harris|{red}\n";
                result+="{gender}|MALE|MALE|{green}\n";
                result+="{birthDate}|1968-07-23|1968-07-23|{green}\n";
            var fsh=new CompareDemographics();
            //given is different
            var myFamily="Douglas";
            var myGiven="Jamieson";
            var myGender="male";
            var myBirth="1968-07-23";
            var comparison=fsh.GetDemographicComparison(server,IdentifierSystem,IdentifierValue,myFamily,myGiven,myGender,myBirth);
            Assert.True(result==comparison,result+"!="+comparison);
        return comparison;
        }
         [Fact]
        public string L01_2_T04_CompareDemographics_DifferentBirthDate()
        {

               MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
         var IdentifierValue="L01_2_T02";
            var result ="{family}|Douglas|Douglas|{green}\n";
                result+="{given}|Jamieson Harris|Jamieson Harris|{green}\n";
                result+="{gender}|MALE|MALE|{green}\n";
                result+="{birthDate}|1968-07-24|1968-07-23|{red}\n";
            var fsh=new CompareDemographics();
            //bdate is different
            var myFamily="Douglas";
            var myGiven="Jamieson Harris";
            var myGender="male";
            var myBirth="1968-07-24";
            var comparison=fsh.GetDemographicComparison(server,IdentifierSystem,IdentifierValue,myFamily,myGiven,myGender,myBirth);
            Assert.True(result==comparison,result+"!="+comparison);
            return comparison;
        }
         [Fact]
        public string L01_2_T05_CompareDemographics_DifferentGender()
        {

               MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
         var IdentifierValue="L01_2_T02";
            var result ="{family}|Douglas|Douglas|{green}\n";
                result+="{given}|Jamieson Harris|Jamieson Harris|{green}\n";
                result+="{gender}|FEMALE|MALE|{red}\n";
                result+="{birthDate}|1968-07-23|1968-07-23|{green}\n";
            var fsh=new CompareDemographics();
            //gender is different
            var myFamily="Douglas";
            var myGiven="Jamieson Harris";
            var myGender="female";
            var myBirth="1968-07-23";
            var comparison=fsh.GetDemographicComparison(server,IdentifierSystem,IdentifierValue,myFamily,myGiven,myGender,myBirth);
            Assert.True(result==comparison,result+"!="+comparison);
            return comparison;

        }
           [Fact]
        public string L01_2_T06_CompareDemographics_PerfectMatch()
        {
               MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
         var IdentifierValue="L01_2_T02";
            var result ="{family}|Douglas|Douglas|{green}\n";
                result+="{given}|Jamieson Harris|Jamieson Harris|{green}\n";
                result+="{gender}|MALE|MALE|{green}\n";
                result+="{birthDate}|1968-07-23|1968-07-23|{green}\n";
            var fsh=new CompareDemographics();
            var myFamily="Douglas";
            var myGiven="Jamieson Harris";
            var myGender="male";
            var myBirth="1968-07-23";
            var comparison=fsh.GetDemographicComparison(server,IdentifierSystem,IdentifierValue,myFamily,myGiven,myGender,myBirth);
            Assert.True(result==comparison,result+"!="+comparison);
           return comparison;
        }
      

    }


}