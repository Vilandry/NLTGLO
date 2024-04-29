﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTGLO.Model
{
    public class Student : Entity
    {
        public string Name;
        public string Neptun;
        public Instructor Supervisor;
        public Course ExamCourse;
        public int TimeSlot;
    }
}
