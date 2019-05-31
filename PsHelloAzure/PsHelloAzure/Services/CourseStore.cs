using Microsoft.Azure.Documents.Client;
using PsHelloAzure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsHelloAzure.Services
{
    public class CourseStore
    {
        private DocumentClient client;
        private Uri coursesLink;

        public CourseStore()
        {
            var uri = new Uri("https://pshelloazuredb.documents.azure.com:443/");
            var key = "Je9HiLUWamLkosC1nomt9ztGlnDATGzitB1aL1Npyn7uSHKnRJ4XceOKJ5hebL6zoUQiiKlaKHJHdlwqHGyqUA==";
            client = new DocumentClient(uri, key);
            coursesLink = UriFactory.CreateDocumentCollectionUri("pshelloazuredb", "courses");
        }

        public async Task InsertCourses(IEnumerable<Course> courses)
        {
            foreach (var course in courses)
            {
                await client.CreateDocumentAsync(coursesLink, course);
            }
        }

        public IEnumerable<Course> GetAllCourses()
        {
            var courses = client.CreateDocumentQuery<Course>(coursesLink)
                                .OrderBy(c => c.Title);

            return courses;
        }
    }
}
