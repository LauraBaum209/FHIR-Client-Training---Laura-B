using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using System.Collections.Generic;
using fhirclient_dotnet;
namespace fhirclient_dotnet_submission
{
    public class SubmissionCreator
    {
      
        public class TestResult
        {
            public string Code;
            public string Name;
            public string Result;
        }
     
        public Dictionary<string,string> L01_1_Tests()
        {
            MyConfiguration c=new MyConfiguration();
            String ServerAddress=c.ServerEndpoint;
            String IdentifierSystem=c.PatientIdentifierSystem;
             
            Dictionary<string,string> tl=new System.Collections.Generic.Dictionary<string, string>();
            Dictionary<string,string> tr=new System.Collections.Generic.Dictionary<string, string>();
            tl.Add("L01_1_T01","L01_1_T01");
            tl.Add("L01_1_T02","L01_1_T02");
            tl.Add("L01_1_T03","L01_1_T03");
            tl.Add("L01_1_T04","L01_1_T04");
            tl.Add("L01_1_T05","L01_1_T05");
            tl.Add("L01_1_T06","L01_1_T06");
            foreach(KeyValuePair<string,string> test in tl)
            {
          
                    String IdentifierValue=test.Value;
                    fhirclient_dotnet.FetchDemographics L01_1=new FetchDemographics();
                    String result= L01_1.GetPatientPhoneAndEmail(ServerAddress,IdentifierSystem,IdentifierValue);
                    tr.Add(test.Key,result);
            }
            return tr;
        }

    public class Person {
        public String identifier;
        public String family;
        public String given;
        public String gender;
        public String birthDate;

        public Person(String Identifier, String Family, String Given, String Gender, String BirthDate) {
            this.identifier = Identifier;
            this.family = Family;
            this.given = Given;
            this.gender = Gender;
            this.birthDate = BirthDate;
        }

    }

    private static Dictionary<String, String> L01_2_Tests() {
        Dictionary<String, String> my_results = new Dictionary<String, String>();

        Dictionary<String, Person> my_tests = new Dictionary<String, Person>();
        
        my_tests.Add("L01_2_T01", new Person("L01_2_T01", "", "", "", ""));
        my_tests.Add("L01_2_T02", new Person("L01_2_T02", "Dougras", "Jamieson Harris", "male", "1968-07-23"));
        my_tests.Add("L01_2_T03", new Person("L01_2_T02", "Douglas", "Jamieson", "male", "1968-07-23"));
        my_tests.Add("L01_2_T04", new Person("L01_2_T02", "Douglas", "Jamieson Harris", "male", "1968-07-24"));
        my_tests.Add("L01_2_T05", new Person("L01_2_T02", "Douglas", "Jamieson Harris", "female", "1968-07-23"));
        my_tests.Add("L01_2_T06", new Person("L01_2_T02", "Douglas", "Jamieson Harris", "male", "1968-07-23"));

         foreach(KeyValuePair<string,Person> test in my_tests)
            {
                String aux = "";
           
                MyConfiguration c=new MyConfiguration();
                String ServerAddress=c.ServerEndpoint;
                String IdentifierSystem=c.PatientIdentifierSystem;
                Person p=test.Value;
                String IdentifierValue = p.identifier;
                String myFamily = p.family;
                String myGiven = p.given;
                String myGender = p.gender;
                String myBirth = p.birthDate;
                try {
                    fhirclient_dotnet.CompareDemographics L01_2=new CompareDemographics();
                    aux = L01_2.GetDemographicComparison(ServerAddress, IdentifierSystem, IdentifierValue, myFamily, myGiven,
                    
                    myGender, myBirth);

                } catch (Exception e) {
                    aux = e.Message;

                }
                my_results.Add(test.Key, aux);
            }
        
        return my_results;
    }

