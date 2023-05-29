using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserAuthenticationService.Data.Data;

namespace UserAuthenticationService.Data.Utilities
{
    public class ValidationFunctions
    {
         public static bool IsValidNigerianPhoneNumber(string phoneNumber)
         {
             // Remove any whitespace or special characters from the phone number
             phoneNumber = Regex.Replace(phoneNumber, @"[\s\-\(\)]", "");
             if (phoneNumber.Length > 0 && phoneNumber[0] != '0')
             {
                 phoneNumber = "0" + phoneNumber;
             }

             // Check if the phone number matches the Nigerian phone number format with or without leading "+234"
             bool isMatch = Regex.IsMatch(phoneNumber, @"^(234|0)(7[01]|8[01]|9[01])\d{8}$");

             // Check if the phone number has a length of 10 or 11 digits
             bool isValidLength = (phoneNumber.Length == 10 || phoneNumber.Length == 11);

             // Return true if the phone number matches the format and has a valid length
             return isMatch && isValidLength;
         }

         // Define a function to calculate the similarity between two users based on their likes
         public static double CalculateSimilarity(ApiUser currentUser, ApiUser matchedUser)
         {
             string[] currentUserInterests = SplitByDash(currentUser.Interests);
             string[] matchedUserInterests = SplitByDash(matchedUser.Interests);
             var sharedInterests = currentUserInterests.Intersect(matchedUserInterests).ToList();
             var totalInterests = currentUserInterests.Union(currentUserInterests).ToList();
             //return (double)sharedInterests.Count / (double)totalInterests.Count;

             int ageDifference = Math.Abs(currentUser.Age.GetValueOrDefault() - matchedUser.Age.GetValueOrDefault());
             int agePoints = 5; // start with 5 points

             if (ageDifference > 18)
             {
                 agePoints = 0; // set to 0 if age difference is greater than 18
             }
             else if (ageDifference % 3 == 0)
             {
                 agePoints = Math.Max(0, agePoints - (ageDifference / 3)); // decrement points for every multiple of 3
             }
             else
             {
                 agePoints = Math.Max(0, agePoints - (ageDifference / 3) - 1); // decrement points for every multiple of 3 except for exact multiples
             }

             return (double)(sharedInterests.Count + agePoints) / (double)(totalInterests.Count + 1);
         }

         public static string[] SplitByDash(string input)
         {
             string[] words = input.Split('-');
             return words;
         }
    }
}


