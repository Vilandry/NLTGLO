﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTGLO.Model
{
    public class Course : Entity
    {
        public string Name;
        public string CourseCode;
        public Instructor[] Instructors;

    }
}