        private Dictionary<string,string> L01_3_Tests()
        {
            MyConfiguration c=new MyConfiguration();
            String ServerAddress=c.ServerEndpoint;
            String IdentifierSystem=c.PatientIdentifierSystem;
             
            Dictionary<string,string> tl=new System.Collections.Generic.Dictionary<string, string>();
            Dictionary<string,string> tr=new System.Collections.Generic.Dictionary<string, string>();
            tl.Add("L01_3_T01","L01_3_T01");
            tl.Add("L01_3_T02","L01_3_T02");
            tl.Add("L01_3_T03","L01_3_T03");
            tl.Add("L01_3_T04","L01_3_T04");
            tl.Add("L01_3_T05","L01_3_T05");
            foreach(KeyValuePair<string,string> test in tl)
            {
          
                    String IdentifierValue=test.Value;
                    fhirclient_dotnet.GetProvidersNearPatient L01_3=new fhirclient_dotnet.GetProvidersNearPatient();
                    String result= L01_3.GetProvidersNearCity(ServerAddress,IdentifierSystem,IdentifierValue);
                    tr.Add(test.Key,result);
            }
            return tr;
        }
   private Dictionary<string,string> L03_1_Tests()
        {
            MyConfiguration c=new MyConfiguration();
            String ServerAddress=c.ServerEndpoint;
            String IdentifierSystem=c.PatientIdentifierSystem;
             
            Dictionary<string,string> tl=new System.Collections.Generic.Dictionary<string, string>();
            Dictionary<string,string> tr=new System.Collections.Generic.Dictionary<string, string>();
            tl.Add("L03_1_T01","L03_1_T01");
            tl.Add("L03_1_T02","L03_1_T02");
            tl.Add("L03_1_T03","L03_1_T03");
            tl.Add("L03_1_T04","L03_1_T04");
            foreach(KeyValuePair<string,string> test in tl)
            {
          
                    String IdentifierValue=test.Value;
                    fhirclient_dotnet.FetchImmunization L03_1=new fhirclient_dotnet.FetchImmunization();
                    String result= L03_1.GetImmunizations(ServerAddress,IdentifierSystem,IdentifierValue);
                    tr.Add(test.Key,result);
            }
            return tr;
        }

        private Dictionary<string,string> L03_2_Tests()
        {
            MyConfiguration c=new MyConfiguration();
            String ServerAddress=c.ServerEndpoint;
            String IdentifierSystem=c.PatientIdentifierSystem;
             
            Dictionary<string,string> tl=new System.Collections.Generic.Dictionary<string, string>();
            Dictionary<string,string> tr=new System.Collections.Generic.Dictionary<string, string>();
            tl.Add("L03_2_T01","L03_2_T01");
            tl.Add("L03_2_T02","L03_2_T02");
            tl.Add("L03_2_T03","L03_2_T03");
            tl.Add("L03_2_T04","L03_2_T04");
            foreach(KeyValuePair<string,string> test in tl)
            {
          
                    String IdentifierValue=test.Value;
                    fhirclient_dotnet.FetchMedication L03_2=new fhirclient_dotnet.FetchMedication();
                    String result= L03_2.GetMedications(ServerAddress,IdentifierSystem,IdentifierValue);
                    tr.Add(test.Key,result);
            }
            return tr;
        }
        private Dictionary<string,string> L03_3_Tests()
        {
            MyConfiguration c=new MyConfiguration();
            String ServerAddress=c.IPSServerEndpoint;
            String IdentifierSystem=c.PatientIdentifierSystem;
             
            Dictionary<string,string> tl=new System.Collections.Generic.Dictionary<string, string>();
            Dictionary<string,string> tr=new System.Collections.Generic.Dictionary<string, string>();
            tl.Add("L03_3_T01","L03_3_T01");
            tl.Add("L03_3_T02","L03_3_T02");
            tl.Add("L03_3_T03","L03_3_T03");
            tl.Add("L03_3_T04","L03_3_T04");
            tl.Add("L03_3_T05","L03_3_T04");
            tl.Add("L03_3_T06","L03_3_T03");
            
            foreach(KeyValuePair<string,string> test in tl)
            {
          
                    String IdentifierValue=test.Value;
                    fhirclient_dotnet.FetchIPS L03_3=new fhirclient_dotnet.FetchIPS();
                    String kind="medications";
                    String result="";
                    if (test.Key=="L03_3_T05"){kind="immunizations";}
                    if (test.Key=="L03_3_T06"){kind="immunizations";}
                    if (kind=="immunizations")
                    {
                     result= L03_3.GetIPSImmunizations(ServerAddress,IdentifierSystem,IdentifierValue) ;  
                    }
                    else
                    {
                        result= L03_3.GetIPSMedications(ServerAddress,IdentifierSystem,IdentifierValue);
                    }

                    tr.Add(test.Key,result);
            }
            return tr;
        }

