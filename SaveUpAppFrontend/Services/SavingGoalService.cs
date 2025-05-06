// SavingsGoalService.cs
using System;

namespace SaveUpAppFrontend.Services
{
    public class SavingsGoalService
    {
        public event Action<double> SavingGoalChanged;

        private double _savingGoal;

        public double SavingGoal
        {
            get => _savingGoal;
            set
            {
                if (Math.Abs(_savingGoal - value) > 0.01) // Vermeide unnötige Updates bei kleinen Änderungen
                {
                    _savingGoal = value;
                    Preferences.Set("SavingGoal", value);
                    SavingGoalChanged?.Invoke(value);
                }
            }
        }

        public SavingsGoalService()
        {
            _savingGoal = Preferences.Get("SavingGoal", 0.0);
        }
    }
}