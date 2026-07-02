namespace fhirclient_dotnet_submission
{
    public  class MyConfiguration
    {

         public string ServerEndpoint{get;}=
            "https://hl7int-server.com/server/fhir/";
        
            
        public string PatientIdentifierSystem{get;}=
            "http://fhirintermediate.org/patient_id";
        
            
        public string TerminologyServerEndpoint{get;}=
            "https://r4.ontoserver.csiro.au/fhir";

        public string IPSServerEndpoint{get;}=
            "https://hl7-ips-server.hl7.org/fhir/";
            
        public string AssignmentSubmissionFHIRServer{get;}=
              "http://fhirserver.hl7fundamentals.org/fhir";

        public string StudentId{get;}=
             "laura.baum@cookchildrens.org";
        
        public string StudentName{get;}=
            "Laura Baum";    
        
    }

}