        public  class Immun {
        public String identifierValue;
        public String immunizationStatusCode;
        public String immunizationDateTime;
        public String productCVXCode;
        public String productCVXDisplay;
        public String reasonCode;

        public Immun(String IdentifierValue, String ImmunizationStatusCode, String ImmunizationDateTime,
                String ProductCVXCode, String ProductCVXDisplay, String ReasonCode

        ) {
            this.identifierValue = IdentifierValue;
            this.immunizationStatusCode = ImmunizationStatusCode;
            this.immunizationDateTime = ImmunizationDateTime;
            this.productCVXCode = ProductCVXCode;
            this.productCVXDisplay = ProductCVXDisplay;
            this.reasonCode = ReasonCode;
        }

    }
    public  class LabResult {
        public String identifierValue;
        public String observationStatusCode;
        public String observationDateTime;
        public String observationLOINCCode;
        public String observationLOINCDisplay;
        public String resultType;
        public String numericResultValue;
        public String numericResultUCUMUnit;
        public String codedResultSNOMEDCode;
        public String codedResultSNOMEDDisplay;

        public LabResult(String IdentifierValue, String ObservationStatusCode, String ObservationDateTime,
                String ObservationLOINCCode, String ObservationLOINCDisplay, String ResultType,
                String NumericResultValue, String NumericResultUCUMUnit, String CodedResultSNOMEDCode,
                String CodedResultSNOMEDDisplay) {
            this.identifierValue = IdentifierValue;
            this.observationStatusCode = ObservationStatusCode;
            this.observationDateTime = ObservationDateTime;
            this.observationLOINCCode = ObservationLOINCCode;
            this.observationLOINCDisplay = ObservationLOINCDisplay;
            this.resultType = ResultType;
            this.numericResultValue = NumericResultValue;
            this.numericResultUCUMUnit = NumericResultUCUMUnit;
            this.codedResultSNOMEDCode = CodedResultSNOMEDCode;
            this.codedResultSNOMEDDisplay = CodedResultSNOMEDDisplay;
        }

    }

