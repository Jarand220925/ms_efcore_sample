// See https://aka.ms/new-console-template for more information

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ms_efcore_sample.models;
using ms_efcore_sample.runs;

//-------------------------------------------
//var msSample = Ms_Sample.RunThis();

//-------------------------------------------

//await CoordinateContextPractice.RunThis();

//Lag en ny CoordinateNoPoint og legg til i databasen.
//await CoordinateContextPractice.AddNoPoint();

//Lag en Coordinate-model og sjekk verdiene på den.
await CoordinateContextPractice.CreateAndLook();

await CoordinateContextPractice.CreateAndLookGeojson();
