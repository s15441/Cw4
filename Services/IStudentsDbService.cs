using Cw4.DTOs.Requests;
using Cw4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw4.Services
{
    interface IStudentsDbService
    {
        Enrollment EnrollStudent(EnrollStudentRequest request);
        Enrollment PromoteStudents(PromoteRequest request);
    }
}
