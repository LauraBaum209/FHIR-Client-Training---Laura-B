using System;
using Hl7.Fhir.Model;  
using Hl7.Fhir.Rest;

namespace fhirclient_dotnet
{
    public class FetchEthnicity
    {
        public string GetEthnicity
        (string ServerEndPoint,
         string IdentifierSystem,
         string IdentifierValue
         )
         {
            return "";
         }
    
     private string GetEthnicityExtension(Hl7.Fhir.Model.Patient p)
    {   
        string auxt="";
        string auxo="";
        string auxd="";
        string ethnicityExtensionUrl="http://hl7.org/fhir/us/core/StructureDefinition/us-core-ethnicity";
        string aux="Error:No_us-core-ethnicity_Extension";
        System.Collections.Generic.List<Extension> e=p.Extension;
        foreach(Extension ef in e)
        {
            if (ef.Url==ethnicityExtensionUrl)
            {
                aux="";
                foreach(Extension efs in ef.Extension)
                {
                    switch(efs.Url)
                    {
                        case "text":
                        {
                             auxt="text|"+efs.Value.ToString()+"\n";
                             break;
                        }
                        case "ombCategory":
                        {
                            Coding c=(Coding) efs.Value;
                            auxo="code|"+c.Code+":"+c.Display+"\n";
                            break;
                        }
                        case "detailed":
                        {
                            Coding c=(Coding) efs.Value;
                            auxd+="detail|"+c.Code+":"+c.Display+"\n";
                             break;
                        }   
                    }
                    
                }
                aux+=auxt;
                aux+=auxo;
                aux+=auxd;
                break;    
            }
            
        }
        if ((auxt=="") || (auxo==""))
        {
            aux="Error:Non_Conformant_us-core-ethnicity_Extension";
        }
        return aux;
    }
     private Hl7.Fhir.Model.Patient FHIR_SearchByIdentifier(string ServerEndPoint, string IdentifierSystem,string IdentifierValue)
        {
             Hl7.Fhir.Model.Patient  o=new  Hl7.Fhir.Model.Patient() ;
            var client = new Hl7.Fhir.Rest.FhirClient(ServerEndPoint); 
            Bundle bu = client.SearchAsync<Hl7.Fhir.Model.Patient>(new string[]
             { "identifier=" + IdentifierSystem + "|" + IdentifierValue }).GetAwaiter().GetResult();  
            if (bu.Entry.Count>0)
                    {
                        o=(Hl7.Fhir.Model.Patient)bu.Entry[0].Resource;
                    }
            else
                {o=null;}
            return o;
        }
        
    }
}
