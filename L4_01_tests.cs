using Xunit;
using fhirclient_dotnet;
using Hl7.Fhir.Model; 
using Hl7.Fhir.Rest; 

namespace fhirclient_dotnet_tests
{
    public class L04_1_CreateUSCoreLabResult_Tests
    {
        //L04_1_T01: Patient Does Not Exist
        //L04_2_T02: Patient Exists, Create Numerical Observation
        //L03_3_T03: Patient Exists, Create Categorical Observation
  
        [Fact]
        public string L04_1_T01_CreateNumerical_NonExistingPatient()

        {
               MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
           var IdentifierValue="L04_1_T01";
            var ExpObservations="Error:Patient_Not_Found";
            string ObservationStatusCode="";
            string ObservationDateTime="";
            string ObservationLOINCCode="";
            string ObservationLOINCDisplay="";
            string ResultType="";
            string NumericResultValue="";
            string NumericResultUCUMUnit="";
            string CodedResultSNOMEDCode="";
            string CodedResultSNOMEDDisplay="";
      
            var fsh=new CreateUSCoreObs();
            var rm=fsh.CreateUSCoreR4LabObservation(server,IdentifierSystem,IdentifierValue
            ,ObservationStatusCode,ObservationDateTime,ObservationLOINCCode,ObservationLOINCDisplay,ResultType,
            NumericResultValue,NumericResultUCUMUnit,CodedResultSNOMEDCode,CodedResultSNOMEDDisplay);
            Assert.True(ExpObservations==rm,ExpObservations+"!="+rm);
            return rm;
        }
      
  [Fact]
         public string L04_1_T02_CreateCodedObservation()

        {

               MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
           var IdentifierValue="L04_1_T02";
            var ExpObservations="Error:Patient_Not_Found";
            string ObservationStatusCode="final";
            string ObservationDateTime="2020-10-11T20:30:00Z";
            string ObservationLOINCCode="5778-6";
            string ObservationLOINCDisplay="Color of Urine";
            string ResultType="Coded";
            string NumericResultValue="";
            string NumericResultUCUMUnit="";
            string CodedResultSNOMEDCode="371244009";
            string CodedResultSNOMEDDisplay="Yellow";
      
            var fsh=new CreateUSCoreObs();
            var rm=fsh.CreateUSCoreR4LabObservation(server,IdentifierSystem,IdentifierValue
            ,ObservationStatusCode,ObservationDateTime,ObservationLOINCCode,ObservationLOINCDisplay,ResultType,
            NumericResultValue,NumericResultUCUMUnit,CodedResultSNOMEDCode,CodedResultSNOMEDDisplay);
           
            if (rm!="Error:Patient_Not_Found")
            {
            ExpObservations=ValidateObservationUSCORE(rm,server);
            
                if (ExpObservations=="")
                {
                    ExpObservations=VerifyObservationContents(rm,server,IdentifierSystem,IdentifierValue
                    ,ObservationStatusCode,ObservationDateTime,ObservationLOINCCode,ObservationLOINCDisplay,ResultType,
                    NumericResultValue,NumericResultUCUMUnit,CodedResultSNOMEDCode,CodedResultSNOMEDDisplay);
                }
            }
            Assert.True(ExpObservations=="",ExpObservations);
             return ExpObservations;
        }


 [Fact]
         public string L04_1_T03_CreateNumericalObservation()

