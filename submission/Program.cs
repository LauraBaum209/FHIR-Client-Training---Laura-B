using System;
using fhirclient_dotnet;
namespace fhirclient_dotnet_submission
{
    class Program
    {
        static void Main(string[] args)
        {
            SubmissionCreator submission=new SubmissionCreator();
            String filename=submission.CreateSubmission();
            Console.WriteLine(filename);
        }
    }
}
