using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Data.Utilities
{
    public class Enums
    {
        public enum OnboardingStep
        {
            RegisterEmail,
            UpdateMobileNumber,
            UpdateName,
            UpdateBirthday,
            UpdateGender,
            UpdatePhotos,
            UpdateInterests,
            Done
        }
    }
}