        {
              MyConfiguration c=new MyConfiguration();
            var server=c.ServerEndpoint;
            var IdentifierSystem=c.PatientIdentifierSystem;
            var IdentifierValue="L04_1_T02";
            var ExpObservations="Error:Patient_Not_Found";
            string ObservationStatusCode="final";
            string ObservationDateTime="2020-10-11T20:30:00Z";
            string ObservationLOINCCode="1975-2";
            string ObservationLOINCDisplay="Bilirubin, blood";
            string ResultType="numeric";
            string NumericResultValue="8.6";
            string NumericResultUCUMUnit="mg/dl";
            string CodedResultSNOMEDCode="";
            string CodedResultSNOMEDDisplay="";
      
            var fsh=new CreateUSCoreObs();
            var rm=fsh.CreateUSCoreR4LabObservation(server,IdentifierSystem,IdentifierValue
            ,ObservationStatusCode,ObservationDateTime,ObservationLOINCCode,ObservationLOINCDisplay,ResultType,
            NumericResultValue,NumericResultUCUMUnit,CodedResultSNOMEDCode,CodedResultSNOMEDDisplay);
            ExpObservations=ValidateObservationUSCORE(rm,server);
            
            if (ExpObservations=="")
            {
                ExpObservations=VerifyObservationContents(rm,server,IdentifierSystem,IdentifierValue
                ,ObservationStatusCode,ObservationDateTime,ObservationLOINCCode,ObservationLOINCDisplay,ResultType,
                NumericResultValue,NumericResultUCUMUnit,CodedResultSNOMEDCode,CodedResultSNOMEDDisplay);
            }
            Assert.True(ExpObservations=="",ExpObservations);
            return ExpObservations;
           
        
        }
      
    public string VerifyObservationContents(string JsonObservation,
            string server,string IdentifierSystem,string IdentifierValue
            ,string ObservationStatusCode,string ObservationDateTime,string ObservationLOINCCode,string ObservationLOINCDisplay,string ResultType,
            string NumericResultValue,string NumericResultUCUMUnit,string CodedResultSNOMEDCode,string CodedResultSNOMEDDisplay)

    {
            string aux="";
              Hl7.Fhir.Model.Observation  o=new  Hl7.Fhir.Model.Observation() ;
             var parser = new Hl7.Fhir.Serialization.FhirJsonParser();
          
            try
            {
                o = parser.Parse<Observation>(JsonObservation);
                if(o.Status.ToString().ToUpper()!=ObservationStatusCode.ToUpper()){aux+="Status Code Differs";}
                if(o.Code.Coding[0].Code!=ObservationLOINCCode){aux+="Code Differs";}
                if(o.Effective.ToString()!=ObservationDateTime){aux+="Date Differs";}
                if(ResultType=="numeric")
                {
                    Quantity q= (Quantity) o.Value;
                    if (q.Unit!=NumericResultUCUMUnit){aux+="Numeric Result Unit differs";}
                    decimal? actualVal = q.Value;
                    if (!decimal.TryParse(NumericResultValue, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var expectedVal) || !actualVal.HasValue || System.Math.Abs(actualVal.Value - expectedVal) > 0.0001m){aux+="Numeric Result Value differs";}
                 }
                else
                {
                    CodeableConcept c=(CodeableConcept) o.Value;
                    if (c.Coding[0].Code!=CodedResultSNOMEDCode){aux+="Coded Result Code differs";}
                }

            }
            catch
            {
                aux="Error:Invalid_Observation_Resource";
            }
           
            return aux;
    }
    
    public string ValidateObservationUSCORE(string JsonObservation,string server)
    {
             string aux="";
             Hl7.Fhir.Model.Observation  o=new  Hl7.Fhir.Model.Observation() ;
             var parser = new Hl7.Fhir.Serialization.FhirJsonParser();
          
            try
            {
                o = parser.Parse<Observation>(JsonObservation);
            }
            catch
            {
                aux="Error:Invalid_Observation_Resource";
            }
            if (aux=="")
            {
            var client = new Hl7.Fhir.Rest.FhirClient(server); 
            OperationOutcome bu = client.ValidateResourceAsync(o).GetAwaiter().GetResult();
            var text = (bu?.Issue != null && bu.Issue.Count > 0 && bu.Issue[0].Details != null) ? (bu.Issue[0].Details.Text ?? "") : "";
            bool isSuccess = text == "Validation successful, no issues found" || text == "All OK"
                || (text != null && text.Contains("Reference to draft CodeSystem") && text.Contains("observation-category"));
            if (!isSuccess)
            {
                aux = string.IsNullOrEmpty(text) ? "" : "Error:" + text;
            }
        }
            return aux;

    }


}
}