        public static Dictionary<String, String> L04_1_Tests() {
        Dictionary<String, String> my_results = new Dictionary<String, String>();

        Dictionary<String, LabResult> my_tests = new Dictionary<String, LabResult>();
        my_tests.Add("L04_1_T01", new LabResult("L04_1_T01", "", "", "", "", "", "", "", "", ""));
        my_tests.Add("L04_1_T02", new LabResult("L04_1_T02", "final", "2020-10-11T20:30:00Z", "5778-6",
                "Color of Urine", "coded", "", "", "371244009", "Yellow"));
        my_tests.Add("L04_1_T03", new LabResult("L04_1_T02", "final", "2020-10-11T20:30:00Z", "1975-2",
                "Bilirubin, blood", "numeric", "8.6", "mg/dl", "", ""));

        MyConfiguration c=new MyConfiguration();
        String ServerAddress=c.ServerEndpoint;
        String IdentifierSystem=c.PatientIdentifierSystem;
        
            foreach(KeyValuePair<string,LabResult> test in my_tests)
            {
                String aux="";
                String str = test.Key;
                LabResult l = test.Value;
                String IdentifierValue = l.identifierValue;
                String ObservationStatusCode = l.observationStatusCode;
                String ObservationDateTime = l.observationDateTime;
                String ObservationLOINCCode = l.observationLOINCCode;
                String ObservationLOINCDisplay = l.observationLOINCDisplay;
                String ResultType = l.resultType;
                String NumericResultValue = l.numericResultValue;
                String NumericResultUCUMUnit = l.numericResultUCUMUnit;
                String CodedResultSNOMEDCode = l.codedResultSNOMEDCode;
                String CodedResultSNOMEDDisplay = l.codedResultSNOMEDDisplay;
                fhirclient_dotnet.CreateUSCoreObs L04_1=new fhirclient_dotnet.CreateUSCoreObs();
                try {

                    aux = L04_1.CreateUSCoreR4LabObservation(ServerAddress, IdentifierSystem, IdentifierValue,
                    ObservationStatusCode, ObservationDateTime, ObservationLOINCCode, ObservationLOINCDisplay,
                    ResultType, NumericResultValue, NumericResultUCUMUnit, CodedResultSNOMEDCode,
                    CodedResultSNOMEDDisplay);
                    if(aux!="")
                    {
                        if (str!="L04_1_T01")
                        {
                          aux=ValidateObservationUSCORE(aux,ServerAddress);
                        }
                        
                    }
                }
                catch(Exception e){aux=e.Message;}
                
                my_results.Add(test.Key,aux);


        }
        return my_results;
    }

    
        public static Dictionary<String, String> L04_2_Tests() {
        Dictionary<String, String> my_results = new Dictionary<String, String>();

        Dictionary<String, Immun> my_tests = new Dictionary<String, Immun>();
        my_tests.Add("L04_2_T01", new Immun("L04_2_T01", "", "", "", "", ""));
        my_tests.Add("L04_2_T02", new Immun("L04_2_T02", "completed", "2021-10-25", "173", "", "cholera, BivWC"));
        my_tests.Add("L04_2_T03", new Immun("L04_2_T02", "not-done", "2021-10-30T10:30:00Z", "207",
                "COVID-19, mRNA, LNP-S, PF, 100 mcg/0.5 mL dose", "171283008"));
        MyConfiguration c=new MyConfiguration();
        String ServerAddress=c.ServerEndpoint;
        String IdentifierSystem=c.PatientIdentifierSystem;
        
            foreach(KeyValuePair<string,Immun> test in my_tests)
            {
                String aux="";
                String str = test.Key;
                Immun i = test.Value;
                String IdentifierValue = i.identifierValue;
                String ImmunizationStatusCode = i.immunizationStatusCode;
                String ImmunizationDateTime = i.immunizationDateTime;
                String ProductCVXCode = i.productCVXCode;
                String ProductCVXDisplay = i.productCVXDisplay;
                String ReasonCode = i.reasonCode;
                try {
                     

                    fhirclient_dotnet.CreateUSCoreImm L04_2=new fhirclient_dotnet.CreateUSCoreImm();
                 
                    aux = L04_2.CreateUSCoreR4Immunization(ServerAddress, IdentifierSystem, IdentifierValue,
                    ImmunizationStatusCode, ImmunizationDateTime, ProductCVXCode, ProductCVXDisplay, ReasonCode);
                    if(aux!="")
                    {
                        if (str!="L04_2_T01")
                        {
                          aux=ValidateImmunizationUSCORE(aux,ServerAddress);
                        }
                        
                    }
                }
                catch(Exception e){aux=e.Message;}
                
                my_results.Add(test.Key,aux);
            }
            
        return my_results;
    }
        public static string ValidateImmunizationUSCORE(string JsonImmunization,string server)
        {
             string aux="";
             Hl7.Fhir.Model.Immunization o=new  Hl7.Fhir.Model.Immunization() ;
             var parser = new Hl7.Fhir.Serialization.FhirJsonParser();
          
            try
            {
                o = parser.Parse<Immunization>(JsonImmunization);
            }
            catch
            {
                aux="Error:Invalid_Immunization_Resource";
            }

            if (aux=="")
            {
                var client = new Hl7.Fhir.Rest.FhirClient(server); 
                if (o.Meta.Profile is null)
                   o.Meta.Profile = new List<string> { "http://hl7.org/fhir/us/core/StructureDefinition/us-core-immunization" };
                   
                Parameters inParams = new Parameters();
                inParams.Add("resource", o);
                OperationOutcome bu = client.ValidateResourceAsync(o).GetAwaiter().GetResult(); 
                aux="OK";
                if (bu.Issue[0].Details.Text!="Validation successful, no issues found")
                {
                    aux="Error:"+bu.Issue[0].Details.Text;
                    if (aux=="Error:All OK")
                    {
                        aux="OK";
                    }
                }
            }
            return aux;

    }
   public static string ValidateObservationUSCORE(string JsonObservation,string server)
        {
             string aux="";
             Hl7.Fhir.Model.Observation o=new  Hl7.Fhir.Model.Observation() ;
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
                if (o.Meta.Profile is null)
                   o.Meta.Profile = new List<string> { "http://hl7.org/fhir/us/core/StructureDefinition/us-core-observation-lab" };
                   
                Parameters inParams = new Parameters();
                inParams.Add("resource", o);
                OperationOutcome bu = client.ValidateResourceAsync(o).GetAwaiter().GetResult(); 
                aux="OK";
                if (bu.Issue[0].Details.Text!="Validation successful, no issues found")
                {
                    aux="Error:"+bu.Issue[0].Details.Text;
                    if (aux=="Error:All OK")
                    {
                        aux="OK";
                    }
                }
            }
            return aux;

    }
    
