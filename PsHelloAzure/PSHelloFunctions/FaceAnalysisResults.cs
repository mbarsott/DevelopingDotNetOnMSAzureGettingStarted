using Microsoft.ProjectOxford.Face.Contract;

namespace PSHelloFunctions
{
    public class FaceAnalysisResults
    {
        public string Id { get; set; }
        public Face[] Faces { get; set; }
    }
}