        private Dictionary<string,string> L05_1_Tests()
        {
            MyConfiguration c=new MyConfiguration();
            String ServerAddress=c.TerminologyServerEndpoint;
            
             
            Dictionary<string,string> tl=new System.Collections.Generic.Dictionary<string, string>();
            tl.Add("L05_1_T01", "xxxxxxxxx");
            tl.Add("L05_1_T02","Drug-induced diabetes");
            String url="http://snomed.info/sct?fhir_vs=isa/73211009";

            Dictionary<string, string> tr = new System.Collections.Generic.Dictionary<string, string>();
            foreach (KeyValuePair<string, string> test in tl)
            {
                String filter = test.Value;
                var fsh = new TerminologyService();
                String result = fsh.ExpandValueSetForCombo(
                     ServerAddress,
                     url,
                     filter);
                tr.Add(test.Key, result);

            }
            return tr;
         
        }
        
        private Dictionary<string,string> L02_1_Tests()
        {
            MyConfiguration c=new MyConfiguration();
            String ServerAddress=c.ServerEndpoint;
            String IdentifierSystem=c.PatientIdentifierSystem;
             
            Dictionary<string,string> tl=new System.Collections.Generic.Dictionary<string, string>();
            Dictionary<string,string> tr=new System.Collections.Generic.Dictionary<string, string>();
            tl.Add("L02_1_T01","L02_1_T01");
            tl.Add("L02_1_T02","L02_1_T02");
            tl.Add("L02_1_T03","L02_1_T03");
            tl.Add("L02_1_T04","L02_1_T04");
            tl.Add("L02_1_T05","L02_1_T05");
            
            
            foreach(KeyValuePair<string,string> test in tl)
            {
          
                    String IdentifierValue=test.Value;
                    fhirclient_dotnet.FetchEthnicity L02_1=new fhirclient_dotnet.FetchEthnicity();
                    

                    String result= L02_1.GetEthnicity(ServerAddress,IdentifierSystem,IdentifierValue);
                    tr.Add(test.Key,result);
            }
            return tr;
        }    
        public Dictionary<String,String> AddAll(Dictionary<String,String> tAll,Dictionary<String,String> tOne)
        {
            Dictionary<string,string> tl=new System.Collections.Generic.Dictionary<string, string>();
            foreach(KeyValuePair<string,string> test in tAll)
            {
                tl.Add(test.Key,test.Value);

            }
            foreach(KeyValuePair<string,string> test in tOne)
            {
                Console.WriteLine("adding...");
                Console.WriteLine(test.Key);     
                tl.Add(test.Key,test.Value);

            }
            return tl;  

        }
        public String CreateSubmission()
        {
           
            MyConfiguration c=new MyConfiguration();


            Dictionary<string,string> TestList=new System.Collections.Generic.Dictionary<string, string>();
            TestList=AddAll (TestList,L01_1_Tests());
            TestList=AddAll (TestList,L01_2_Tests());
            TestList=AddAll (TestList,L01_3_Tests());
            TestList=AddAll (TestList,L02_1_Tests());
            TestList=AddAll (TestList,L03_1_Tests());
            TestList=AddAll (TestList,L03_2_Tests());
            TestList=AddAll (TestList,L03_3_Tests());
            TestList=AddAll (TestList,L04_1_Tests());
            TestList=AddAll (TestList,L04_2_Tests());
            TestList=AddAll (TestList,L05_1_Tests());
                  
          
            
            Hl7.Fhir.Model.TestReport  tr=new  Hl7.Fhir.Model.TestReport();
            //Identifier
            //tr.Identifier.System="http://fhirintermediate.org/test_report";
            //tr.Identifier.Value="1";
            //In progress
            tr.Status=TestReport.TestReportStatus.InProgress;
            ResourceReference r=new ResourceReference();
            r.Identifier=new Identifier("http://fhirintermediate.org/test_script/id","FHIR_INTERMEDIATE_U02-.NET");
            tr.TestScript=r;
            tr.Tester=c.StudentId;

            TestReport.ParticipantComponent pcS=new TestReport.ParticipantComponent();
            pcS.Type=TestReport.TestReportParticipantType.Server;
            pcS.Uri=c.ServerEndpoint;
            pcS.Display="Resource Server";
            tr.Participant.Add(pcS);
            
            TestReport.ParticipantComponent pcTS=new TestReport.ParticipantComponent();
            pcTS.Type=TestReport.TestReportParticipantType.Server;
            pcTS.Uri=c.TerminologyServerEndpoint;
            pcTS.Display="Terminology Server";
            tr.Participant.Add(pcTS);
            
            TestReport.ParticipantComponent pcC=new TestReport.ParticipantComponent();
            pcC.Type=TestReport.TestReportParticipantType.Client;
            pcC.Uri="http://localhost";
            pcC.Display=c.StudentName;
            tr.Participant.Add(pcC);

            foreach(KeyValuePair<string,string> test in TestList)
            {
                
                TestReport.TestComponent testComponent=new TestReport.TestComponent();
                testComponent.Name=test.Key;
                testComponent.Description=test.Key;
                TestReport.TestActionComponent ta=new TestReport.TestActionComponent();
                TestReport.AssertComponent tac=new TestReport.AssertComponent();
                tac.Result=TestReport.TestReportActionResult.Pass;
                tac.Message=new Markdown(test.Value);
                ta.Assert=tac;
                testComponent.Action.Add(ta);
                tr.Test.Add(testComponent);
            }
            
            
            String results = System.Text.Json.JsonSerializer.Serialize(tr, new System.Text.Json.JsonSerializerOptions().ForFhir());  
            String filename=@"FHIR_INTERMEDIATE_U2_SUBMISSION_" + c.StudentId+".JSON";
            System.IO.File.WriteAllText(filename, results);
            return filename;


        }
        
      

   }
}